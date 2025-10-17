import Store from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import { useChatHub } from '@/shared/hooks/useChatHub';
import logger from '@/utils/Logger';
import { memo, useEffect, useRef, useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { ChatApi } from '../../api/Chat.api';
import { usePartialUpdateGroupChatMessageMutation } from '../../api/GroupChatMessage.api';
import useGroupChatData from '../../hooks/useGroupChatData';
import type { GroupChatMessageModel } from '../../types/GroupChatMessageModel';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { GroupChatMessagePatch } from '../../types/patches/GroupChatMessagePatch';
import type { PersonalChatMessagePatch } from '../../types/patches/PersonalChatMessagePatch';
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

    let page = 1;

    const [settingsIsShow, setSettingsIsShow] = useState(false);
    const [groupChatUsersId, setGroupChatUsersId] = useState<string[]>([]);

    const [haveMoreMessages, setHaveMoreMessage] = useState(false);
    const [currentMessages, setCurrentMessages] = useState<GroupChatMessageModel[]>([]);
    const [messagesIsLoaded, setMessagesIsLoaded] = useState(false);
    const [areLoadingOldMessages, setAreLoadingOldMessages] = useState(true);

    const chatContainerRef = useRef<HTMLUListElement | null>(null);
    const pageSizeRef = useRef<number>(APP_CONFIG.communication.chatPageSize ? +APP_CONFIG.communication.chatPageSize : 5);

    const { messages, count, IasGroupChatUser, groupChatUsers } = useGroupChatData(chat.id, myself.id, page, pageSizeRef);

    const [partialUpdateGroupChatMessage] = usePartialUpdateGroupChatMessageMutation();

    useEffect(() => {
        setCurrentMessages([]);

        if (!chatHub) {
            return;
        }

        (async () => {
            await chatHub.connectToGroupChatMessagesAsync(chat.id);

            chatHub.subscribeToGroupChatMessages((message: GroupChatMessageModel) => {
                setCurrentMessages(prevMessages => [...prevMessages, message]);
            });

            chatHub.subscribeToGroupChatMessageEdit((messageId: number) => {
                Store.dispatch(
                    ChatApi.util.invalidateTags([{ type: "GroupChatMessage", id: messageId }])
                );
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

            if (!messages || !chatContainer || !count) {
                return;
            }

            if (chatContainer.scrollTop === 0) {
                const moreMessagesCount = count - currentMessages.length + (messages === null ? 0 : messages.length) - pageSizeRef.current;

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

    useEffect(() => {
        if (!groupChatUsers) {
            return;
        }

        const customersId: string[] = [];
        for (let i = 0; i < groupChatUsers.length; i++) {
            customersId.push(groupChatUsers[i].appUserId);
        }

        setGroupChatUsersId(customersId);
    }, [groupChatUsers]);

    const updateMessageAsync = async (message: PersonalChatMessagePatch | GroupChatMessagePatch) => {
        try {
            await partialUpdateGroupChatMessage({ id: message.id, message }).unwrap();

            if (chatHub && chatHub.groupChatMessagesHubConnectionRef.current) {
                await chatHub.groupChatMessagesHubConnectionRef.current.invoke("RequestEditedMessage", chat.id, message.id);
            }
        } catch (e) {
            logger.error("Failed to update group chat message", e);
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

        page++;

        saveScrollState();
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
                    {currentMessages.map((message) => (
                        <li className="message" key={message.id}>
                            {(IasGroupChatUser && groupChatUsers && chatHub) &&
                                <ChatMessage
                                    reviewerId={IasGroupChatUser.id ?? ""}
                                    chatUserAsUserId={groupChatUsers.filter(u => u.id === message.groupChatUserId)[0]?.appUserId}
                                    chatUserUsername={groupChatUsers.filter(u => u.id === message.groupChatUserId)[0]?.username}
                                    messageOwnerId={groupChatUsers.filter(u => u.id === message.groupChatUserId)[0]?.id ?? ""}
                                    message={message}
                                    updateMessageAsync={updateMessageAsync}
                                    hubConnection={chatHub.groupChatMessagesHubConnectionRef.current}
                                    subscribeToChatMessageHasBeenRead={chatHub.subscribeToGroupMessageHasBeenRead}
                                    lastReadMessageId={IasGroupChatUser.lastReadMessageId}
                                />
                            }
                        </li>
                    ))}
                </ul>
                {IasGroupChatUser &&
                    <MessageInput
                        chatId={chat.id}
                        initiator={IasGroupChatUser}
                        setAreLoadingOldMessages={setAreLoadingOldMessages}
                        targetChatType={1}
                        t={t}
                    />
                }
            </div>
            {(settingsIsShow && IasGroupChatUser) &&
                <GroupChatMenu
                    myself={myself}
                    setSelectedChat={setSelectedChat}
                    groupChatUsersId={groupChatUsersId}
                    IasGroupChatUser={IasGroupChatUser}
                    chat={chat}
                    t={t}
                />
            }
        </div>
    );
}

export default memo(GroupChat);