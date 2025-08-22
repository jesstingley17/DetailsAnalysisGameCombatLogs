import type { UserPostReactionModel } from '../types/UserPostReactionModel';
import { PostApi } from './Post.api';

export const UserPostLikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostLike: builder.mutation<UserPostReactionModel, UserPostReactionModel>({
            query: userPostLike => ({
                body: userPostLike,
                url: '/UserPostLike',
                method: 'POST'
            })
        }),
        removeUserPostLike: builder.mutation<void, number>({
            query: id => ({
                url: `/UserPostLike/${id}`,
                method: 'DELETE'
            })
        }),
        searchUserPostLikeByPostId: builder.query<UserPostReactionModel[], number>({
            query: id => `/UserPostLike/searchByPostId/${id}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPostLike' as const, id })), { type: 'UserPostLike' }]
                    : [{ type: 'UserPostLike' }]
        }),
    })
})

export const {
    useCreateUserPostLikeMutation,
    useRemoveUserPostLikeMutation,
    useLazySearchUserPostLikeByPostIdQuery,
} = UserPostLikeApi;