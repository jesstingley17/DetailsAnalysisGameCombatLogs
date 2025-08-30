import APP_CONFIG from '@/config/appConfig';
import Loading from '@/shared/components/Loading';
import { useChatHub } from '@/shared/hooks/useChatHub';
import { memo, useEffect, useRef, useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useUpdateGroupChatMessageMutation } from '../../api/GroupChatMessage.api';
import useGroupChatData from '../../hooks/useGroupChatData';
import type { GroupChatMessageModel } from '../../types/GroupChatMessageModel';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { PersonalChatMessageModel } from '../../types/PersonalChatMessageModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';
import ChatMessage from '../ChatMessage';
import MessageInput from '../MessageInput';
import GroupChatMenu from './GroupChatMenu';
import GroupChatTitle from './GroupChatTitle';

import './GroupChat.scss';

interface GroupChatProps {
    myself: AppUserModel;
    chat: GroupChatModel;
    setSelectedChat: (value: SetStateAction<PersonalChatModel | GroupChatModel | null>) => void;
}

const GroupChat: React.FC<GroupChatProps> = ({ myself, chat, setSelectedChat }) => {
    const { t } = useTranslation('communication/chats/groupChat');

    const chatHub = useChatHub();

    const [settingsIsShow, setSettingsIsShow] = useState(false);
    const [groupChatUsersId, setGroupChatUsersId] = useState<string[]>([]);

    const [haveMoreMessages, setHaveMoreMessage] = useState(false);
    const [currentMessages, setCurrentMessages] = useState<GroupChatMessageModel[]>([]);
    const [messagesIsLoaded, setMessagesIsLoaded] = useState(false);
    const [areLoadingOldMessages, setAreLoadingOldMessages] = useState(true);

    const chatContainerRef = useRef<HTMLUListElement | null>(null);
    const pageSizeRef = useRef<number>(APP_CONFIG.communication.chatPageSize ? +APP_CONFIG.communication.chatPageSize : 5);

    const { groupChatData, getMoreMessagesAsync } = useGroupChatData(chat.id, myself.id, pageSizeRef);

    const [updateGroupChatMessage] = useUpdateGroupChatMessageMutation();

    useEffect(() => {
        setCurrentMessages([]);

        if (!chatHub) {
            return;
        }

        (async () => {
            await chatHub.connectToGroupChatAsync();
            await chatHub.connectToGroupChatMessagesAsync(chat.id);

            chatHub.subscribeToGroupChatMessages((message: GroupChatMessageModel) => {
                message.groupChatMessageId = 1;
                setCurrentMessages(prevMessages => [...prevMessages, message]);
            });
        })();

        return () => {
            (async () => {
                await chatHub.disconnectFromGroupChatHubAsync();
            })();
        }
    }, [chat]);

    useEffect(() => {
        if (!groupChatData || !groupChatData.messages) {
            return;
        }

        setCurrentMessages(groupChatData.messages);
    }, [groupChatData?.messages]);

    useEffect(() => {
        if (!groupChatData || !groupChatData.messages) {
            return;
        }

        const handleScroll = () => {
            const chatContainer: HTMLUListElement | null = chatContainerRef.current;

            if (!groupChatData.messages || !chatContainer) {
                return;
            }

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
    }, [currentMessages, groupChatData?.messages]);

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
        if (!groupChatData || !groupChatData.groupChatUsers) {
            return;
        }

        const customersId: string[] = [];
        for (let i = 0; i < groupChatData.groupChatUsers.length; i++) {
            customersId.push(groupChatData.groupChatUsers[i].appUserId);
        }

        setGroupChatUsersId(customersId);
    }, [groupChatData?.groupChatUsers]);

    const updateMessageAsync = async (message: PersonalChatMessageModel | GroupChatMessageModel) => {
        if ("groupChatUserId" in message) {
            await updateGroupChatMessage(message);
        }
    }

    const saveScrollState = () => {
        const chatContainer: HTMLUListElement | null = chatContainerRef.current;
        if (!chatContainer) {
            return;
        }

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

        const moreMessages = await getMoreMessagesAsync(currentMessages.length);

        setCurrentMessages(prevMessages => [...moreMessages, ...prevMessages]);

        saveScrollState();
    }

    if (!chatHub || !groupChatData || groupChatData.isLoading) {
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
                                user={myself}
                                reviewerId={groupChatData.IasGroupChatUser.id ?? ""}
                                chatUserAsUserId={groupChatData.groupChatUsers.filter(u => u.id === message.groupChatUserId)[0]?.appUserId}
                                chatUserUsername={groupChatData.groupChatUsers.filter(u => u.id === message.groupChatUserId)[0]?.username}
                                messageOwnerId={groupChatData.groupChatUsers.filter(u => u.id === message.groupChatUserId)[0]?.id ?? ""}
                                message={message}
                                updateMessageAsync={updateMessageAsync}
                                hubConnection={chatHub.groupChatMessagesHubConnectionRef.current}
                                subscribeToChatMessageHasBeenRead={chatHub.subscribeToGroupMessageHasBeenRead}
                            />
                        </li>
                    ))}
                </ul>
                <MessageInput
                    chatId={chat.id}
                    initiator={groupChatData.IasGroupChatUser}
                    setAreLoadingOldMessages={setAreLoadingOldMessages}
                    t={t}
                    targetChatType={1}
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