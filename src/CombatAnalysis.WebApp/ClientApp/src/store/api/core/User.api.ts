import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { AppUser } from '../../../types/AppUser';
import { Customer } from '../../../types/Customer';
import { IdentityRedirect } from '../../../types/IdentityRedirect';

const apiURL = '/api/v1';

export const UserApi = createApi({
    reducerPath: 'userApi',
    tagTypes: [
        'Account',
        'Authentication',
        'Customer',
        'Friend',
        'RequestToConnect',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getUsers: builder.query<AppUser[], void>({
            query: () => '/Account',
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Account' as const, id })), 'Account']
                    : ['Account'],
        }),
        getCustomers: builder.query<Customer[], void>({
            query: () => '/Customer',
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Customer' as const, id })), 'Customer']
                    : ['Customer'],
        }),
        authentication: builder.query<AppUser, void>({
            query: () => '/Authentication',
            providesTags: (result, error, id) =>
                result ? [{ type: 'Authentication', id: result.id }] : ['Authentication'],
        }),
        authorization: builder.query<IdentityRedirect, string>({
            query: (identityPath) => `/Authentication/authorization?identityPath=${identityPath}`
        }),
        verifyEmail: builder.query<IdentityRedirect, { identityPath: string, email: string }>({
            query: ({ identityPath, email }) => `/Authentication/verifyEmail?identityPath=${identityPath}&email=${email}`
        }),
        stateValidate: builder.query <void, string>({
            query: (state) => `/Authentication/stateValidate?state=${state}`
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