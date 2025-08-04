import type { UserPostReactionModel } from '../types/UserPostReactionModel';
import { PostApi } from './Post.api';

export const UserPostDislikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostDislike: builder.mutation<UserPostReactionModel, UserPostReactionModel>({
            query: userPostDislike => ({
                body: userPostDislike,
                url: '/UserPostDislike',
                method: 'POST'
            })
        }),
        removeUserPostDislike: builder.mutation<void, number>({
            query: id => ({
                url: `/UserPostDislike/${id}`,
                method: 'DELETE'
            })
        }),
        searchUserPostDislikeByPostId: builder.query<UserPostReactionModel[], number>({
            query: id => `/UserPostDislike/searchByPostId/${id}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPostDislike' as const, id })), { type: 'UserPostDislike' }]
                    : [{ type: 'UserPostDislike' }]
        }),
    })
})

export const {
    useCreateUserPostDislikeMutation,
    useRemoveUserPostDislikeMutation,
    useLazySearchUserPostDislikeByPostIdQuery,
} = UserPostDislikeApi;