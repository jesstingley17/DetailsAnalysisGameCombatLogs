import { Friend } from "../../../types/Friend";
import { UserApi } from "../core/User.api";

export const FriendApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        createFriendAsync: builder.mutation<Friend, Friend>({
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
            invalidatesTags: (result, error) => [{ type: 'Friend', result }],
        }),
        friendSearchMyFriends: builder.query<Friend[], string>({
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