import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { Community } from '../../../types/Community';
import { InviteToCommunity } from '../../../types/InviteToCommunity';

const apiURL = '/api/v1';

export const CommunityApi = createApi({
    reducerPath: 'communityApi',
    tagTypes: [
        'Community',
        'CommunityUser',
        'InviteToCommunity',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getCommunities: builder.query<Community[], void>({
            query: () => '/Community',
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Community' as const, id })), { type: 'Community' }]
                    : [{ type: 'Community' }]
        }),
        getCommunityById: builder.query<Community, number>({
            query: (id) => `/Community/${id}`,
            providesTags: (result, error, id) => [{ type: 'Community', id }]
        }),
        getCommunitiesCount: builder.query<number, void>({
            query: () => '/Community/count'
        }),
        getCommunitiesWithPagination: builder.query<Community[], number>({
            query: (pageSize) => `/Community/getWithPagination?pageSize=${pageSize}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Community' as const, id })), { type: 'Community' }]
                    : [{ type: 'Community' }]
        }),
        getMoreCommunitiesWithPagination: builder.query<Community[], { offset: number, pageSize: number }>({
            query: ({ offset, pageSize }) => `/Community/getMoreWithPagination?offset=${offset}&pageSize=${pageSize}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Community' as const, id })), { type: 'Community' }]
                    : [{ type: 'Community' }]
        }),
        createCommunity: builder.mutation<Community, Community>({
            query: community => ({
                body: community,
                url: '/Community',
                method: 'POST'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'Community', arg }]
        }),
        updateCommunityAsync: builder.mutation<void, Community>({
            query: community => ({
                body: community,
                url: '/Community',
                method: 'PUT'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'Community', arg }]
        }),
        removeCommunityAsync: builder.mutation<void, number>({
            query: id => ({
                url: `/Community/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'Community', arg }]
        }),
        getInviteToCommunityById: builder.query<InviteToCommunity, number>({
            query: (id) => `/InviteToCommunity/${id}`,
            providesTags: (result, error, id) => [{ type: 'InviteToCommunity', id }]
        }),
    })
})

export const {
    useGetCommunitiesQuery,
    useLazyGetCommunitiesQuery,
    useGetCommunityByIdQuery,
    useLazyGetCommunityByIdQuery,
    useGetCommunitiesCountQuery,
    useLazyGetCommunitiesCountQuery,
    useGetCommunitiesWithPaginationQuery,
    useLazyGetCommunitiesWithPaginationQuery,
    useGetMoreCommunitiesWithPaginationQuery,
    useLazyGetMoreCommunitiesWithPaginationQuery,
    useCreateCommunityMutation,
    useUpdateCommunityAsyncMutation,
    useRemoveCommunityAsyncMutation,
    useGetInviteToCommunityByIdQuery,
    useLazyGetInviteToCommunityByIdQuery,
} = CommunityApi;