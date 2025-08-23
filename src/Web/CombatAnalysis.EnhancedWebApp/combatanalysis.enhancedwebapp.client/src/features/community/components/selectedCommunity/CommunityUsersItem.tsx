import { useState, type ChangeEvent, type JSX, type SetStateAction } from 'react';
import User from '../../../user/components/User';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import type { CommunityUserModel } from '../../types/CommunityUserModel';

interface CommunityUsersItemProps {
    me: AppUserModel;
    communityUser: CommunityUserModel;
    usersToRemove: CommunityUserModel[];
    setUsersToRemove(value: SetStateAction<CommunityUserModel[]>): void;
    showRemoveUser: boolean;
}

const CommunityUsersItem: React.FC<CommunityUsersItemProps> = ({ me, communityUser, usersToRemove, setUsersToRemove, showRemoveUser }) => {
    const [userInformation, setUserInformation] = useState<JSX.Element | null>(null);

    const addUserToUsersToRemove = (communityUser: CommunityUserModel) => {
        const users = usersToRemove;
        users.push(communityUser);

        setUsersToRemove(users);
    }

    const excludeUserFromUsersToRemove = (communityUser: CommunityUserModel) => {
        const people = usersToRemove.filter(user => user.id !== communityUser.id);

        setUsersToRemove(people);
    }

    const removeUserHandle = (e: ChangeEvent<HTMLInputElement>, communityUser: CommunityUserModel) => {
        const checked = e.target.checked;

        if (checked) {
            addUserToUsersToRemove(communityUser);
        }
        else {
            excludeUserFromUsersToRemove(communityUser);
        }
    }

    return (
        <>
            <div className="user-target-community__information">
                <User
                    myself={me}
                    targetUserId={communityUser.appUserId}
                    targetUsername={communityUser.appUserId}
                    setUserInformation={setUserInformation}
                />
                {(me.id !== communityUser.appUserId && showRemoveUser) &&
                    <input className="form-check-input" type="checkbox"
                        onChange={(e) => removeUserHandle(e, communityUser)} />
                }
            </div>
            {userInformation !== null &&
                <div className="community-user-information">{userInformation}</div>
            }
        </>
    );
}

export default CommunityUsersItem;