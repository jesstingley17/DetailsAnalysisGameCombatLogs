import type { ChatHubContextModel } from '@/shared/types/ChatHubModel';
import { updateGroupChatUser } from '@/features/chat/store/GroupChatUserSlice';
import { useEffect, useState, type SetStateAction } from 'react';
import { useDispatch } from 'react-redux';
import { useGetGroupChatByIdQuery } from '../../api/GroupChat.api';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { GroupChatUserModel } from '../../types/GroupChatUserModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';

interface GroupChatListItemProps {
    myselfInChat: GroupChatUserModel;
    setSelectedGroupChat: (value: SetStateAction<GroupChatModel | PersonalChatModel | null>) => void;
    chatHub: ChatHubContextModel;
}

const GroupChatListItem: React.FC<GroupChatListItemProps> = ({ myselfInChat, setSelectedGroupChat, chatHub }) => {
    const dispatch = useDispatch();
    
    const [unreadMessageCount, setUnreadMessageCount] = useState(-1);

    const { data: chat, isLoading } = useGetGroupChatByIdQuery(myselfInChat.groupChatId);

    useEffect(() => {
        if (!chatHub.groupChatUnreadMessagesHubConnectionRef.current) {
            return;
        }

        chatHub.subscribeToUnreadGroupMessagesUpdated((targetChatId: number, targetChatUserId: string, count: number) => {
            if (targetChatId === myselfInChat.groupChatId && targetChatUserId === myselfInChat.id) {
                setUnreadMessageCount(count);
            }
        });
    }, [chatHub.groupChatUnreadMessagesHubConnectionRef.current]);

    useEffect(() => {
        if (!myselfInChat) {
            return;
        }

        setUnreadMessageCount(myselfInChat.unreadMessages);
    }, [myselfInChat]);

    const selectChatHandle = () => {
        setSelectedGroupChat(null);

        if (chat) {
            setSelectedGroupChat(chat);
            dispatch(updateGroupChatUser(myselfInChat));
        }
    }

    if (isLoading || !chat) {
        return (<div className="chat-loading-yet">Loading...</div>);
    }

    return (
        <span className="chat-card" onClick={selectChatHandle}>
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