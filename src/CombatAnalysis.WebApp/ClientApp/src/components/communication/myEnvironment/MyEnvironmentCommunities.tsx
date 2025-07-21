import CommunicationMenu from '../CommunicationMenu';
import MyCommunities from './MyCommunities';

import '../../../styles/communication/community/communities.scss';

const MyEnvironmentCommunities: React.FC = () => {
    return (
        <>
            <MyCommunities />
            <CommunicationMenu
                currentMenuItem={7}
                hasSubMenu={true}
            />
        </>
    );
}

export default MyEnvironmentCommunities;