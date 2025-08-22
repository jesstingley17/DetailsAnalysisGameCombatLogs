import type { CommunityPostReactionModel } from '../types/CommunityPostReactionModel';
import { PostApi } from './Post.api';

export const CommunityPostLikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPostLike: builder.mutation<CommunityPostReactionModel, CommunityPostReactionModel>({
            query: communityPostLike => ({
                body: communityPostLike,
                url: '/CommunityPostLike',
                method: 'POST'
            })
        }),
        removeCommunityPostLike: builder.mutation<void, number>({
            query: id => ({
                url: `/CommunityPostLike/${id}`,
                method: 'DELETE'
            })
        }),
        searchCommunityPostLikeByPostId: builder.query<CommunityPostReactionModel[], number>({
            query: id => `/CommunityPostLike/searchByPostId/${id}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPostLike' as const, id })), { type: 'CommunityPostLike' }]
                    : [{ type: 'CommunityPostLike' }]
        }),
    })
})

export const {
    useCreateCommunityPostLikeMutation,
    useRemoveCommunityPostLikeMutation,
    useLazySearchCommunityPostLikeByPostIdQuery,
} = CommunityPostLikeApi;