import { useChatHub } from '@/shared/hooks/useChatHub';
import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState, type SetStateAction } from 'react';
import { useFindGroupChatUsersByUserIdQuery } from '../../api/GroupChatUser.api';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { GroupChatUserModel } from '../../types/GroupChatUserModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';
import GroupChatListItem from './GroupChatListItem';

interface GroupChatListProps {
    meId: string;
    selectedChat: GroupChatModel | PersonalChatModel | null;
    setSelectedChat(value: SetStateAction<GroupChatModel | PersonalChatModel | null>): void;
    chatsHidden: boolean;
    toggleChatsHidden(): void;
    t(key: string): string;
    setShowCreateGroupChat(value: SetStateAction<boolean>): void;
}

const GroupChatList: React.FC<GroupChatListProps> = ({ meId, t, selectedChat, setSelectedChat, chatsHidden, toggleChatsHidden, setShowCreateGroupChat }) => {
    const { data, isLoading } = useFindGroupChatUsersByUserIdQuery(meId);

    const chatHub = useChatHub();

    const [meInGroupChats, setMeInGroupChats] = useState<GroupChatUserModel[]>([]);

    useEffect(() => {
        if (!data || !chatHub) {
            return;
        }

        setMeInGroupChats(data);

        const connectToPersonalChatUnreadMessages = async () => {
            await chatHub.connectToGroupChatUnreadMessagesAsync(data);

            chatHub.subscribeToGroupChat((groupChatUser: GroupChatUserModel) => {
                setMeInGroupChats(prev => [...prev, groupChatUser]);
            });
        }

        connectToPersonalChatUnreadMessages();
    }, [data]);

    if (isLoading || !chatHub || !chatHub.groupChatUnreadMessagesHubConnection  || !chatHub) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="chat-list">
            <div className="chats__my-chats_title">
                <div>{t("GroupChats")}</div>
                <div className="not-found">
                    <span onClick={() => setShowCreateGroupChat(true)}>{t("Create")}</span>
                </div>
                <FontAwesomeIcon
                    icon={chatsHidden ? faArrowDown : faArrowUp}
                    title={chatsHidden ? t("ShowChats") : t("HideChats")}
                    onClick={toggleChatsHidden}
                />
            </div>
            <ul className={`chat-list__chats${!chatsHidden ? "_active" : ""}`}>
                {meInGroupChats.length === 0
                    ? <div className="group-chats not-found">
                        <div>{t("GroupChatsEmptyYet")}</div>
                        <span onClick={() => setShowCreateGroupChat(true)}>{t("Create")}</span>
                    </div>
                    : meInGroupChats.map((meInChat) => (
                        <li key={meInChat.id} className={selectedChat && "appUserId" in selectedChat && selectedChat.id === meInChat.chatId ? `selected` : ``}>
                            <GroupChatListItem
                                meInChat={meInChat}
                                setSelectedGroupChat={setSelectedChat}
                                subscribeToUnreadGroupMessagesUpdated={chatHub.subscribeToUnreadGroupMessagesUpdated}
                            />
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default GroupChatList;