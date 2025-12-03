import type { CommunityPostReactionModel } from '../types/CommunityPostReactionModel';
import { PostApi } from './Post.api';

export const CommunityPostLikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPostLike: builder.mutation<CommunityPostReactionModel, CommunityPostReactionModel>({
            query: communityPostLike => ({
                body: communityPostLike,
                url: '/CommunityPostLike',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'CommunityPostLike', id: result.id }] : [],
        }),
        removeCommunityPostLike: builder.mutation<void, number>({
            query: id => ({
                url: `/CommunityPostLike/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'CommunityPostDislike', id }],
        }),
        searchCommunityPostLikeByPostId: builder.query<CommunityPostReactionModel[], number>({
            query: id => `/CommunityPostLike/searchByPostId/${id}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(communityPostLike => ({ type: 'CommunityPostLike' as const, id: communityPostLike.id })),
                        { type: 'CommunityPostLike', id: 'LIST' },
                    ]
                    : [{ type: 'CommunityPostLike', id: 'LIST' }]
        }),
    })
})

export const {
    useCreateCommunityPostLikeMutation,
    useRemoveCommunityPostLikeMutation,
    useLazySearchCommunityPostLikeByPostIdQuery,
} = CommunityPostLikeApi;