import { type JSX, useState } from 'react';
import type { AppUserModel } from '../../types/AppUserModel';
import User from '../User';

const PeopleItem: React.FC<{ targetUser: AppUserModel }> = ({ targetUser }) => {
    const [userInformation, setUserInformation] = useState<JSX.Element | null>(null);

    return (
        <>
            <div className="card box-shadow">
                <User
                    targetUserId={targetUser.id}
                    targetUsername={targetUser.username}
                    setUserInformation={setUserInformation}
                />
            </div>
            <div className="people-user-information">
                {userInformation}
            </div>
        </>
    );
}

export default PeopleItem;