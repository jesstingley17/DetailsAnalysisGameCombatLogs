import CommunicationMenu from '@/shared/components/CommunicationMenu';
import UserCommunities from './UserCommunities';

import '../Communities.scss';

const UserEnvironmentCommunities: React.FC = () => {
    return (
        <>
            <UserCommunities />
            <CommunicationMenu
                currentMenuItem={7}
                hasSubMenu={true}
            />
        </>
    );
}

export default UserEnvironmentCommunities;