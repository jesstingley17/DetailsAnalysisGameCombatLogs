import type { CommunityPostModel } from '../types/CommunityPostModel';
import { PostApi } from './Post.api';

export const CommunityPostApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPost: builder.mutation<CommunityPostModel, CommunityPostModel>({
            query: post => ({
                body: post,
                url: '/CommunityPost',
                method: 'POST'
            }),
            invalidatesTags: result => [{ type: 'CommunityPost', result }],
        }),
        updateCommunityPost: builder.mutation<void, CommunityPostModel>({
            query: post => ({
                body: post,
                url: '/CommunityPost',
                method: 'PUT'
            }),
            invalidatesTags: result => [{ type: 'CommunityPost', result }],
        }),
        removeCommunityPost: builder.mutation<void, number>({
            query: id => ({
                url: `/CommunityPost/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityPost', arg }]
        }),
        getCommunityPostById: builder.query<CommunityPostModel, number>({
            query: id => `/CommunityPost/${id}`,
            providesTags: result =>
                result
                    ? [{ type: 'CommunityPost', id: result.id }]
                    : [{ type: 'CommunityPost' }]
        }),
        getCommunityPostCountByCommunityId: builder.query<number, number>({
            query: communityId => `/CommunityPost/count/${communityId}`,
        }),
        getCommunityPostCountByListOfCommunityId: builder.query<number, string>({
            query: communityIds => `/CommunityPost/countByListOfCommunities/${communityIds}`,
        }),
    })
})

export const {
    useCreateCommunityPostMutation,
    useUpdateCommunityPostMutation,
    useRemoveCommunityPostMutation,
    useGetCommunityPostByIdQuery,
    useLazyGetCommunityPostByIdQuery,
    useGetCommunityPostCountByCommunityIdQuery,
    useLazyGetCommunityPostCountByCommunityIdQuery,
    useGetCommunityPostCountByListOfCommunityIdQuery,
    useLazyGetCommunityPostCountByListOfCommunityIdQuery,
} = CommunityPostApi;