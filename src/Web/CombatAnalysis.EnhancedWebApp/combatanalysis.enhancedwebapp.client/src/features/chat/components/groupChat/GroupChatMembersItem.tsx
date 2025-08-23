import { useState, type ChangeEvent, type SetStateAction } from 'react';
import User from '../../../user/components/User';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import type { GroupChatUserModel } from '../../types/GroupChatUserModel';

interface GroupChatMembersItemProps {
    myself: AppUserModel;
    groupChatUser: GroupChatUserModel;
    usersToRemove: GroupChatUserModel[];
    setUsersToRemove(value: SetStateAction<GroupChatUserModel[]>): void;
    showRemoveUser: boolean;
}

const GroupChatMembersItem: React.FC<GroupChatMembersItemProps> = ({ myself, groupChatUser, usersToRemove, setUsersToRemove, showRemoveUser }) => {
    const [userInformation, setUserInformation] = useState(null);

    const addUserToUsersToRemove = (GroupChatUserModel: GroupChatUserModel) => {
        const users = usersToRemove;
        users.push(GroupChatUserModel);

        setUsersToRemove(users);
    }

    const excludeUserFromUsersToRemove = (GroupChatUserModel: GroupChatUserModel) => {
        const people = usersToRemove.filter(user => user.id !== GroupChatUserModel.id);

        setUsersToRemove(people);
    }

    const handleRemoveUser = (e: ChangeEvent<HTMLInputElement> | undefined, GroupChatUserModel: GroupChatUserModel) => {
        const checked = e?.target.checked ?? false;

        if (checked) {
            addUserToUsersToRemove(GroupChatUserModel);
        }
        else {
            excludeUserFromUsersToRemove(GroupChatUserModel);
        }
    }

    return (
        <>
            <div className="user-target-community__information">
                <User
                    myself={myself}
                    targetUserId={groupChatUser.appUserId}
                    targetUsername={groupChatUser.username}
                    setUserInformation={setUserInformation}
                />
                {(myself.id !== groupChatUser.appUserId && showRemoveUser) &&
                    <input className="form-check-input" type="checkbox" onChange={(e) => handleRemoveUser(e, groupChatUser)} />
                }
            </div>
            {userInformation !== null &&
                <div className="community-user-information">{userInformation}</div>
            }
        </>
    );
}

export default GroupChatMembersItem;