import type { IdentityUserPrivacyModel } from '../types/IdentityUserPrivacyModel';
import { UserApi } from './User.api';

export const IdentityApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        authorizationCodeExchange: builder.query<void, string>({
            query: authorizationCode => `/Identity?authorizationCode=${authorizationCode}`
        }),
        refreshToken: builder.query<void, void>({
            query: () => `/Identity/refresh`
        }),
        getUserPrivacy: builder.query<IdentityUserPrivacyModel, string>({
            query: id => `/Identity/userPrivacy/${id}`
        }),
    })
})

export const {
    useLazyAuthorizationCodeExchangeQuery,
    useLazyRefreshTokenQuery,
    useLazyGetUserPrivacyQuery,
} = IdentityApi;