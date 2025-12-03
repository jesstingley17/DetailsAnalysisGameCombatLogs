import type { CommunityPostReactionModel } from '../types/CommunityPostReactionModel';
import { PostApi } from './Post.api';

export const CommunityPostDislikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPostDislike: builder.mutation<CommunityPostReactionModel, CommunityPostReactionModel>({
            query: communityPostDislike => ({
                body: communityPostDislike,
                url: '/CommunityPostDislike',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'CommunityPostDislike', id: result.id }] : [],
        }),
        removeCommunityPostDislike: builder.mutation<void, number>({
            query: id => ({
                url: `/CommunityPostDislike/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'CommunityPostDislike', id }],
        }),
        searchCommunityPostDislikeByPostId: builder.query<CommunityPostReactionModel[], number>({
            query: id => `/CommunityPostDislike/searchByPostId/${id}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(communityPostDislike => ({ type: 'CommunityPostDislike' as const, id: communityPostDislike.id })),
                        { type: 'CommunityPostDislike', id: 'LIST' },
                    ]
                    : [{ type: 'CommunityPostDislike', id: 'LIST' }]
        }),
    })
})

export const {
    useCreateCommunityPostDislikeMutation,
    useRemoveCommunityPostDislikeMutation,
    useLazySearchCommunityPostDislikeByPostIdQuery,
} = CommunityPostDislikeApi;