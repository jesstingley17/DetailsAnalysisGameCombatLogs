import type { CommunityPostCommentModel } from '../types/CommunityPostCommentModel';
import { PostApi } from './Post.api';

export const CommunityPostCommentApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPostComment: builder.mutation<CommunityPostCommentModel, CommunityPostCommentModel>({
            query: communityPostComment => ({
                body: communityPostComment,
                url: '/CommunityPostComment',
                method: 'POST'
            }),
            invalidatesTags: result => [{ type: 'CommunityPostComment', result }],
        }),
        updateCommunityPostComment: builder.mutation<void, CommunityPostCommentModel>({
            query: communityPostComment => ({
                body: communityPostComment,
                url: '/CommunityPostComment',
                method: 'PUT'
            }),
            invalidatesTags: result => [{ type: 'CommunityPostComment', result }],
        }),
        removeCommunityPostComment: builder.mutation<void, number>({
            query: id => ({
                url: `/CommunityPostComment/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: result => [{ type: 'CommunityPostComment', result }],
        }),
        searchCommunityPostCommentByPostId: builder.query<CommunityPostCommentModel[], number>({
            query: id => `/CommunityPostComment/searchByPostId/${id}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPostComment' as const, id })), { type: 'CommunityPostComment' }]
                    : [{ type: 'CommunityPostComment' }]
        }),
    })
});

export const {
    useCreateCommunityPostCommentMutation,
    useUpdateCommunityPostCommentMutation,
    useRemoveCommunityPostCommentMutation,
    useSearchCommunityPostCommentByPostIdQuery,
} = CommunityPostCommentApi;