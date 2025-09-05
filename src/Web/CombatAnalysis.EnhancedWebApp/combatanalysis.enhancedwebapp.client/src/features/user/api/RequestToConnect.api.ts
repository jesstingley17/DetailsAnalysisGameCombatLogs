import type { RequestToConnectModel } from '../types/RequestToConnectModel';
import { UserApi } from './User.api';

export const RequestToConnectApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        createRequestAsync: builder.mutation<RequestToConnectModel, RequestToConnectModel>({
            query: request => ({
                body: request,
                url: '/RequestToConnect',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'RequestToConnect', id: result.id }] : []
        }),
        removeRequestAsync: builder.mutation<number, number>({
            query: id => ({
                url: `/RequestToConnect/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'RequestToConnect', id }]
        }),
        requestIsExist: builder.query<boolean, { userId: string, targetUserId: string }>({
            query: ({ userId, targetUserId }) => `/RequestToConnect/isExist?initiatorId=${userId}&companionId=${targetUserId}`,
            providesTags: (_result, _error, { userId, targetUserId }) => [{ type: 'RequestToConnect', id: `${userId}-${targetUserId}` }]
        }),
        searchByOwnerId: builder.query<RequestToConnectModel[], string>({
            query: userId => `/RequestToConnect/searchByOwnerId/${userId}`,
            providesTags: (_result, _error, userId) => [{ type: 'RequestToConnect', id: userId }]
        }),
        searchByToUserId: builder.query<RequestToConnectModel[], string>({
            query: userId => `/RequestToConnect/searchByToUserId/${userId}`,
            providesTags: (_result, _error, userId) => [{ type: 'RequestToConnect', id: userId }],
        }),
    })
})

export const {
    useCreateRequestAsyncMutation,
    useRemoveRequestAsyncMutation,
    useRequestIsExistQuery,
    useLazyRequestIsExistQuery,
    useSearchByOwnerIdQuery,
    useLazySearchByOwnerIdQuery,
    useSearchByToUserIdQuery,
} = RequestToConnectApi;