import { memo, useEffect, useRef, useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import { useChatHub } from '../../../../context/ChatHubProvider';
import Loading from '../../../../shared/components/Loading';
import { useGetUserByIdQuery } from '../../../user/api/Account.api';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useGetMessagesByPersonalChatIdQuery, useLazyGetMoreMessagesByPersonalChatIdQuery } from '../../api/Chat.api';
import {
    useGetPersonalChatMessageCountByChatIdQuery,
    useUpdatePersonalChatMessageMutation
} from '../../api/PersonalChatMessage.api';
import type { GroupChatMessageModel } from '../../types/GroupChatMessageModel';
import type { PersonalChatMessageModel } from '../../types/PersonalChatMessageModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';
import type { SelectedChatModel } from '../../types/SelectedChatModel';
import ChatMessage from '../ChatMessage';
import MessageInput from '../MessageInput';
import PersonalChatTitle from './PersonalChatTitle';

import '../../../../styles/communication/chats/personalChat.scss';

interface PersonalChatProps {
    myself: AppUserModel;
    chat: PersonalChatModel;
    setSelectedChat(value: SetStateAction<SelectedChatModel>): void;
    companionId: string;
}

const PersonalChat: React.FC<PersonalChatProps> = ({ myself, chat, setSelectedChat, companionId }) => {
    const { t } = useTranslation("communication/chats/personalChat");

    const chatHub = useChatHub();

    const chatContainerRef = useRef<HTMLUListElement | null>(null);
    const pageSizeRef = useRef<number>(process.env.REACT_APP_CHAT_PAGE_SIZE ? +process.env.REACT_APP_CHAT_PAGE_SIZE : 1);

    const [haveMoreMessages, setHaveMoreMessage] = useState(false);
    const [currentMessages, setCurrentMessages] = useState<PersonalChatMessageModel[]>([]);
    const [messagesIsLoaded, setMessagesIsLoaded] = useState(false);
    const [areLoadingOldMessages, setAreLoadingOldMessages] = useState(true);

    const { data: count, isLoading: countIsLoading } = useGetPersonalChatMessageCountByChatIdQuery(chat.id);
    const { data: messages, isLoading } = useGetMessagesByPersonalChatIdQuery({
        chatId: chat.id,
        pageSize: pageSizeRef.current
    });
    const [getMoreMessagesByPersonalChatIdAsync] = useLazyGetMoreMessagesByPersonalChatIdQuery();

    const { data: companion, isLoading: companionIsLoading } = useGetUserByIdQuery(companionId);
    const [updatePersonalChatMessage] = useUpdatePersonalChatMessageMutation();

    useEffect(() => {
        if (!chatHub) {
            return;
        }

        const connectToPersonalChatMessages = async () => {
            await chatHub.connectToPersonalChatMessagesAsync(chat.id);
        }

        connectToPersonalChatMessages();
    }, []);

    useEffect(() => {
        if (!chatHub || !chatHub.personalChatMessagesHubConnection) {
            return;
        }

        chatHub.subscribeToPersonalChatMessages((message: PersonalChatMessageModel) => {
            setCurrentMessages(prevMessages => [...prevMessages, message]);
        });
    }, [chatHub?.personalChatMessagesHubConnection]);

    useEffect(() => {
        if (!messages) {
            return;
        }

        setCurrentMessages(messages);
    }, [messages]);

    useEffect(() => {
        if (!messages) {
            return;
        }

        const handleScroll = () => {
            const chatContainer: HTMLUListElement | null = chatContainerRef.current;
            if (!chatContainer) {
                return;
            }

            if (chatContainer.scrollTop === 0) {
                const moreMessagesCount = count ?? 0 - currentMessages.length + messages.length - pageSizeRef.current;
                setHaveMoreMessage(moreMessagesCount > 0);
            }
            else if (chatContainer.scrollHeight - chatContainer.scrollTop === chatContainer.clientHeight) {
                setHaveMoreMessage(false);
            }
        }

        const scrollContainer: HTMLUListElement | null = chatContainerRef.current;
        scrollContainer?.addEventListener("scroll", handleScroll);

        return () => {
            scrollContainer?.removeEventListener("scroll", handleScroll);
        }
    }, [currentMessages, messages]);

    useEffect(() => {
        if (!currentMessages || messagesIsLoaded) {
            return;
        }

        scrollToBottom();

        setMessagesIsLoaded(true);
    }, [currentMessages]);

    useEffect(() => {
        if (!currentMessages || areLoadingOldMessages) {
            return;
        }

        scrollToBottom();
    }, [currentMessages]);

    const updateMessageAsync = async (message: PersonalChatMessageModel | GroupChatMessageModel) => {
        if ("isRead" in message) {
            await updatePersonalChatMessage(message);
        }
    }

    const saveScrollState = () => {
        const chatContainer: HTMLUListElement | null = chatContainerRef.current;
        if (!chatContainer) {
            return;
        }

        const previousScrollHeight = chatContainer?.scrollHeight;
        const previousScrollTop = chatContainer?.scrollTop;

        setTimeout(() => {
            chatContainer.scrollTop = chatContainer.scrollHeight - previousScrollHeight + previousScrollTop;
        }, 0);
    }

    const scrollToBottom = () => {
        const chatContainer = chatContainerRef.current;
        if (chatContainer !== null) {
            chatContainer.scrollTop = chatContainer.scrollHeight;
        }
    }

    const getMoreMessagesAsync = async (offset: number) => {
        const arg = {
            chatId: chat.id,
            offset,
            pageSize: pageSizeRef.current
        };

        const response = await getMoreMessagesByPersonalChatIdAsync(arg);
        if (response.data) {
            return response.data;
        }

        return [];
    }

    const handleLoadMoreMessagesAsync = async () => {
        setAreLoadingOldMessages(true);

        const moreMessages = await getMoreMessagesAsync(currentMessages.length);

        setCurrentMessages(prevMessages => [...moreMessages, ...prevMessages]);

        saveScrollState();
    }

    if (!chatHub || isLoading || companionIsLoading || countIsLoading) {
        return (
            <div className="chats__selected-chat_loading">
                <Loading />
            </div>
        );
    }

    return (
        <div className="chats__selected-chat personal-chat">
            <div className="messages-container">
                <PersonalChatTitle
                    chat={chat}
                    companionUsername={companion?.username ?? ""}
                    setSelectedChat={setSelectedChat}
                    haveMoreMessages={haveMoreMessages}
                    setHaveMoreMessage={setHaveMoreMessage}
                    loadMoreMessagesAsync={handleLoadMoreMessagesAsync}
                    t={t}
                />
                <ul className="chat-messages" ref={chatContainerRef}>
                    {currentMessages?.map((message) => (
                            <li key={message.id}>
                                <ChatMessage
                                    myself={myself}
                                    reviewerId={myself.id}
                                    chatUserAsUserId={myself.id}
                                    chatUserUsername={myself.username}
                                    messageOwnerId={message.appUserId}
                                    message={message}
                                    updateMessageAsync={updateMessageAsync}
                                    hubConnection={chatHub.personalChatMessagesHubConnection}
                                    subscribeToChatMessageHasBeenRead={chatHub.subscribeToPersonalMessageHasBeenRead}
                                />
                            </li>
                    ))}
                </ul>
                <MessageInput
                    chatId={chat.id}
                    IasGroupChatUser={myself}
                    setAreLoadingOldMessages={setAreLoadingOldMessages}
                    targetChatType={0}
                    t={t}
                    companionsId={[companionId]}
                />
            </div>
        </div>
    );
}

export default memo(PersonalChat);