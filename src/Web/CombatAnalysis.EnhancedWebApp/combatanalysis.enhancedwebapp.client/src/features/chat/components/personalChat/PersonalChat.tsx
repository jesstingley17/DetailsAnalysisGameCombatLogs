import APP_CONFIG from '@/config/appConfig';
import Loading from '@/shared/components/Loading';
import { useChatHub } from '@/shared/hooks/useChatHub';
import logger from '@/utils/Logger';
import { memo, useEffect, useRef, useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../user/api/Account.api';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useGetMessagesByPersonalChatIdQuery, useLazyGetMessagesByPersonalChatIdQuery } from '../../api/Chat.api';
import {
    useGetPersonalChatMessageCountByChatIdQuery,
    useUpdatePersonalChatMessageMutation
} from '../../api/PersonalChatMessage.api';
import type { GroupChatMessageModel } from '../../types/GroupChatMessageModel';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { PersonalChatMessageModel } from '../../types/PersonalChatMessageModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';
import ChatMessage from '../ChatMessage';
import MessageInput from '../MessageInput';
import PersonalChatTitle from './PersonalChatTitle';

import './PersonalChat.scss';

interface PersonalChatProps {
    myself: AppUserModel;
    chat: PersonalChatModel;
    setSelectedChat: (value: SetStateAction<PersonalChatModel | GroupChatModel | null>) => void;
    companionId: string;
}

const PersonalChat: React.FC<PersonalChatProps> = ({ myself, chat, setSelectedChat, companionId }) => {
    const { t } = useTranslation('communication/chats/personalChat');

    const chatHub = useChatHub();

    let page = 1;

    const chatContainerRef = useRef<HTMLUListElement | null>(null);
    const pageSizeRef = useRef<number>(APP_CONFIG.communication.chatPageSize ? +APP_CONFIG.communication.chatPageSize : 5);

    const [haveMoreMessages, setHaveMoreMessage] = useState(false);
    const [currentMessages, setCurrentMessages] = useState<PersonalChatMessageModel[]>([]);
    const [messagesIsLoaded, setMessagesIsLoaded] = useState(false);
    const [areLoadingOldMessages, setAreLoadingOldMessages] = useState(true);

    const { data: count, isLoading: countIsLoading } = useGetPersonalChatMessageCountByChatIdQuery(chat.id);
    const { data: messages, isLoading } = useGetMessagesByPersonalChatIdQuery({
        chatId: chat.id,
        page: 1,
        pageSize: pageSizeRef.current
    });
    const [getMessagesByPersonalChatIdAsync] = useLazyGetMessagesByPersonalChatIdQuery();

    const { data: companion, isLoading: companionIsLoading } = useGetUserByIdQuery(companionId);
    const [updatePersonalChatMessage] = useUpdatePersonalChatMessageMutation();

    useEffect(() => {
        setCurrentMessages([]);

        if (!chatHub) {
            return;
        }

        (async () => {
            await chatHub.connectToPersonalChatMessagesAsync(chat.id);

            chatHub.subscribeToPersonalChatMessages((message: PersonalChatMessageModel) => {
                setCurrentMessages(prevMessages => [...prevMessages, message]);
            });
        })();

        return () => {
            (async () => {
                await chatHub.disconnectFromGroupChatMessageHubAsync();
            })();
        }
    }, [chat]);

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
        try {
            await updatePersonalChatMessage({ id: message.id, message: message as PersonalChatMessageModel }).unwrap();
        } catch (e) {
            logger.error("Failed to update personal chat message", e);
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

    const getMoreMessagesAsync = async (page: number) => {
        const arg = {
            chatId: chat.id,
            page,
            pageSize: pageSizeRef.current
        };

        const response = await getMessagesByPersonalChatIdAsync(arg);
        if (response.data) {
            return response.data;
        }

        return [];
    }

    const handleLoadMoreMessagesAsync = async () => {
        setAreLoadingOldMessages(true);

        page++;

        const moreMessages = await getMoreMessagesAsync(page);

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
                                    user={myself}
                                    reviewerId={myself.id}
                                    chatUserAsUserId={message.appUserId}
                                    chatUserUsername={message.username}
                                    messageOwnerId={message.appUserId}
                                    message={message}
                                    updateMessageAsync={updateMessageAsync}
                                    hubConnection={chatHub.personalChatMessagesHubConnectionRef.current}
                                    subscribeToChatMessageHasBeenRead={chatHub.subscribeToPersonalMessageHasBeenRead}
                                />
                            </li>
                    ))}
                </ul>
                <MessageInput
                    chatId={chat.id}
                    initiator={myself}
                    setAreLoadingOldMessages={setAreLoadingOldMessages}
                    t={t}
                    companionId={companionId}
                />
            </div>
        </div>
    );
}

export default memo(PersonalChat);