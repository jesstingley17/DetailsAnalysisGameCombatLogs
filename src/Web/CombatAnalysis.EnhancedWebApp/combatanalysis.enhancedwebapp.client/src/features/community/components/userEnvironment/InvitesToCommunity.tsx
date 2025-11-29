import { useTranslation } from 'react-i18next';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useInviteFindByUserIdQuery } from '../../api/InviteToCommunity.api';
import type { InviteToCommunityModel } from '../../types/InviteToCommunityModel';
import InvitesToCommunityItem from './InvitesToCommunityItem';

const InvitesToCommunity: React.FC<{ user: AppUserModel | null }> = ({ user }) => {
    const { t } = useTranslation('communication/myEnvironment/invitesToCommunity');

    const { data: invitesToCommunity, isLoading } = useInviteFindByUserIdQuery(user?.id ?? "");

    if (isLoading || invitesToCommunity?.length === 0) {
        return (<></>);
    }

    return (
        <div className="invite-to-community">
            <div>{t("InvitesToCommunity")}</div>
            <ul>
                {
                    invitesToCommunity?.map((invite: InviteToCommunityModel) => (
                        <li key={invite.id}>
                            <InvitesToCommunityItem
                                user={user}
                                inviteToCommunity={invite}
                            />
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default InvitesToCommunity;