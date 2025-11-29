import type { RootState } from '@/app/Store';
import { useSelector } from 'react-redux';
import { useCommunityUserFindByUserIdQuery } from '../../../community/api/CommunityUser.api';
import type { CommunityUserModel } from '../../../community/types/CommunityUserModel';
import type { AppUserModel } from '../../types/AppUserModel';
import CommunityItem from './CommunityItem';

interface SelectedUserCommunitiesProps {
    user: AppUserModel | null;
    t: (key: string) => string;
}

const SelectedUserCommunities: React.FC<SelectedUserCommunitiesProps> = ({ user, t }) => {
    const me = useSelector((state: RootState) => state.user.value);

    const { data: communityUsers, isLoading } = useCommunityUserFindByUserIdQuery(user?.id || "");

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <div className="communities__list">
            {communityUsers?.length === 0
                ? <div>{t("Empty")}</div>
                : <ul>
                    {communityUsers?.map((communityUser: CommunityUserModel) => (
                        <li key={communityUser.id} className="community">
                            <CommunityItem
                                id={communityUser.communityId}
                                myself={me}
                            />
                        </li>
                    ))}
                </ul>
            }
        </div>
    );
}

export default SelectedUserCommunities;