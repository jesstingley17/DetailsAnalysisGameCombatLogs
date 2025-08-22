import type { UserPostCommentModel } from '../types/UserPostCommentModel';
import { PostApi } from './Post.api';

export const UserPostCommentApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostComment: builder.mutation<UserPostCommentModel, UserPostCommentModel>({
            query: userPostComment => ({
                body: userPostComment,
                url: '/UserPostComment',
                method: 'POST'
            }),
            invalidatesTags: result => [{ type: 'UserPostComment', result }],
        }),
        updateUserPostComment: builder.mutation<void, UserPostCommentModel>({
            query: userPostComment => ({
                body: userPostComment,
                url: '/UserPostComment',
                method: 'PUT'
            }),
            invalidatesTags: result => [{ type: 'UserPostComment', result }],
        }),
        removeUserPostComment: builder.mutation<void, number>({
            query: id => ({
                url: `/UserPostComment/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: result => [{ type: 'UserPostComment', result }],
        }),
        searchUserPostCommentByPostId: builder.query<UserPostCommentModel[], number>({
            query: id => `/UserPostComment/searchByPostId/${id}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPostComment' as const, id })), { type: 'UserPostComment' }]
                    : [{ type: 'UserPostComment' }]
        }),
    })
});

export const {
    useCreateUserPostCommentMutation,
    useUpdateUserPostCommentMutation,
    useRemoveUserPostCommentMutation,
    useSearchUserPostCommentByPostIdQuery,
} = UserPostCommentApi;