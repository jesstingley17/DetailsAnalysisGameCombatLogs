import { faCircleCheck, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useCreateCommunityUserMutation } from '../../../store/api/community/CommunityUser.api';
import { useRemoveCommunityInviteMutation } from '../../../store/api/community/InviteToCommunity.api';
import { useGetCommunityByIdQuery } from '../../../store/api/core/Community.api';
import { useGetUserByIdQuery } from '../../../store/api/user/Account.api';
import { CommunityUser } from '../../../types/CommunityUser';
import { InvitesToCommunityItemProps } from '../../../types/components/communication/myEnvironment/InvitesToCommunityItemProps';
import User from '../User';

const InvitesToCommunityItem: React.FC<InvitesToCommunityItemProps>  = ({ user, inviteToCommunity }) => {
    const { t } = useTranslation("communication/myEnvironment/invitesToCommunityItem");

    const { data: community, isLoading: communityIsLoading } = useGetCommunityByIdQuery(inviteToCommunity?.communityId);
    const { data: inviteOwner, isLoading: targetUserIsLoading } = useGetUserByIdQuery(inviteToCommunity?.appUserId);
    const [createCommunityUserAsyn] = useCreateCommunityUserMutation();
    const [removeInviteAsync] = useRemoveCommunityInviteMutation();

    const [userInformation, setUserInformation] = useState(null);

    const acceptRequestAsync = async () => {
        try {
            const newCommunityUser: CommunityUser = {
                id: " ",
                username: user?.username,
                communityId: community?.id || 0,
                appUserId: user?.id
            };

            await createCommunityUserAsyn(newCommunityUser);

            await removeInviteAsync(inviteToCommunity.id);
        } catch (e) {
            console.error(e);
        }
    }

    const rejectRequestAsync = async () => {
        await removeInviteAsync(inviteToCommunity.id);
    }

    if (communityIsLoading || targetUserIsLoading) {
        return <></>;
    }

    return (
        <div className="request-to-connect">
            <div className="request-to-connect__content">
                <User
                    me={user}
                    targetUserId={inviteOwner.id}
                    setUserInformation={setUserInformation}
                />
                <div>{t("SentInvite")}</div>
                <div className="community-name">{community?.name}</div>
            </div>
            <div className="request-to-connect__answer">
                <div className="accept"><FontAwesomeIcon icon={faCircleCheck} title={t("Accept") || ""}
                    onClick={acceptRequestAsync} /></div>
                <div className="reject"><FontAwesomeIcon icon={faCircleXmark} title={t("Reject") || ""}
                    onClick={rejectRequestAsync} /></div>
            </div>
            {userInformation}
        </div>
    );
}

export default InvitesToCommunityItem;