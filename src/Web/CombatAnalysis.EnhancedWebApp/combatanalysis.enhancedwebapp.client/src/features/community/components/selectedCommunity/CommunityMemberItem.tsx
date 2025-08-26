import React, { type JSX, useState } from 'react';
import { useGetUserByIdQuery } from '../../../user/api/Account.api';
import User from '../../../user/components/User';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import type { CommunityUserModel } from '../../types/CommunityUserModel';

const CommunityMemberItem: React.FC<{ myself: AppUserModel | null, comunityUser: CommunityUserModel }> = ({ myself, comunityUser }) => {
    const [userInformation, setUserInformation] = useState<JSX.Element | null>(null);

    const { data: member, isLoading } = useGetUserByIdQuery(comunityUser.appUserId);

    if (isLoading || !member) {
        return (<></>);
    }

    return (
        <>
            <User
                myself={myself}
                targetUserId={member?.id}
                targetUsername={member.username}
                setUserInformation={setUserInformation}
            />
            {userInformation !== null &&
                <div className="community-user-information">{userInformation}</div>
            }
        </>
    );
}

export default CommunityMemberItem;