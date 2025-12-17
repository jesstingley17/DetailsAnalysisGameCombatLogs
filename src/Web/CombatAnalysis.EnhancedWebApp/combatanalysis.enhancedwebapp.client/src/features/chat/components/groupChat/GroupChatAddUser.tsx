import AddPeople from '@/shared/components/AddPeople';
import type { ChatHubContextModel } from '@/shared/types/ChatHubModel';
import logger from '@/utils/Logger';
import { useState, type SetStateAction } from 'react';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import type { GroupChatUserModel } from '../../types/GroupChatUserModel';
import type { GroupChatModel } from '../../types/GroupChatModel';

interface GroupChatAddUserProps {
    chat: GroupChatModel;
    groupChatUsersId: string[];
    setShowAddPeople: (value: SetStateAction<boolean>) => void;
    chatHub: ChatHubContextModel | null;
    t: (key: string) => string;
}

const GroupChatAddUser: React.FC<GroupChatAddUserProps> = ({ chat, groupChatUsersId, setShowAddPeople, chatHub, t }) => {
    const [peopleToJoin, setPeopleToJoin] = useState<AppUserModel[]>([]);

    const createGroupChatUserAsync = async () => {
        try {
            if (!chatHub || !chatHub.groupChatHubConnectionRef.current) {
                return;
            }

            for (let i = 0; i < peopleToJoin.length; i++) {
                const newGroupChatUser: GroupChatUserModel = {
                    username: peopleToJoin[i].username,
                    unreadMessages: 0,
                    groupChatId: chat.id,
                    appUserId: peopleToJoin[i].id,
                };

                await chatHub.groupChatHubConnectionRef.current.invoke("AddUserToChat", chat.ownerId, newGroupChatUser);
            }

            setPeopleToJoin([]);
            setShowAddPeople(false);
        } catch (e) {
            logger.error("Failed to remove group chat users", e);
        }
    }

    return (
        <div className="add-people-to-chat box-shadow">
            <AddPeople
                usersId={groupChatUsersId}
                peopleToJoin={peopleToJoin}
                setPeopleToJoin={setPeopleToJoin}
            />
            <div className="item-result">
                <div className="btn-border-shadow invite" onClick={createGroupChatUserAsync}>{t("Invite")}</div>
                <div className="btn-border-shadow" onClick={() => setShowAddPeople(false)}>{t("Close")}</div>
            </div>
        </div>
    );
}

export default GroupChatAddUser;