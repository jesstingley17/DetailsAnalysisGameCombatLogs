import { combineReducers, configureStore } from '@reduxjs/toolkit';
import { ChatApi } from '../features/chat/api/Chat.api';
import { CommunityApi } from '../features/community/api/Community.api';
import communityMenuReducer from '../features/community/store/CommunityMenuSlice';
import { GameLogsApi } from '../features/gameLogs/api/GameLogs.api';
import { NotificationApi } from '../features/notification/api/Notification.api';
import { PostApi } from '../features/post/api/Post.api';
import { UserApi } from '../features/user/api/User.api';
import customerReducer from '../features/user/store/CustomerSlice';
import userPrivacyReducer from '../features/user/store/UserPrivacySlice';
import userReducer from '../features/user/store/UserSlice';
import authenticationMiddleware from '../middleware/authenticationMiddleware';

const reducers = combineReducers({
    customer: customerReducer,
    user: userReducer,
    userPrivacy: userPrivacyReducer,
    communityMenu: communityMenuReducer,
    [UserApi.reducerPath]: UserApi.reducer,
    [ChatApi.reducerPath]: ChatApi.reducer,
    [CommunityApi.reducerPath]: CommunityApi.reducer,
    [PostApi.reducerPath]: PostApi.reducer,
    [GameLogsApi.reducerPath]: GameLogsApi.reducer,
    [NotificationApi.reducerPath]: NotificationApi.reducer,
});

const Store = configureStore({
    reducer: reducers,
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware()
            .concat(UserApi.middleware)
            .concat(ChatApi.middleware)
            .concat(CommunityApi.middleware)
            .concat(PostApi.middleware)
            .concat(GameLogsApi.middleware)
            .concat(NotificationApi.middleware)
            .concat(authenticationMiddleware)
});

export type RootState = ReturnType<typeof Store.getState>;

export default Store;