import type { UserPostReactionModel } from '../types/UserPostReactionModel';
import { PostApi } from './Post.api';

export const UserPostLikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostLike: builder.mutation<UserPostReactionModel, UserPostReactionModel>({
            query: userPostLike => ({
                body: userPostLike,
                url: '/UserPostLike',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'UserPostLike', id: result.id }] : [],
        }),
        removeUserPostLike: builder.mutation<void, number>({
            query: id => ({
                url: `/UserPostLike/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'UserPostLike', id }],
        }),
        searchUserPostLikeByPostId: builder.query<UserPostReactionModel[], number>({
            query: id => `/UserPostLike/searchByPostId/${id}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(userPostLike => ({ type: 'UserPostLike' as const, id: userPostLike.id })),
                        { type: 'UserPostLike', id: 'LIST' },
                    ]
                    : [{ type: 'UserPostLike', id: 'LIST' }]
        }),
    })
})

export const {
    useCreateUserPostLikeMutation,
    useRemoveUserPostLikeMutation,
    useLazySearchUserPostLikeByPostIdQuery,
} = UserPostLikeApi;