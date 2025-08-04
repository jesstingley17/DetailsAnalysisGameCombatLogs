import type { InviteToCommunityModel } from '../types/InviteToCommunityModel';
import { CommunityApi } from './Community.api';

export const InviteToCommunityApi = CommunityApi.injectEndpoints({
    endpoints: builder => ({
        createInviteAsync: builder.mutation<InviteToCommunityModel, InviteToCommunityModel>({
            query: invite => ({
                body: invite,
                url: '/InviteToCommunity',
                method: 'POST'
            }),
            invalidatesTags: (result) => [{ type: 'InviteToCommunity', result }],
        }),
        removeCommunityInvite: builder.mutation<void, number>({
            query: id => ({
                url: `/InviteToCommunity/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: result => [{ type: 'InviteToCommunity', result }],
        }),
        getInviteToCommunityById: builder.query<InviteToCommunityModel, number>({
            query: id => `/InviteToCommunity/${id}`,
            providesTags: result =>
                result
                    ? [{ type: 'InviteToCommunity', id: result.id }]
                    : [{ type: 'InviteToCommunity' }]
        }),
        inviteSearchByUserId: builder.query<InviteToCommunityModel[], number>({
            query: id => `/InviteToCommunity/searchByUserId/${id}`,
            providesTags: (result, error, id) => [{ type: 'InviteToCommunity', id }],
        }),
        inviteIsExist: builder.query<boolean, { appUserId: string, communityId: number }>({
            query: ({ appUserId, communityId }) => `/InviteToCommunity/isExist?appUserId=${appUserId}&communityId=${communityId}`,
            providesTags: (result, error, { appUserId, communityId }) => [{ type: 'InviteToCommunity', id: `${appUserId}-${communityId}` }]
        }),
    })
})

export const {
    useCreateInviteAsyncMutation,
    useRemoveCommunityInviteMutation,
    useGetInviteToCommunityByIdQuery,
    useInviteSearchByUserIdQuery,
    useLazyInviteIsExistQuery,
} = InviteToCommunityApi;