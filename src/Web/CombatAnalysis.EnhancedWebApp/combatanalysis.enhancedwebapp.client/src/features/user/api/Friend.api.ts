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
            invalidatesTags: (result, error, arg) => [{ type: 'Friend', arg }]
        }),
        removeFriendAsync: builder.mutation<void, number>({
            query: id => ({
                url: `/Friend/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result) => [{ type: 'Friend', result }],
        }),
        friendSearchMyFriends: builder.query<FriendModel[], string>({
            query: (currentUserId) => `/Friend/searchMyFriends/${currentUserId}`,
            providesTags: (result, error, id) => [{ type: 'Friend', id }],
        }),
    })
})

export const {
    useCreateFriendAsyncMutation,
    useRemoveFriendAsyncMutation,
    useFriendSearchMyFriendsQuery
} = FriendApi;