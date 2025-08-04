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
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityDiscussionComment', arg }]
        }),
        updateCommunityDiscussionCommentAsync: builder.mutation<void, CommunityDiscussionCommentModel>({
            query: discussion => ({
                body: discussion,
                url: '/CommunityDiscussionComment',
                method: 'PUT'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityDiscussionComment', arg }]
        }),
        removeCommunityDiscussionCommentAsync: builder.mutation<void, number>({
            query: id => ({
                url: `/CommunityDiscussionComment/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityDiscussionComment', arg }]
        }),
        getCommunityDiscussionCommentById: builder.query<CommunityDiscussionCommentModel, number>({
            query: id => `/CommunityDiscussionComment/${id}`,
            providesTags: (result, error, id) => [{ type: 'CommunityDiscussionComment', id }]
        }),
        getCommunityDiscussionCommentByDiscussionId: builder.query<CommunityDiscussionCommentModel[], number>({
            query: id => `/CommunityDiscussionComment/findByDiscussionId/${id}`,
            providesTags: (result, error, id) => [{ type: 'CommunityDiscussionComment', id }]
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