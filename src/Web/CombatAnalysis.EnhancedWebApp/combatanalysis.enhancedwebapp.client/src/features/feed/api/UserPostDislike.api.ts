import type { UserPostReactionModel } from '../types/UserPostReactionModel';
import { PostApi } from './Post.api';

export const UserPostDislikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostDislike: builder.mutation<UserPostReactionModel, UserPostReactionModel>({
            query: userPostDislike => ({
                body: userPostDislike,
                url: '/UserPostDislike',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'UserPostDislike', id: result.id }] : [],
        }),
        removeUserPostDislike: builder.mutation<void, number>({
            query: id => ({
                url: `/UserPostDislike/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'UserPostDislike', id }],
        }),
        searchUserPostDislikeByPostId: builder.query<UserPostReactionModel[], number>({
            query: id => `/UserPostDislike/searchByPostId/${id}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(userPostDislike => ({ type: 'UserPostDislike' as const, id: userPostDislike.id })),
                        { type: 'UserPostDislike', id: 'LIST' },
                    ]
                    : [{ type: 'UserPostDislike', id: 'LIST' }]
        }),
    })
})

export const {
    useCreateUserPostDislikeMutation,
    useRemoveUserPostDislikeMutation,
    useLazySearchUserPostDislikeByPostIdQuery,
} = UserPostDislikeApi;