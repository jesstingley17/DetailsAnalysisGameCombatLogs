import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { CommunityPostModel } from '../types/CommunityPostModel';
import type { UserPostModel } from '../types/UserPostModel';

const apiURL = '/api/v1';

export const PostApi = createApi({
    reducerPath: 'postApi',
    tagTypes: [
        'UserPost',
        'UserPostLike',
        'UserPostDislike',
        'UserPostComment',
        'CommunityPost',
        'CommunityPostLike',
        'CommunityPostDislike',
        'CommunityPostComment',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getUserPosts: builder.query<UserPostModel[], void>({
            query: () => '/UserPost',
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost' as const, id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
        }),
        getUserPostsByUserId: builder.query<UserPostModel[], { appUserId: string, pageSize: number }>({
            query: ({ appUserId, pageSize }) => `/UserPost/getByUserId?appUserId=${appUserId}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost' as const, id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
        }),
        getMoreUserPostsByUserId: builder.query<UserPostModel[], { appUserId: string, offset: number, pageSize: number }>({
            query: ({ appUserId, offset, pageSize }) => `/UserPost/getMoreByUserId?appUserId=${appUserId}&offset=${offset}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost' as const, id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
        }),
        getNewUserPostsByUserId: builder.query<UserPostModel[], { appUserId: string, checkFrom: string }>({
            query: ({ appUserId, checkFrom }) => `/UserPost/getNewPosts?appUserId=${appUserId}&checkFrom=${checkFrom}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost' as const, id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
        }),
        getUserPostByListOfUserIds: builder.query<UserPostModel[], { appUserIds: string, pageSize: number }>({
            query: ({ appUserIds, pageSize }) => `/UserPost/getByListOfUserIds?appUserIds=${appUserIds}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost' as const, id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
        }),
        getMoreUserPostByListOfUserIds: builder.query<UserPostModel[], { appUserIds: string, offset: number, pageSize: number }>({
            query: ({ appUserIds, offset, pageSize }) => `/UserPost/getMoreByListOfUserIds?appUserIds=${appUserIds}&offset=${offset}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost' as const, id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
        }),
        getNewUserPostByListOfUserIds: builder.query<UserPostModel[], { appUserIds: string, checkFrom: string }>({
            query: ({ appUserIds, checkFrom }) => `/UserPost/getNewByListOfUserIds?appUserIds=${appUserIds}&checkFrom=${checkFrom}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost' as const, id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
        }),
        getCommunityPostsByCommunityId: builder.query<CommunityPostModel[], { communityId: number, pageSize: number }>({
            query: ({ communityId, pageSize }) => `/CommunityPost/getByCommunityId?communityId=${communityId}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPost' as const, id })), { type: 'CommunityPost' }]
                    : [{ type: 'CommunityPost' }]
        }),
        getMoreCommunityPostsByCommunityId: builder.query<CommunityPostModel[], { communityId: number, offset: number, pageSize: number }>({
            query: ({ communityId, offset, pageSize }) => `/CommunityPost/getMoreByCommunityId?communityId=${communityId}&offset=${offset}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPost' as const, id })), { type: 'CommunityPost' }]
                    : [{ type: 'CommunityPost' }]
        }),
        getNewCommunityPostsByCommunityId: builder.query<CommunityPostModel[], { communityId: number, checkFrom: string }>({
            query: ({ communityId, checkFrom }) => `/CommunityPost/getNewPosts?communityId=${communityId}&checkFrom=${checkFrom}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPost' as const, id })), { type: 'CommunityPost' }]
                    : [{ type: 'CommunityPost' }]
        }),
        getCommunityPostByListOfCommunityIds: builder.query<CommunityPostModel[], { communityIds: string, pageSize: number }>({
            query: ({ communityIds, pageSize }) => `/CommunityPost/getByListOfCommunityIds?communityIds=${communityIds}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPost' as const, id })), { type: 'CommunityPost' }]
                    : [{ type: 'CommunityPost' }]
        }),
        getMoreCommunityPostByListOfCommunityIds: builder.query<CommunityPostModel[], { communityIds: string, offset: number, pageSize: number }>({
            query: ({ communityIds, offset, pageSize }) => `/CommunityPost/getMoreByListOfCommunityIds?communityIds=${communityIds}&offset=${offset}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPost' as const, id })), { type: 'CommunityPost' }]
                    : [{ type: 'CommunityPost' }]
        }),
        getNewCommunityPostByListOfCommunityIds: builder.query<CommunityPostModel[], { communityIds: string, checkFrom: string }>({
            query: ({ communityIds, checkFrom }) => `/CommunityPost/getNewByListOfCommunityIds?communityIds=${communityIds}&checkFrom=${checkFrom}`,
            providesTags: (result) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPost' as const, id })), { type: 'CommunityPost' }]
                    : [{ type: 'CommunityPost' }]
        }),
    })
})

export const {
    useGetUserPostsQuery,
    useLazyGetUserPostsQuery,
    useGetUserPostsByUserIdQuery,
    useLazyGetUserPostsByUserIdQuery,
    useGetMoreUserPostsByUserIdQuery,
    useLazyGetMoreUserPostsByUserIdQuery,
    useGetNewUserPostsByUserIdQuery,
    useLazyGetNewUserPostsByUserIdQuery,
    useGetUserPostByListOfUserIdsQuery,
    useLazyGetUserPostByListOfUserIdsQuery,
    useGetMoreUserPostByListOfUserIdsQuery,
    useLazyGetMoreUserPostByListOfUserIdsQuery,
    useGetNewUserPostByListOfUserIdsQuery,
    useLazyGetNewUserPostByListOfUserIdsQuery,
    useGetCommunityPostsByCommunityIdQuery,
    useLazyGetCommunityPostsByCommunityIdQuery,
    useGetMoreCommunityPostsByCommunityIdQuery,
    useLazyGetMoreCommunityPostsByCommunityIdQuery,
    useGetNewCommunityPostsByCommunityIdQuery,
    useLazyGetNewCommunityPostsByCommunityIdQuery,
    useGetCommunityPostByListOfCommunityIdsQuery,
    useLazyGetCommunityPostByListOfCommunityIdsQuery,
    useGetMoreCommunityPostByListOfCommunityIdsQuery,
    useLazyGetMoreCommunityPostByListOfCommunityIdsQuery,
    useGetNewCommunityPostByListOfCommunityIdsQuery,
    useLazyGetNewCommunityPostByListOfCommunityIdsQuery,
} = PostApi;