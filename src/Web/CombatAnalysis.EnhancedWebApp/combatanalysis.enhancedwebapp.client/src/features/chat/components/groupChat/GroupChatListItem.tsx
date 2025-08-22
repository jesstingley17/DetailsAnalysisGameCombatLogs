import { useEffect, useState, type SetStateAction } from 'react';
import { useGetGroupChatByIdQuery } from '../../api/GroupChat.api';
import type { GroupChatUserModel } from '../../types/GroupChatUserModel';
import type { SelectedChatModel } from '../../types/SelectedChatModel';

interface GroupChatListItemProps {
    meInChat: GroupChatUserModel;
    setSelectedGroupChat(value: SetStateAction<SelectedChatModel>): void;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    subscribeToUnreadGroupMessagesUpdated(callback: any): void;
}

const GroupChatListItem: React.FC<GroupChatListItemProps> = ({ meInChat, setSelectedGroupChat, subscribeToUnreadGroupMessagesUpdated }) => {
    const [unreadMessageCount, setUnreadMessageCount] = useState(-1);

    const { data: chat, isLoading } = useGetGroupChatByIdQuery(meInChat.chatId);

    useEffect(() => {
        subscribeToUnreadGroupMessagesUpdated((targetChatId: number, targetChatUserId: string, count: number) => {
            if (targetChatId === meInChat.chatId && targetChatUserId === meInChat.id) {
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