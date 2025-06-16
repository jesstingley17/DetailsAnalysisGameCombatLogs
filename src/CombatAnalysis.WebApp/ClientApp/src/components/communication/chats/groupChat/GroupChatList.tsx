import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import { useChatHub } from '../../../../context/ChatHubProvider';
import { useFindGroupChatUserByUserIdQuery } from '../../../../store/api/chat/GroupChatUser.api';
import { GroupChatUser } from '../../../../types/GroupChatUser';
import { GroupChatListProps } from '../../../../types/components/communication/chats/GroupChatListProps';
import GroupChatListItem from './GroupChatListItem';

const GroupChatList: React.FC<GroupChatListProps> = ({ meId, t, selectedChat, setSelectedChat, chatsHidden, toggleChatsHidden, setShowCreateGroupChat }) => {
    const { data, isLoading } = useFindGroupChatUserByUserIdQuery(meId);

    const chatHub = useChatHub();

    const [meInGroupChats, setMeInGroupChats] = useState<GroupChatUser[]>([]);

    useEffect(() => {
        if (!data || !chatHub) {
            return;
        }

        setMeInGroupChats(data);

        const connectToPersonalChatUnreadMessages = async () => {
            await chatHub.connectToGroupChatUnreadMessagesAsync(data);

            chatHub.subscribeToGroupChat((groupChatUser: GroupChatUser) => {
                setMeInGroupChats(prev => [...prev, groupChatUser]);
            });
        }

        connectToPersonalChatUnreadMessages();
    }, [data]);

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
                {meInGroupChats.length === 0
                    ? <div className="group-chats not-found">
                        <div>{t("GroupChatsEmptyYet")}</div>
                        <span onClick={() => setShowCreateGroupChat(true)}>{t("Create")}</span>
                    </div>
                    : meInGroupChats.map((meInChat) => (
                        <li key={meInChat.id} className={selectedChat.type === "group" && selectedChat.chat.id === meInChat.chatId ? `selected` : ``}>
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