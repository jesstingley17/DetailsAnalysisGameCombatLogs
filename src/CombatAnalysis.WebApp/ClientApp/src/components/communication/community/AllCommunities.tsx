import CommunicationMenu from "../CommunicationMenu";
import MyCommunities from "../myEnvironment/MyCommunities";
import Communities from "./Communities";

const AllCommunities: React.FC = () => {
    return (
        <>
            <Communities />
            <MyCommunities />
            <CommunicationMenu
                currentMenuItem={2}
                hasSubMenu={false}
            />
        </>
    );
}

export default AllCommunities;