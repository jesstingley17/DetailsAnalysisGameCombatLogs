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
                    ? [
                        ...result.map(userPost => ({ type: 'UserPost' as const, id: userPost.id })),
                        { type: 'UserPost', id: 'LIST' },
                    ]
                    : [{ type: 'UserPost', id: 'LIST' }]
        }),
        getUserPostsByUserId: builder.query<UserPostModel[], { appUserId: string, pageSize: number }>({
            query: ({ appUserId, pageSize }) => `/UserPost/getByUserId?appUserId=${appUserId}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(userPost => ({ type: 'UserPost' as const, id: userPost.id })),
                        { type: 'UserPost', id: 'LIST' },
                    ]
                    : [{ type: 'UserPost', id: 'LIST' }]
        }),
        getMoreUserPostsByUserId: builder.query<UserPostModel[], { appUserId: string, offset: number, pageSize: number }>({
            query: ({ appUserId, offset, pageSize }) => `/UserPost/getMoreByUserId?appUserId=${appUserId}&offset=${offset}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(userPost => ({ type: 'UserPost' as const, id: userPost.id })),
                        { type: 'UserPost', id: 'LIST' },
                    ]
                    : [{ type: 'UserPost', id: 'LIST' }]
        }),
        getNewUserPostsByUserId: builder.query<UserPostModel[], { appUserId: string, checkFrom: string }>({
            query: ({ appUserId, checkFrom }) => `/UserPost/getNewPosts?appUserId=${appUserId}&checkFrom=${checkFrom}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(userPost => ({ type: 'UserPost' as const, id: userPost.id })),
                        { type: 'UserPost', id: 'LIST' },
                    ]
                    : [{ type: 'UserPost', id: 'LIST' }]
        }),
        getUserPostByListOfUserId: builder.query<UserPostModel[], { collectionUserId: string, pageSize: number }>({
            query: ({ collectionUserId, pageSize }) => `/UserPost/getByListOfUserId?collectionUserId=${collectionUserId}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(userPost => ({ type: 'UserPost' as const, id: userPost.id })),
                        { type: 'UserPost', id: 'LIST' },
                    ]
                    : [{ type: 'UserPost', id: 'LIST' }]
        }),
        getMoreUserPostByListOfUserId: builder.query<UserPostModel[], { collectionUserId: string, offset: number, pageSize: number }>({
            query: ({ collectionUserId, offset, pageSize }) => `/UserPost/getMoreByListOfUserId?collectionUserId=${collectionUserId}&offset=${offset}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(userPost => ({ type: 'UserPost' as const, id: userPost.id })),
                        { type: 'UserPost', id: 'LIST' },
                    ]
                    : [{ type: 'UserPost', id: 'LIST' }]
        }),
        getNewUserPostByListOfUserId: builder.query<UserPostModel[], { collectionUserId: string, checkFrom: string }>({
            query: ({ collectionUserId, checkFrom }) => `/UserPost/getNewByListOfUserId?collectionUserId=${collectionUserId}&checkFrom=${checkFrom}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(userPost => ({ type: 'UserPost' as const, id: userPost.id })),
                        { type: 'UserPost', id: 'LIST' },
                    ]
                    : [{ type: 'UserPost', id: 'LIST' }]
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
                    ? [
                        ...result.map(communityPost => ({ type: 'CommunityPost' as const, id: communityPost.id })),
                        { type: 'CommunityPost', id: 'LIST' },
                    ]
                    : [{ type: 'CommunityPost', id: 'LIST' }]
        }),
        getNewCommunityPostsByCommunityId: builder.query<CommunityPostModel[], { communityId: number, checkFrom: string }>({
            query: ({ communityId, checkFrom }) => `/CommunityPost/getNewPosts?communityId=${communityId}&checkFrom=${checkFrom}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(communityPost => ({ type: 'CommunityPost' as const, id: communityPost.id })),
                        { type: 'CommunityPost', id: 'LIST' },
                    ]
                    : [{ type: 'CommunityPost', id: 'LIST' }]
        }),
        getCommunityPostByListOfCommunityId: builder.query<CommunityPostModel[], { collectionCommunityId: string, pageSize: number }>({
            query: ({ collectionCommunityId, pageSize }) => `/CommunityPost/getByListOfCommunityId?collectionCommunityId=${collectionCommunityId}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(communityPost => ({ type: 'CommunityPost' as const, id: communityPost.id })),
                        { type: 'CommunityPost', id: 'LIST' },
                    ]
                    : [{ type: 'CommunityPost', id: 'LIST' }]
        }),
        getMoreCommunityPostByListOfCommunityId: builder.query<CommunityPostModel[], { collectionCommunityId: string, offset: number, pageSize: number }>({
            query: ({ collectionCommunityId, offset, pageSize }) => `/CommunityPost/getMoreByListOfCommunityId?collectionCommunityId=${collectionCommunityId}&offset=${offset}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(communityPost => ({ type: 'CommunityPost' as const, id: communityPost.id })),
                        { type: 'CommunityPost', id: 'LIST' },
                    ]
                    : [{ type: 'CommunityPost', id: 'LIST' }]
        }),
        getNewCommunityPostByListOfCommunityId: builder.query<CommunityPostModel[], { collectionCommunityId: string, checkFrom: string }>({
            query: ({ collectionCommunityId, checkFrom }) => `/CommunityPost/getNewByListOfCommunityId?collectionCommunityId=${collectionCommunityId}&checkFrom=${checkFrom}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(communityPost => ({ type: 'CommunityPost' as const, id: communityPost.id })),
                        { type: 'CommunityPost', id: 'LIST' },
                    ]
                    : [{ type: 'CommunityPost', id: 'LIST' }]
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
    useGetUserPostByListOfUserIdQuery,
    useLazyGetUserPostByListOfUserIdQuery,
    useGetMoreUserPostByListOfUserIdQuery,
    useLazyGetMoreUserPostByListOfUserIdQuery,
    useGetNewUserPostByListOfUserIdQuery,
    useLazyGetNewUserPostByListOfUserIdQuery,
    useGetCommunityPostsByCommunityIdQuery,
    useLazyGetCommunityPostsByCommunityIdQuery,
    useGetMoreCommunityPostsByCommunityIdQuery,
    useLazyGetMoreCommunityPostsByCommunityIdQuery,
    useGetNewCommunityPostsByCommunityIdQuery,
    useLazyGetNewCommunityPostsByCommunityIdQuery,
    useGetCommunityPostByListOfCommunityIdQuery,
    useLazyGetCommunityPostByListOfCommunityIdQuery,
    useGetMoreCommunityPostByListOfCommunityIdQuery,
    useLazyGetMoreCommunityPostByListOfCommunityIdQuery,
    useGetNewCommunityPostByListOfCommunityIdQuery,
    useLazyGetNewCommunityPostByListOfCommunityIdQuery,
} = PostApi;