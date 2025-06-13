import { useEffect, useState } from 'react';
import { useGetUserByIdQuery } from '../../../../store/api/user/Account.api';
import { PersonalChatListItemProps } from '../../../../types/components/communication/chats/PersonalChatListItemProps';

const PersonalChatListItem: React.FC<PersonalChatListItemProps> = ({ chat, setSelectedChat, companionId, meId, subscribeToUnreadPersonalMessagesUpdated }) => {
    const [unreadMessageCount, setUnreadMessageCount] = useState(-1);

    const { data: companion, isLoading } = useGetUserByIdQuery(companionId);

    useEffect(() => {
        subscribeToUnreadPersonalMessagesUpdated(meId, (targetChatId: number, targetMeInChatId: string, count: number) => {
            if (targetChatId === chat?.id && targetMeInChatId === meId) {
                setUnreadMessageCount(count);
            }
        });
    }, []);

    useEffect(() => {
        if (!chat) {
            return;
        }

        setUnreadMessageCount(chat.initiatorId === meId ? chat.initiatorUnreadMessages : chat.companionUnreadMessages);
    }, [chat]);

    if (isLoading) {
        return (<div className="chat-loading-yet">Loading...</div>);
    }

    return (
        <span className="chat-card" onClick={() => setSelectedChat({ type: "personal", chat: chat })}>
            <div className="username">{companion?.username}</div>
            {unreadMessageCount > 0 &&
                <div className="chat-tooltip">
                    <div className="unread-message-count">{unreadMessageCount > 99 ? "99+" : unreadMessageCount}</div>
                </div>
            }
        </span>
    );
}

export default PersonalChatListItem;