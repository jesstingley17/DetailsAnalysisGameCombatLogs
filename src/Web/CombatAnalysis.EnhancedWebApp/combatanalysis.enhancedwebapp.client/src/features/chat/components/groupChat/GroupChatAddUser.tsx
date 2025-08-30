import AddPeople from '@/shared/components/AddPeople';
import type { ChatHubContextModel } from '@/shared/types/ChatHubModel';
import logger from '@/utils/Logger';
import { useState, type SetStateAction } from 'react';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import type { GroupChatUserModel } from '../../types/GroupChatUserModel';

interface GroupChatAddUserProps {
    myself: AppUserModel;
    chatId: number;
    groupChatUsersId: string[];
    setShowAddPeople: (value: SetStateAction<boolean>) => void;
    chatHub: ChatHubContextModel | null;
    t: (key: string) => string;
}

const GroupChatAddUser: React.FC<GroupChatAddUserProps> = ({ myself, chatId, groupChatUsersId, setShowAddPeople, chatHub, t }) => {
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
                    chatId: chatId,
                    appUserId: peopleToJoin[i].id,
                };

                await chatHub.groupChatHubConnectionRef.current.invoke("AddUserToChat", newGroupChatUser);
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
                user={myself}
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