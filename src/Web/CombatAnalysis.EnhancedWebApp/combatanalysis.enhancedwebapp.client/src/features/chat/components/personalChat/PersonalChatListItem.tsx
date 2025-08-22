import { useEffect, useState, type SetStateAction } from 'react';
import { useGetUserByIdQuery } from '../../../user/api/Account.api';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';

interface PersonalChatListItemProps {
    chat: PersonalChatModel;
    setSelectedChat(value: SetStateAction<PersonalChatModel | GroupChatModel | null>): void;
    companionId: string;
    meId: string;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    subscribeToUnreadPersonalMessagesUpdated(callback: any): void;
}

const PersonalChatListItem: React.FC<PersonalChatListItemProps> = ({ chat, setSelectedChat, companionId, meId, subscribeToUnreadPersonalMessagesUpdated }) => {
    const [unreadMessageCount, setUnreadMessageCount] = useState(-1);

    const { data: companion, isLoading } = useGetUserByIdQuery(companionId);

    useEffect(() => {
        subscribeToUnreadPersonalMessagesUpdated((targetChatId: number, targetMeInChatId: string, count: number) => {
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
        <span className="chat-card" onClick={() => setSelectedChat(chat)}>
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