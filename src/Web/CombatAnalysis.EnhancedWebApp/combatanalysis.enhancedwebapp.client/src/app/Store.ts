import { combineReducers, configureStore } from '@reduxjs/toolkit';
import { UserApi } from '../features/user/api/User.api';
import customerReducer from '../features/user/store/CustomerSlice';
import userPrivacyReducer from '../features/user/store/UserPrivacySlice';
import userReducer from '../features/user/store/UserSlice';
import authenticationMiddleware from '../middleware/authenticationMiddleware';
import { ChatApi } from '../features/chat/api/Chat.api';
import { CombatParserApi } from './api/core/CombatParser.api';
import { CommunityApi } from '../features/community/api/Community.api';
import { NotificationApi } from './api/core/Notification.api';
import { PostApi } from './api/core/Post.api';
import communityMenuReducer from './slicers/CommunityMenuSlice';

const reducers = combineReducers({
    customer: customerReducer,
    user: userReducer,
    userPrivacy: userPrivacyReducer,
    communityMenu: communityMenuReducer,
    [UserApi.reducerPath]: UserApi.reducer,
    [ChatApi.reducerPath]: ChatApi.reducer,
    [CommunityApi.reducerPath]: CommunityApi.reducer,
    [PostApi.reducerPath]: PostApi.reducer,
    [CombatParserApi.reducerPath]: CombatParserApi.reducer,
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
            .concat(CombatParserApi.middleware)
            .concat(NotificationApi.middleware)
            .concat(authenticationMiddleware)
});

export type RootState = ReturnType<typeof Store.getState>;

export default Store;