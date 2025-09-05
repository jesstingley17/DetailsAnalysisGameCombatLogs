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
            invalidatesTags: result => result ? [{ type: 'CommunityPostComment', id: result.id }] : [],
        }),
        updateCommunityPostComment: builder.mutation<void, CommunityPostCommentModel>({
            query: communityPostComment => ({
                body: communityPostComment,
                url: '/CommunityPostComment',
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, communityPostComment) => [{ type: 'CommunityPostComment', id: communityPostComment.id }],
        }),
        removeCommunityPostComment: builder.mutation<void, number>({
            query: id => ({
                url: `/CommunityPostComment/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'CommunityPostComment', id }],
        }),
        searchCommunityPostCommentByPostId: builder.query<CommunityPostCommentModel[], number>({
            query: id => `/CommunityPostComment/searchByPostId/${id}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(communityPostComment => ({ type: 'CommunityPostComment' as const, id: communityPostComment.id })),
                        { type: 'CommunityPostComment', id: 'LIST' },
                    ]
                    : [{ type: 'CommunityPostComment', id: 'LIST' }]
        }),
    })
});

export const {
    useCreateCommunityPostCommentMutation,
    useUpdateCommunityPostCommentMutation,
    useRemoveCommunityPostCommentMutation,
    useSearchCommunityPostCommentByPostIdQuery,
} = CommunityPostCommentApi;