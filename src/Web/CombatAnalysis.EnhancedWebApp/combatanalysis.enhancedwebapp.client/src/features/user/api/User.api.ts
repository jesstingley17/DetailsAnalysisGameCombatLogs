import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { AppUserModel } from '../types/AppUserModel';
import type { CustomerModel } from '../types/CustomerModel';
import type { IdentityRedirectModel } from '../types/IdentityRedirectModel';

const apiURL = '/api/v1';

export const UserApi = createApi({
    reducerPath: 'userApi',
    tagTypes: [
        'Account',
        'Authentication',
        'Customer',
        'Friend',
        'RequestToConnect',
        'Identity',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getUsers: builder.query<AppUserModel[], void>({
            query: () => '/Account',
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Account' as const, id })), 'Account']
                    : ['Account'],
        }),
        getCustomers: builder.query<CustomerModel[], void>({
            query: () => '/Customer',
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Customer' as const, id })), 'Customer']
                    : ['Customer'],
        }),
        authentication: builder.query<AppUserModel, void>({
            query: () => '/Authentication',
            providesTags: result =>
                result ? [{ type: 'Authentication', id: result.id }] : ['Authentication'],
        }),
        authorization: builder.query<{ uri: string }, string>({
            query: identityPath => `/Authentication/authorization?identityPath=${identityPath}`,
        }),
        verifyEmail: builder.query<IdentityRedirectModel, { identityPath: string, email: string }>({
            query: ({ identityPath, email }) => `/Authentication/verifyEmail?identityPath=${identityPath}&email=${email}`,
        }),
        stateValidate: builder.query <void, string>({
            query: state => `/Authentication/stateValidate?state=${state}`,
        }),
    })
})

export const {
    useGetUsersQuery,
    useLazyGetUsersQuery,
    useGetCustomersQuery,
    useLazyGetCustomersQuery,
    useAuthenticationQuery,
    useLazyAuthenticationQuery,
    useLazyAuthorizationQuery,
    useLazyVerifyEmailQuery,
    useLazyStateValidateQuery,
} = UserApi;