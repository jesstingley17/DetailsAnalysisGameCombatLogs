import type { CommunityDiscussionCommentModel } from '../types/CommunityDiscussionCommentModel';
import { CommunityApi } from './Community.api';

export const CommunityDiscussionCommentApi = CommunityApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityDiscussionCommentAsync: builder.mutation<CommunityDiscussionCommentModel, CommunityDiscussionCommentModel>({
            query: discussion => ({
                body: discussion,
                url: '/CommunityDiscussionComment',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'CommunityDiscussionComment', id: result.id }] : [],
        }),
        updateCommunityDiscussionCommentAsync: builder.mutation<void, CommunityDiscussionCommentModel>({
            query: discussion => ({
                body: discussion,
                url: '/CommunityDiscussionComment',
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, discussion) => [{ type: 'CommunityDiscussionComment', id: discussion.id }]
        }),
        removeCommunityDiscussionCommentAsync: builder.mutation<void, number>({
            query: id => ({
                url: `/CommunityDiscussionComment/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'CommunityDiscussionComment', id }]
        }),
        getCommunityDiscussionCommentById: builder.query<CommunityDiscussionCommentModel, number>({
            query: id => `/CommunityDiscussionComment/${id}`,
            providesTags: result => result ? [{ type: 'CommunityDiscussionComment', id: result.id }] : [],
        }),
        getCommunityDiscussionCommentByDiscussionId: builder.query<CommunityDiscussionCommentModel[], number>({
            query: id => `/CommunityDiscussionComment/findByDiscussionId/${id}`,
            providesTags: (_result, _error, id) => [{ type: 'CommunityDiscussionComment', id }]
        }),
    })
})

export const {
    useCreateCommunityDiscussionCommentAsyncMutation,
    useUpdateCommunityDiscussionCommentAsyncMutation,
    useRemoveCommunityDiscussionCommentAsyncMutation,
    useGetCommunityDiscussionCommentByIdQuery,
    useGetCommunityDiscussionCommentByDiscussionIdQuery,
} = CommunityDiscussionCommentApi;