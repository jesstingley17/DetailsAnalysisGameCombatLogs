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
    myselfId: string;
    selectedChat: GroupChatModel | PersonalChatModel | null;
    setSelectedChat: (value: SetStateAction<GroupChatModel | PersonalChatModel | null>) => void;
    chatsHidden: boolean;
    toggleChatsHidden: () => void;
    t: (key: string) => string;
    setShowCreateGroupChat: (value: SetStateAction<boolean>) => void;
}

const GroupChatList: React.FC<GroupChatListProps> = ({ myselfId, t, selectedChat, setSelectedChat, chatsHidden, toggleChatsHidden, setShowCreateGroupChat }) => {
    const { data: myselfInGroupChats, isLoading } = useFindGroupChatUsersByUserIdQuery(myselfId);

    const chatHub = useChatHub();

    const [extendedMyselfInGroupChats, setExtendedMyselfInGroupChats] = useState<GroupChatUserModel[]>([]);

    useEffect(() => {
        if (!chatHub) {
            return;
        }

        chatHub.subscribeToGroupChat((myselfInGroupChat: GroupChatUserModel) => {
            setExtendedMyselfInGroupChats(prev => [...prev, myselfInGroupChat]);
        });
    }, []);

    useEffect(() => {
        if (!chatHub || !myselfInGroupChats) {
            return;
        }

        setExtendedMyselfInGroupChats(myselfInGroupChats);

        const connectToPersonalChatUnreadMessages = async () => {
            for (let i = 0; i < myselfInGroupChats.length; i++) {
                await chatHub.connectToGroupChatUnreadMessagesAsync(myselfInGroupChats[i].chatId);
            }
        }

        connectToPersonalChatUnreadMessages();
    }, [myselfInGroupChats]);

    if (isLoading || !chatHub) {
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
                {extendedMyselfInGroupChats.length === 0
                    ? <div className="group-chats not-found">
                        <div>{t("GroupChatsEmptyYet")}</div>
                        <span onClick={() => setShowCreateGroupChat(true)}>{t("Create")}</span>
                    </div>
                    : extendedMyselfInGroupChats.map((myselfInChat) => (
                        <li key={myselfInChat.id} className={selectedChat?.id === myselfInChat.chatId ? `selected` : ``}>
                            <GroupChatListItem
                                meInChat={myselfInChat}
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