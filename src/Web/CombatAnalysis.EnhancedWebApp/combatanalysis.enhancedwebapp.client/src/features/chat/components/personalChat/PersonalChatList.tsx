import { useChatHub } from '@/shared/hooks/useChatHub';
import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState, type SetStateAction } from 'react';
import { useGetPersonalChatsByUserIdQuery } from '../../api/PersonalChat.api';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';
import PersonalChatListItem from './PersonalChatListItem';

interface PersonalChatListProps {
    myselfId: string;
    selectedChat: PersonalChatModel | GroupChatModel | null;
    setSelectedChat: (value: SetStateAction<PersonalChatModel | GroupChatModel | null>) => void;
    chatsHidden: boolean;
    toggleChatsHidden: () => void;
    t: (key: string) => string;
}

const PersonalChatList: React.FC<PersonalChatListProps> = ({ myselfId, t, selectedChat, setSelectedChat, chatsHidden, toggleChatsHidden }) => {
    const { data: personalChats, isLoading } = useGetPersonalChatsByUserIdQuery(myselfId);

    const chatHub = useChatHub();

    const [chats, setChats] = useState<PersonalChatModel[]>([]);

    useEffect(() => {
        if (!chatHub) {
            return;
        }

        chatHub.subscribeToPersonalChat((chat: PersonalChatModel) => {
            setChats(prevChats => [...prevChats, chat]);
        });
    }, []);

    useEffect(() => {
        if (!chatHub || !personalChats) {
            return;
        }

        setChats(personalChats);

        const connectToPersonalChatUnreadMessages = async () => {
            for (let i = 0; i < personalChats.length; i++) {
                await chatHub.connectToPersonalChatUnreadMessagesAsync(personalChats[i].id);
            }
        }

        connectToPersonalChatUnreadMessages();
    }, [personalChats]);

    if (!chatHub || isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="chat-list">
            <div className="chats__my-chats_title">
                <div>{t("PersonalChats")}</div>
                <FontAwesomeIcon
                    icon={chatsHidden ? faArrowDown : faArrowUp}
                    title={chatsHidden ? t("ShowChats") : t("HideChats")}
                    onClick={toggleChatsHidden}
                />
            </div>
            <ul className={`chat-list__chats${!chatsHidden ? "_active" : ""}`}>
                {chats.length === 0
                    ? <div className="personal-chats not-found">
                        {t("PersonalChatsEmptyYet")}
                    </div>
                    : chats.map((chat) => (
                        <li key={chat.id} className={selectedChat?.id === chat.id ? `selected` : ``}>
                            <PersonalChatListItem
                                chat={chat}
                                setSelectedChat={setSelectedChat}
                                companionId={chat.initiatorId === myselfId ? chat.companionId : chat.initiatorId}
                                meId={myselfId}
                                subscribeToUnreadPersonalMessagesUpdated={chatHub.subscribeToUnreadPersonalMessagesUpdated}
                            />
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default PersonalChatList;