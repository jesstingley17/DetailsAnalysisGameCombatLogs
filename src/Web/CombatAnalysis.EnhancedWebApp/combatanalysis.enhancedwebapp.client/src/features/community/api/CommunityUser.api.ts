import type { CommunityUserModel } from '../types/CommunityUserModel';
import { CommunityApi } from './Community.api';

export const CommunityUserApi = CommunityApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityUser: builder.mutation<CommunityUserModel, CommunityUserModel>({
            query: (communityUser) => ({
                body: communityUser,
                url: '/CommunityUser',
                method: 'POST'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityUser', id: arg.id }]
        }),
        removeCommunityUser: builder.mutation<void, string>({
            query: id => ({
                url: `/CommunityUser/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, id) => [{ type: 'CommunityUser', id }]
        }),
        communityUserSearchByCommunityId: builder.query<CommunityUserModel[], number>({
            query: communityId => `/CommunityUser/searchByCommunityId/${communityId}`,
            providesTags: (result) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityUser' as const, id })), { type: 'CommunityUser' }]
                    : [{ type: 'CommunityUser' }]
        }),
        communityUserSearchByUserId: builder.query<CommunityUserModel[], string>({
            query: userId => `/CommunityUser/searchByUserId/${userId}`,
            providesTags: (result, error, id) => [{ type: 'CommunityUser', id }]
        }),
    })
})

export const {
    useCommunityUserSearchByCommunityIdQuery,
    useLazyCommunityUserSearchByCommunityIdQuery,
    useCommunityUserSearchByUserIdQuery,
    useLazyCommunityUserSearchByUserIdQuery,
    useCreateCommunityUserMutation,
    useRemoveCommunityUserMutation
} = CommunityUserApi;