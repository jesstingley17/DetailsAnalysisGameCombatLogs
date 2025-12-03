import type { FriendModel } from '../types/FriendModel';
import { UserApi } from './User.api';

export const FriendApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        createFriendAsync: builder.mutation<FriendModel, FriendModel>({
            query: friend => ({
                body: friend,
                url: '/Friend',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'Friend', id: result.id }] : [],
        }),
        removeFriendAsync: builder.mutation<void, number>({
            query: id => ({
                url: `/Friend/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'Friend', id }]
        }),
        findFriendByUserId: builder.query<FriendModel[], string>({
            query: currentUserId => `/Friend/findByUserId/${currentUserId}`,
            providesTags: (_result, _error, currentUserId) => [{ type: 'Friend', id: currentUserId }],
        }),
    })
})

export const {
    useCreateFriendAsyncMutation,
    useRemoveFriendAsyncMutation,
    useFindFriendByUserIdQuery
} = FriendApi;