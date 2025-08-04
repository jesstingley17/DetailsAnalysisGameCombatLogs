import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { CommunityModel } from '../types/CommunityModel';
import type { InviteToCommunityModel } from '../types/InviteToCommunityModel';

const apiURL = '/api/v1';

export const CommunityApi = createApi({
    reducerPath: 'communityApi',
    tagTypes: [
        'Community',
        'CommunityUser',
        'InviteToCommunity',
        'CommunityDiscussion',
        'CommunityDiscussionComment',
        'InviteToCommunity',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        createCommunity: builder.mutation<CommunityModel, CommunityModel>({
            query: community => ({
                body: community,
                url: '/Community',
                method: 'POST'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'Community', arg }]
        }),
        updateCommunityAsync: builder.mutation<void, CommunityModel>({
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
        getCommunities: builder.query<CommunityModel[], void>({
            query: () => '/Community',
            providesTags: (result) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Community' as const, id })), { type: 'Community' }]
                    : [{ type: 'Community' }]
        }),
        getCommunityById: builder.query<CommunityModel, number>({
            query: id => `/Community/${id}`,
            providesTags: (result, error, id) => [{ type: 'Community', id }]
        }),
        getCommunitiesCount: builder.query<number, void>({
            query: () => '/Community/count'
        }),
        getCommunitiesWithPagination: builder.query<CommunityModel[], number>({
            query: pageSize => `/Community/getWithPagination?pageSize=${pageSize}`,
            providesTags: (result) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Community' as const, id })), { type: 'Community' }]
                    : [{ type: 'Community' }]
        }),
        getMoreCommunitiesWithPagination: builder.query<CommunityModel[], { offset: number, pageSize: number }>({
            query: ({ offset, pageSize }) => `/Community/getMoreWithPagination?offset=${offset}&pageSize=${pageSize}`,
            providesTags: (result) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Community' as const, id })), { type: 'Community' }]
                    : [{ type: 'Community' }]
        }),
        getInviteToCommunityById: builder.query<InviteToCommunityModel, number>({
            query: id => `/InviteToCommunity/${id}`,
            providesTags: (result, error, id) => [{ type: 'InviteToCommunity', id }]
        }),
    })
})

export const {
    useCreateCommunityMutation,
    useUpdateCommunityAsyncMutation,
    useRemoveCommunityAsyncMutation,
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
    useGetInviteToCommunityByIdQuery,
    useLazyGetInviteToCommunityByIdQuery,
} = CommunityApi;