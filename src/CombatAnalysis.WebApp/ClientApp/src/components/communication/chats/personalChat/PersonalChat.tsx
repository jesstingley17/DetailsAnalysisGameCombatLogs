import { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useChatHub } from '../../../../context/ChatHubProvider';
import {
    useGetPersonalChatMessageCountByChatIdQuery,
    useUpdatePersonalChatMessageAsyncMutation
} from '../../../../store/api/chat/PersonalChatMessage.api';
import { useGetMessagesByPersonalChatIdQuery, useLazyGetMoreMessagesByPersonalChatIdQuery } from '../../../../store/api/core/Chat.api';
import { useGetUserByIdQuery } from '../../../../store/api/user/Account.api';
import { GroupChatMessage } from '../../../../types/GroupChatMessage';
import { PersonalChatMessage } from '../../../../types/PersonalChatMessage';
import { PersonalChatProps } from '../../../../types/components/communication/chats/PersonalChatProps';
import Loading from '../../../Loading';
import ChatMessage from '../ChatMessage';
import MessageInput from '../MessageInput';
import PersonalChatTitle from './PersonalChatTitle';

import '../../../../styles/communication/chats/personalChat.scss';

const PersonalChat: React.FC<PersonalChatProps> = ({ me, chat, setSelectedChat, companionId }) => {
    const { t } = useTranslation("communication/chats/personalChat");

    const chatHub = useChatHub();

    const chatContainerRef = useRef<HTMLUListElement | null>(null);
    const pageSizeRef = useRef<any>(process.env.REACT_APP_CHAT_PAGE_SIZE);

    const [haveMoreMessages, setHaveMoreMessage] = useState(false);
    const [currentMessages, setCurrentMessages] = useState<PersonalChatMessage[]>([]);
    const [messagesIsLoaded, setMessagesIsLoaded] = useState(false);
    const [areLoadingOldMessages, setAreLoadingOldMessages] = useState(true);

    const { data: count, isLoading: countIsLoading } = useGetPersonalChatMessageCountByChatIdQuery(chat.id);
    const { data: messages, isLoading } = useGetMessagesByPersonalChatIdQuery({
        chatId: chat.id,
        pageSize: pageSizeRef.current
    });
    const [getMoreMessagesByPersonalChatIdAsync] = useLazyGetMoreMessagesByPersonalChatIdQuery();

    const { data: companion, isLoading: companionIsLoading } = useGetUserByIdQuery(companionId);
    const [updateChatMessage] = useUpdatePersonalChatMessageAsyncMutation();

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

        chatHub.subscribeToPersonalChatMessages((message: PersonalChatMessage) => {
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
            const chatContainer: any = chatContainerRef.current;
            if (chatContainer.scrollTop === 0) {
                const moreMessagesCount = count - currentMessages.length + messages.length - pageSizeRef.current;
                setHaveMoreMessage(moreMessagesCount > 0);
            }
            else if (chatContainer.scrollHeight - chatContainer.scrollTop === chatContainer.clientHeight) {
                setHaveMoreMessage(false);
            }
        }

        const scrollContainer: any = chatContainerRef.current;
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

    const updateMessageAsync = async (message: PersonalChatMessage | GroupChatMessage) => {
        await updateChatMessage(message);
    }

    const saveScrollState = () => {
        const chatContainer: any = chatContainerRef.current;
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
                    companionUsername={companion.username}
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
                                    chatType={0}
                                    me={me}
                                    meInChatId={me.id}
                                    reviewerId={me.id}
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
                    meInChat={me}
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