import type { AppUserModel } from '../types/AppUserModel';
import { UserApi } from './User.api';

export const AccountApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        getUserById: builder.query<AppUserModel, string>({
            query: id => `/Account/${id}`,
            providesTags: (result, error, id) =>
                result ? [{ type: 'Account', id }] : ['Account'],
        }),
        findByIdenityUserId: builder.query<AppUserModel, string>({
            query: identityUserId => `/Account/find/${identityUserId}`,
            providesTags: (result, error, id) =>
                result ? [{ type: 'Account', id }] : ['Account'],
        }),
        checkIfUserExist: builder.query<boolean, string>({
            query: email => `/Account/checkIfUserExist/${email}`
        }),
        editAsync: builder.mutation<boolean, AppUserModel>({
            query: user => ({
                body: user,
                url: '/Account',
                method: 'PUT'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'Account', id: arg.id }],
        }),
        logoutAsync: builder.mutation<void, void>({
            query: () => ({
                url: '/Account/logout',
                method: 'POST'
            }),
            invalidatesTags: ['Account'],
        }),
    })
})

export const {
    useGetUserByIdQuery,
    useLazyGetUserByIdQuery,
    useLazyFindByIdenityUserIdQuery,
    useLazyCheckIfUserExistQuery,
    useEditAsyncMutation,
    useLogoutAsyncMutation
} = AccountApi;