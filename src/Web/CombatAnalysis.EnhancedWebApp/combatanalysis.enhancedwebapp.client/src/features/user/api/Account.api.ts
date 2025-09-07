import type { AppUserModel } from '../types/AppUserModel';
import { UserApi } from './User.api';

export const AccountApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        getUserById: builder.query<AppUserModel, string>({
            query: id => `/Account/${id}`,
            providesTags: result => result ? [{ type: 'Account', id: result.id }] : [],
        }),
        findByIdenityUserId: builder.query<AppUserModel, string>({
            query: identityUserId => `/Account/find/${identityUserId}`,
            providesTags: result => result ? [{ type: 'Account', id: result.id }] : [],
        }),
        checkIfUserExist: builder.query<boolean, string>({
            query: email => `/Account/checkIfUserExist/${email}`,
        }),
        editAsync: builder.mutation<boolean, AppUserModel>({
            query: user => ({
                body: user,
                url: '/Account',
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, user) => [{ type: 'Account', id: user.id }],
        }),
    })
})

export const {
    useGetUserByIdQuery,
    useLazyGetUserByIdQuery,
    useLazyFindByIdenityUserIdQuery,
    useLazyCheckIfUserExistQuery,
    useEditAsyncMutation,
} = AccountApi;