import type { IdentityUserPrivacyModel } from '../types/IdentityUserPrivacyModel';
import { UserApi } from './User.api';

export const IdentityApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        logout: builder.mutation<void, void>({
            query: () => ({
                url: '/Identity/logout',
                method: 'POST'
            }),
            invalidatesTags: [{ type: 'Account' }]
        }),
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
    useLogoutMutation,
} = IdentityApi;