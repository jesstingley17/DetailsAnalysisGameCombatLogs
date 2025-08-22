import { useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import Loading from '../../../shared/components/Loading';
import { useCommunityUserSearchByUserIdQuery } from '../../community/api/CommunityUser.api';
import { useCreateInviteAsyncMutation, useLazyInviteIsExistQuery } from '../../community/api/InviteToCommunity.api';
import type { InviteToCommunityModel } from '../../community/types/InviteToCommunityModel';
import type { AppUserModel } from '../types/AppUserModel';
import TargetCommunity from './TargetCommunity';

import './PeopleInvitesToCommunity.scss';

interface PeopleInvitesToCommunityProps {
    me: AppUserModel;
    targetUser: AppUserModel;
    setOpenInviteToCommunity(value: SetStateAction<boolean>): void;
}

const PeopleInvitesToCommunity: React.FC<PeopleInvitesToCommunityProps> = ({ me, targetUser, setOpenInviteToCommunity }) => {
    const { t } = useTranslation('communication/people/people');

    const { data: communityUsers, isLoading } = useCommunityUserSearchByUserIdQuery(me?.id);

    const [communityIdToInvite, setCommunityIdToInvite] = useState<number[]>([]);

    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();
    const [isInviteExistAsync] = useLazyInviteIsExistQuery();

    const checkIfRequestExistAsync = async (appUserId: string, communityId: number) => {
        const isExist = await isInviteExistAsync({ appUserId, communityId });
        if (isExist.error !== undefined) {
            return true;
        }

        return isExist.data;
    }

    const createInviteAsync = async () => {
        for (let i = 0; i < communityIdToInvite.length; i++) {
            const isExist = await checkIfRequestExistAsync(targetUser.id, communityIdToInvite[i]);
            if (isExist) {
                continue;
            }

            const newInviteToCommunity: InviteToCommunityModel = {
                id: 0,
                communityId: communityIdToInvite[i],
                toAppUserId: targetUser?.id,
                when: new Date(),
                appUserId: me?.id
            }

            await createInviteAsyncMut(newInviteToCommunity);
        }

        setOpenInviteToCommunity(false);
    }

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <div className="invites">
            <div className="title">{t("InviteToCommunity")}</div>
            <ul>
                {
                    communityUsers?.map(item => (
                        <li key={item.id}>
                            <TargetCommunity
                                communityId={item.communityId}
                                communityIdToInvite={communityIdToInvite}
                                setCommunityIdToInvite={setCommunityIdToInvite}
                            />
                        </li>
                    ))
                }
            </ul>
            <div className="actions">
                <div className="btn-shadow send" onClick={createInviteAsync}>{t("Send")}</div>
                <div className="btn-shadow" onClick={() => setOpenInviteToCommunity((item) => !item)}>{t("Cancel")}</div>
            </div>
        </div>
    );
}

export default PeopleInvitesToCommunity;