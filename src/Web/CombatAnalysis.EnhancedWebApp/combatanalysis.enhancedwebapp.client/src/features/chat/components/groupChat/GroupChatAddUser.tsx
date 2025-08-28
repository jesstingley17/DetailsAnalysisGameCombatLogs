import AddPeople from '@/shared/components/AddPeople';
import { useChatHub } from '@/shared/hooks/useChatHub';
import { useState, type SetStateAction } from 'react';
import type { AppUserModel } from '../../../user/types/AppUserModel';

interface GroupChatAddUserProps {
    myself: AppUserModel;
    chatId: number;
    groupChatUsersId: string[];
    setShowAddPeople: (value: SetStateAction<boolean>) => void;
    t: (key: string) => string;
}

const GroupChatAddUser: React.FC<GroupChatAddUserProps> = ({ myself, chatId, groupChatUsersId, setShowAddPeople, t }) => {
    const chatHub = useChatHub();

    const [peopleToJoin, setPeopleToJoin] = useState<AppUserModel[]>([]);

    const createGroupChatUserAsync = async () => {
        if (!chatHub || !chatHub.groupChatHubConnectionRef.current) {
            return;
        }

        for (let i = 0; i < peopleToJoin.length; i++) {
            const newGroupChatUser = {
                id: " ",
                username: peopleToJoin[i].username,
                appUserId: peopleToJoin[i].id,
                chatId: chatId,
            };

            await chatHub.groupChatHubConnectionRef.current.invoke("AddUserToChat", newGroupChatUser);
        }

        setPeopleToJoin([]);
        setShowAddPeople(false);
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