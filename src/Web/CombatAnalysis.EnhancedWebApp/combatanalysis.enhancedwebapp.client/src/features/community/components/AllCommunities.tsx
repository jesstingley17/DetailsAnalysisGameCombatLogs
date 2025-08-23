import CommunicationMenu from '@/shared/components/CommunicationMenu';
import UserCommunities from './userEnvironment/UserCommunities';
import Communities from './Communities';

const AllCommunities: React.FC = () => {
    return (
        <>
            <Communities />
            <UserCommunities />
            <CommunicationMenu
                currentMenuItem={2}
                hasSubMenu={false}
            />
        </>
    );
}

export default AllCommunities;