import type { CommunityPostReactionModel } from '../types/CommunityPostReactionModel';
import { PostApi } from './Post.api';

export const CommunityPostDislikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPostDislike: builder.mutation<CommunityPostReactionModel, CommunityPostReactionModel>({
            query: communityPostDislike => ({
                body: communityPostDislike,
                url: '/CommunityPostDislike',
                method: 'POST'
            })
        }),
        removeCommunityPostDislike: builder.mutation<void, number>({
            query: id => ({
                url: `/CommunityPostDislike/${id}`,
                method: 'DELETE'
            })
        }),
        searchCommunityPostDislikeByPostId: builder.query<CommunityPostReactionModel[], number>({
            query: id => `/CommunityPostDislike/searchByPostId/${id}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPostDislike' as const, id })), { type: 'CommunityPostDislike' }]
                    : [{ type: 'CommunityPostDislike' }]
        }),
    })
})

export const {
    useCreateCommunityPostDislikeMutation,
    useRemoveCommunityPostDislikeMutation,
    useLazySearchCommunityPostDislikeByPostIdQuery,
} = CommunityPostDislikeApi;