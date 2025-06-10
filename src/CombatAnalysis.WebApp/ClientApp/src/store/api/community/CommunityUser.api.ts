import { CommunityUser } from '../../../types/CommunityUser';
import { CommunityApi } from '../core/Community.api';

export const CommunityUserApi = CommunityApi.injectEndpoints({
    endpoints: builder => ({
        communityUserSearchByCommunityId: builder.query<CommunityUser[], number>({
            query: (communityId) => `/CommunityUser/searchByCommunityId/${communityId}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityUser' as const, id })), { type: 'CommunityUser' }]
                    : [{ type: 'CommunityUser' }]
        }),
        communityUserSearchByUserId: builder.query<CommunityUser[], string>({
            query: (userId) => `/CommunityUser/searchByUserId/${userId}`,
            providesTags: (result, error, id) => [{ type: 'CommunityUser', id }]
        }),
        createCommunityUser: builder.mutation<CommunityUser, CommunityUser>({
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
        })
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