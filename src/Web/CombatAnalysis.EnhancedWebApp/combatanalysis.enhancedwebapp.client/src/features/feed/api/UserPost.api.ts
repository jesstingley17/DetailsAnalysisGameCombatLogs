import type { UserPostModel } from '../types/UserPostModel';
import { PostApi } from './Post.api';

export const UserPostApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPost: builder.mutation<UserPostModel, UserPostModel>({
            query: post => ({
                body: post,
                url: '/UserPost',
                method: 'POST'
            }),
            invalidatesTags: result => [{ type: 'UserPost', result }],
        }),
        updateUserPost: builder.mutation<void, UserPostModel>({
            query: post => ({
                body: post,
                url: '/UserPost',
                method: 'PUT'
            }),
            invalidatesTags: result => [{ type: 'UserPost', result }],
        }),
        removeUserPost: builder.mutation<void, number>({
            query: id => ({
                url: `/UserPost/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'UserPost', arg }]
        }),
        getUserPostById: builder.query<UserPostModel, number>({
            query: id => `/UserPost/${id}`,
            providesTags: result =>
                result
                    ? [{ type: 'UserPost', id: result.id }]
                    : [{ type: 'UserPost' }]
        }),
        getUserPostCountByUserId: builder.query<number, string>({
            query: appUserId => `/UserPost/count/${appUserId}`,
        }),
        getUserPostCountByListOfUserId: builder.query<number, string>({
            query: appUserIds => `/UserPost/countByListOfAppUsers/${appUserIds}`,
        }),
    })
})

export const {
    useCreateUserPostMutation,
    useUpdateUserPostMutation,
    useRemoveUserPostMutation,
    useGetUserPostCountByUserIdQuery,
    useLazyGetUserPostCountByUserIdQuery,
    useGetUserPostCountByListOfUserIdQuery,
    useLazyGetUserPostCountByListOfUserIdQuery,
    useGetUserPostByIdQuery,
    useLazyGetUserPostByIdQuery,
} = UserPostApi;