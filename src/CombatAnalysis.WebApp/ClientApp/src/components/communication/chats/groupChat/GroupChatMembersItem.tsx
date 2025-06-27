import { useState } from "react";
import { GroupChatUser } from "../../../../types/GroupChatUser";
import { GroupChatMembersItemProps } from "../../../../types/components/communication/chats/GroupChatMembersItemProps";
import User from "../../User";

const GroupChatMembersItem: React.FC<GroupChatMembersItemProps> = ({ myself, groupChatUser, usersToRemove, setUsersToRemove, showRemoveUser }) => {
    const [userInformation, setUserInformation] = useState(null);

    const addUserToUsersToRemove = (groupChatUser: GroupChatUser) => {
        const users = usersToRemove;
        users.push(groupChatUser);

        setUsersToRemove(users);
    }

    const excludeUserFromUsersToRemove = (groupChatUser: GroupChatUser) => {
        const people = usersToRemove.filter(user => user.id !== groupChatUser.id);

        setUsersToRemove(people);
    }

    const handleRemoveUser = (e: any, groupChatUser: GroupChatUser) => {
        const checked = e.target.checked;

        checked ? addUserToUsersToRemove(groupChatUser) : excludeUserFromUsersToRemove(groupChatUser);
    }

    return (
        <>
            <div className="user-target-community__information">
                <User
                    myself={myself}
                    targetUserId={groupChatUser.appUserId}
                    targetUsername={groupChatUser.appUserId}
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