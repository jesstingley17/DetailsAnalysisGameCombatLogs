import Chats from '../features/chat/components/Chats';
//import VoiceChat from '../features/chat/components/voiceChat/VoiceChat';
import AllCommunities from '../features/community/components/AllCommunities';
import SelectedCommunity from '../features/community/components/selectedCommunity/SelectedCommunity';
import UserEnvironmentCommunities from '../features/community/components/userEnvironment/UserEnvironmentCommunities';
import Feed from '../features/feed/components/Feed';
import CombatAuras from '../features/gameLogs/components/actions/CombatAuras';
import CombatDetails from '../features/gameLogs/components/CombatDetails';
import DetailsSpecificalCombat from '../features/gameLogs/components/DetailsSpecificalCombat';
import GameCombatLogs from '../features/gameLogs/components/GameCombatLogs';
import GeneralAnalysis from '../features/gameLogs/components/GeneralAnalysis';
import AuthorizationCallback from '../features/user/components/identity/AuthorizationCallback';
import People from '../features/user/components/people/People';
import SelectedUser from '../features/user/components/selectedUser/SelectedUser';
import Friends from '../features/user/components/userEnvironment/Friends';
import Profile from '../features/user/components/userEnvironment/Profile';
import UserFeed from '../features/user/components/userEnvironment/UserFeed';
import Home from '../shared/components/Home';

type Route = {
    index?: boolean;
    path?: string;
    element: React.ReactNode;
}

const AppRoutes: Route[] = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/callback',
        element: <AuthorizationCallback />
    },
    {
        path: '/game-combat-logs',
        element: <GameCombatLogs />
    },
    {
        path: '/feed',
        element: <Feed />
    },
    {
        path: '/chats',
        element: <Chats />
    },
    //{
    //    path: '/chats/voice/:roomId/:chatName',
    //    element: <VoiceChat />
    //},
    {
        path: '/communities',
        element: <AllCommunities />
    },
    {
        path: '/people',
        element: <People />
    },
    {
        path: '/user',
        element: <SelectedUser />
    },
    {
        path: '/environment/feed',
        element: <UserFeed />
    },
    {
        path: '/environment/friends',
        element: <Friends />
    },
    {
        path: '/environment/communities',
        element: <UserEnvironmentCommunities />
    },
    {
        path: '/environment/profile',
        element: <Profile />
    },
    {
        path: '/community',
        element: <SelectedCommunity />
    },
    {
        path: '/general-analysis',
        element: <GeneralAnalysis />
    },
    {
        path: '/general-analysis/auras',
        element: <CombatAuras />
    },
    {
        path: '/details-specifical-combat',
        element: <DetailsSpecificalCombat />
    },
    {
        path: '/combat-details',
        element: <CombatDetails />
    },
];

export default AppRoutes;
