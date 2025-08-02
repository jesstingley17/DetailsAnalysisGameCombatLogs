import { type RequestToConnectModel } from '../types/RequestToConnectModel';
import { UserApi } from './User.api';

export const RequestToConnectApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        createRequestAsync: builder.mutation<RequestToConnectModel, RequestToConnectModel>({
            query: request => ({
                body: request,
                url: '/RequestToConnect',
                method: 'POST'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'RequestToConnect', arg }]
        }),
        removeRequestAsync: builder.mutation<number, number>({
            query: id => ({
                url: `/RequestToConnect/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'RequestToConnect', arg }]
        }),
        requestIsExist: builder.query<boolean, { userId: string, targetUserId: string }>({
            query: ({ userId, targetUserId }) => ({
                url: `/RequestToConnect/isExist?initiatorId=${userId}&companionId=${targetUserId}`,
            }),
            providesTags: (result, error, { userId, targetUserId }) => [{ type: 'RequestToConnect', id: `${userId}-${targetUserId}` }]
        }),
        searchByOwnerId: builder.query<RequestToConnectModel, string>({
            query: userId => `/RequestToConnect/searchByOwnerId/${userId}`,
            providesTags: (result, error, id) => [{ type: 'RequestToConnect', id }]
        }),
        searchByToUserId: builder.query<RequestToConnectModel, string>({
            query: userId => `/RequestToConnect/searchByToUserId/${userId}`,
            providesTags: (result, error, id) => [{ type: 'RequestToConnect', id }],
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