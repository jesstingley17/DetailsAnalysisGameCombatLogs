import { UserApi } from './User.api';

export const IdentityApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        authorizationCodeExchange: builder.query<void, string>({
            query: authorizationCode => `/Identity?authorizationCode=${authorizationCode}`
        }),
        getUserPrivacy: builder.query<string, string>({
            query: id => `/Identity/userPrivacy/${id}`
        }),
    })
})

export const {
    useLazyAuthorizationCodeExchangeQuery,
    useLazyGetUserPrivacyQuery,
} = IdentityApi;