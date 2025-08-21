import { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useChatHub } from '../../../../context/ChatHubProvider';
import useGroupChatData from '../../../../hooks/useGroupChatData';
import { useUpdateGroupChatMessageAsyncMutation } from '../../../../store/api/chat/GroupChatMessage.api';
import { GroupChatMessage as GroupChatMessageModel } from '../../../../types/GroupChatMessage';
import { PersonalChatMessage } from '../../../../types/PersonalChatMessage';
import { GroupChatProps } from '../../../../types/components/communication/chats/GroupChatProps';
import Loading from '../../../Loading';
import MessageInput from '../MessageInput';
import GroupChatMenu from './GroupChatMenu';
import GroupChatTitle from './GroupChatTitle';

import '../../../../styles/communication/chats/groupChat.scss';
import ChatMessage from '../ChatMessage';

const GroupChat: React.FC<GroupChatProps> = ({ myself, chat, setSelectedChat }) => {
    const { t } = useTranslation("communication/chats/groupChat");

    const chatHub = useChatHub();

    const [settingsIsShow, setSettingsIsShow] = useState(false);
    const [groupChatUsersId, setGroupChatUsersId] = useState<string[]>([]);

    const [haveMoreMessages, setHaveMoreMessage] = useState(false);
    const [currentMessages, setCurrentMessages] = useState<GroupChatMessageModel[]>([]);
    const [messagesIsLoaded, setMessagesIsLoaded] = useState(false);
    const [areLoadingOldMessages, setAreLoadingOldMessages] = useState(true);

    const chatContainerRef = useRef<HTMLUListElement | null>(null);
    const pageSizeRef = useRef<any>(process.env.REACT_APP_CHAT_PAGE_SIZE);

    const { groupChatData, getMoreMessagesAsync } = useGroupChatData(chat.id, myself.id, pageSizeRef);

    const [updateGroupChatMessage] = useUpdateGroupChatMessageAsyncMutation();

    useEffect(() => {
        if (!chatHub) {
            return;
        }

        const connectToGroupChatMessages = async () => {
            await chatHub.connectToGroupChatMessagesAsync(chat.id);
        }

        connectToGroupChatMessages();
    }, []);

    useEffect(() => {
        if (!chatHub || !chatHub.groupChatMessagesHubConnection) {
            return;
        }

        chatHub.subscribeGroupChatUser({});

        chatHub.subscribeToGroupChatMessages((message: GroupChatMessageModel) => {
            message.groupChatMessageId = 1;
            setCurrentMessages(prevMessages => [...prevMessages, message]);
        });
    }, [chatHub?.groupChatMessagesHubConnection]);

    useEffect(() => {
        if (!groupChatData.messages) {
            return;
        }

        setCurrentMessages(groupChatData.messages);
    }, [groupChatData.messages]);

    useEffect(() => {
        if (!groupChatData.messages) {
            return;
        }

        const handleScroll = () => {
            if (!groupChatData.messages) {
                return;
            }

            const chatContainer: any = chatContainerRef.current;
            if (chatContainer.scrollTop === 0) {
                const moreMessagesCount = groupChatData.count - currentMessages.length + (groupChatData.messages === null ? 0 : groupChatData.messages.length) - pageSizeRef.current;

                setHaveMoreMessage(moreMessagesCount > 0);
            }
            else if (chatContainer.scrollHeight - chatContainer.scrollTop === chatContainer.clientHeight) {
                setHaveMoreMessage(false);
            }
        }

        const scrollContainer = chatContainerRef.current;
        scrollContainer?.addEventListener("scroll", handleScroll);

        return () => {
            scrollContainer?.removeEventListener("scroll", handleScroll);
        }
    }, [currentMessages, groupChatData.messages]);

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

    useEffect(() => {
        if (!groupChatData.groupChatUsers) {
            return;
        }

        const customersId: string[] = [];
        for (let i = 0; i < groupChatData.groupChatUsers.length; i++) {
            customersId.push(groupChatData.groupChatUsers[i].appUserId);
        }

        setGroupChatUsersId(customersId);
    }, [groupChatData.groupChatUsers]);

    const updateMessageAsync = async (message: PersonalChatMessage | GroupChatMessageModel) => {
        await updateGroupChatMessage(message);
    }

    const saveScrollState = () => {
        const chatContainer: any = chatContainerRef.current;
        const previousScrollHeight = chatContainer.scrollHeight;
        const previousScrollTop = chatContainer.scrollTop;

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

    const handleLoadMoreMessagesAsync = async () => {
        setAreLoadingOldMessages(true);

        const moreMessages: any = await getMoreMessagesAsync(currentMessages.length);

        setCurrentMessages(prevMessages => [...moreMessages, ...prevMessages]);

        saveScrollState();
    }

    if (!chatHub || groupChatData.isLoading) {
        return (
            <div className="chats__selected-chat_loading">
                <Loading />
            </div>
        );
    }

    return (
        <div className="chats__selected-chat">
            <div className="messages-container">
                <GroupChatTitle
                    chat={chat}
                    myself={myself}
                    settingsIsShow={settingsIsShow}
                    setSettingsIsShow={setSettingsIsShow}
                    haveMoreMessages={haveMoreMessages}
                    setHaveMoreMessage={setHaveMoreMessage}
                    loadMoreMessagesAsync={handleLoadMoreMessagesAsync}
                    t={t}
                />
                <ul className="chat-messages" ref={chatContainerRef}>
                    {currentMessages?.map((message) => (
                        <li className="message" key={message.id}>
                            <ChatMessage
                                myself={myself}
                                reviewerId={groupChatData.IasGroupChatUser.id}
                                chatUserAsUserId={groupChatData.groupChatUsers.filter(u => u.id === message.groupChatUserId)[0]?.appUserId}
                                chatUserUsername={groupChatData.groupChatUsers.filter(u => u.id === message.groupChatUserId)[0]?.username}
                                messageOwnerId={groupChatData.groupChatUsers.filter(u => u.id === message.groupChatUserId)[0]?.id}
                                message={message}
                                updateMessageAsync={updateMessageAsync}
                                hubConnection={chatHub.groupChatMessagesHubConnection}
                                subscribeToChatMessageHasBeenRead={chatHub.subscribeToGroupMessageHasBeenRead}
                            />
                        </li>
                    ))}
                </ul>
                <MessageInput
                    chatId={chat.id}
                    IasGroupChatUser={groupChatData.IasGroupChatUser}
                    setAreLoadingOldMessages={setAreLoadingOldMessages}
                    targetChatType={1}
                    t={t}
                    companionsId={groupChatUsersId}
                />
            </div>
            {settingsIsShow &&
                <GroupChatMenu
                    myself={myself}
                    setSelectedChat={setSelectedChat}
                    groupChatUsers={groupChatData.groupChatUsers}
                    groupChatUsersId={groupChatUsersId}
                    IasGroupChatUser={groupChatData.IasGroupChatUser}
                    chat={chat}
                    t={t}
                />
            }
        </div>
    );
}

export default memo(GroupChat);