import { useState, type ChangeEvent, type JSX, type SetStateAction } from 'react';
import type { CommunityUserModel } from '../../../community/types/CommunityUserModel';
import User from '../../../user/components/User';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import type { GroupChatUserModel } from '../../types/GroupChatUserModel';

interface GroupChatMembersItemProps {
    myself: AppUserModel;
    user: GroupChatUserModel | CommunityUserModel;
    usersToRemove: (GroupChatUserModel | CommunityUserModel)[];
    setUsersToRemove: (value: SetStateAction<(GroupChatUserModel | CommunityUserModel)[]>) => void;
    showRemoveUser: boolean;
}

const GroupChatMembersItem: React.FC<GroupChatMembersItemProps> = ({ myself, user, usersToRemove, setUsersToRemove, showRemoveUser }) => {
    const [userInformation, setUserInformation] = useState<JSX.Element | null>(null);

    const addUserToUsersToRemove = (user: GroupChatUserModel | CommunityUserModel) => {
        const users = usersToRemove;
        users.push(user);

        setUsersToRemove(users);
    }

    const excludeUserFromUsersToRemove = (user: GroupChatUserModel | CommunityUserModel) => {
        const otherUsersToRemove = usersToRemove.filter(userToRemove => userToRemove.id !== user.id);

        setUsersToRemove(otherUsersToRemove);
    }

    const handleRemoveUser = (e: ChangeEvent<HTMLInputElement> | undefined, user: GroupChatUserModel | CommunityUserModel) => {
        const checked = e?.target.checked ?? false;

        if (checked) {
            addUserToUsersToRemove(user);
        }
        else {
            excludeUserFromUsersToRemove(user);
        }
    }

    return (
        <>
            <div className="user-target-community__information">
                <User
                    myself={myself}
                    targetUserId={user.appUserId}
                    targetUsername={user.username}
                    setUserInformation={setUserInformation}
                />
                {(myself.id !== user.appUserId && showRemoveUser) &&
                    <input className="form-check-input" type="checkbox" onChange={(e) => handleRemoveUser(e, user)} />
                }
            </div>
            {userInformation !== null &&
                <div className="community-user-information">{userInformation}</div>
            }
        </>
    );
}

export default GroupChatMembersItem;