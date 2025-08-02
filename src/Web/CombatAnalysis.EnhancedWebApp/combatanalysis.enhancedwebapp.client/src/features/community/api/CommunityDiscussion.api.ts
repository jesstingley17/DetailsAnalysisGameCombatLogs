import { type CommunityDiscussionModel } from '../types/CommunityDiscussionModel';
import { CommunityApi } from './Community.api';

export const CommunityDiscussionApi = CommunityApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityDiscussionAsync: builder.mutation<CommunityDiscussionModel, CommunityDiscussionModel>({
            query: discussion => ({
                body: discussion,
                url: '/CommunityDiscussion',
                method: 'POST'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityDiscussion', arg }]
        }),
        updateCommunityDiscussionAsync: builder.mutation<void, CommunityDiscussionModel>({
            query: discussion => ({
                body: discussion,
                url: '/CommunityDiscussion',
                method: 'PUT'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityDiscussion', arg }]
        }),
        removeCommunityDiscussionAsync: builder.mutation<void, number>({
            query: id => ({
                url: `/CommunityDiscussion/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityDiscussion', arg }]
        }),
        getCommunityDiscussionById: builder.query<CommunityDiscussionModel, number>({
            query: id => `/CommunityDiscussion/${id}`,
            providesTags: (result, error, id) => [{ type: 'CommunityDiscussion', id }]
        }),
        getCommunityDiscussionByCommunityId: builder.query<CommunityDiscussionModel[], number>({
            query: id => `/CommunityDiscussion/findByCommunityId/${id}`,
            providesTags: (result, error, id) => [{ type: 'CommunityDiscussion', id }]
        }),
    })
})

export const {
    useCreateCommunityDiscussionAsyncMutation,
    useUpdateCommunityDiscussionAsyncMutation,
    useRemoveCommunityDiscussionAsyncMutation,
    useGetCommunityDiscussionByIdQuery,
    useGetCommunityDiscussionByCommunityIdQuery,
    useLazyGetCommunityDiscussionByCommunityIdQuery,
} = CommunityDiscussionApi;