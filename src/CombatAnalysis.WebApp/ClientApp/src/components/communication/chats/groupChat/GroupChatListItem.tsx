import { useEffect, useState } from 'react';
import { useGetGroupChatByIdQuery } from '../../../../store/api/chat/GroupChat.api';
import { GroupChatListItemProps } from '../../../../types/components/communication/chats/GroupChatListItemProps';

const GroupChatListItem: React.FC<GroupChatListItemProps> = ({ meInChat, setSelectedGroupChat, subscribeToUnreadGroupMessagesUpdated }) => {
    const [unreadMessageCount, setUnreadMessageCount] = useState(-1);

    const { data: chat, isLoading } = useGetGroupChatByIdQuery(meInChat.chatId);

    useEffect(() => {
        subscribeToUnreadGroupMessagesUpdated(meInChat.id, (targetChatId: number, targetMeInChatId: string, count: number) => {
            if (targetChatId === meInChat.chatId && targetMeInChatId === meInChat.id) {
                setUnreadMessageCount(count);
            }
        });
    }, []);

    useEffect(() => {
        if (!meInChat) {
            return;
        }

        setUnreadMessageCount(meInChat.unreadMessages);
    }, [meInChat]);

    if (isLoading || !chat) {
        return (<div className="chat-loading-yet">Loading...</div>);
    }

    return (
        <span className="chat-card" onClick={() => setSelectedGroupChat({ type: "group", chat: chat })}>
            <div className="username">{chat?.name}</div>
            {unreadMessageCount > 0 &&
                <div className="chat-tooltip">
                    <div className="unread-message-count">{unreadMessageCount > 99 ? "99+" : unreadMessageCount}</div>
                </div>
            }
        </span>
    );
}

export default GroupChatListItem;