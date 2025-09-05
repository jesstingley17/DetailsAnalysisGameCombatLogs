import { type JSX, useState } from 'react';
import type { AppUserModel } from '../../types/AppUserModel';
import User from '../User';

interface PeopleItemProps {
    myself: AppUserModel | null;
    targetUser: AppUserModel;
}

const PeopleItem: React.FC<PeopleItemProps> = ({ myself, targetUser }) => {
    const [userInformation, setUserInformation] = useState<JSX.Element | null>(null);

    return (
        <>
            <div className="card box-shadow">
                <User
                    myself={myself}
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