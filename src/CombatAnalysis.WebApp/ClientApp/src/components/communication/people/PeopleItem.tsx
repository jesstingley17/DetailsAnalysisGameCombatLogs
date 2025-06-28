import { useState } from 'react';
import { PeopleItemProps } from '../../../types/components/communication/people/PeopleItemProps';
import User from '../User';

const PeopleItem: React.FC<PeopleItemProps> = ({ me, targetUser }) => {
    const [userInformation, setUserInformation] = useState(null);

    return (
        <>
            <div className="card box-shadow">
                <User
                    myself={me}
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