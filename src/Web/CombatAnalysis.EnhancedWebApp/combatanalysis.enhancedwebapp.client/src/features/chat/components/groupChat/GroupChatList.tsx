import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState, type SetStateAction } from 'react';
import { useChatHub } from '../../../../shared/hooks/useChatHub';
import { useFindGroupChatUsersByUserIdQuery } from '../../api/GroupChatUser.api';
import type { GroupChatUserModel } from '../../types/GroupChatUserModel';
import type { SelectedChatModel } from '../../types/SelectedChatModel';
import GroupChatListItem from './GroupChatListItem';

interface GroupChatListProps {
    meId: string;
    selectedChat: SelectedChatModel;
    setSelectedChat(value: SetStateAction<SelectedChatModel>): void;
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
                        <li key={meInChat.id} className={selectedChat.type === "group" && selectedChat && selectedChat.chat?.id === meInChat.chatId ? `selected` : ``}>
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