/* eslint-disable @typescript-eslint/no-unused-vars */
import type { GroupChatUser } from '../../../types/GroupChatUser';
import { ChatApi } from '../core/Chat.api';

export const GroupChatUserApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChatUserAsync: builder.mutation<GroupChatUser, GroupChatUser>({
            query: groupChatUser => ({
                body: groupChatUser,
                url: '/GroupChatUser',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatUser', result }]
        }),
        removeGroupChatUserAsync: builder.mutation<void, number>({
            query: id => ({
                url: `/GroupChatUser/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatUser', result }],
        }),
        getGroupChatUserById: builder.query<GroupChatUser, number>({
            query: id => `/GroupChatUser/${id}`,
            providesTags: (result, error, id) => [{ type: 'GroupChatUser', id }],
        }),
        findMeInChat: builder.query<GroupChatUser, { chatId: number, appUserId: string }>({
            query: ({ chatId, appUserId }) => `/GroupChatUser/findMeInChat?chatId=${chatId}&appUserId=${appUserId}`,
            providesTags: (result, error, { chatId, appUserId }) => [{ type: 'GroupChatUser', id: `${chatId}-${appUserId}` }],
        }),
        findGroupChatUsersByUserId: builder.query<GroupChatUser[], string>({
            query: userId => `/GroupChatUser/findByUserId/${userId}`,
            providesTags: (result, error, id) => [{ type: 'GroupChatUser', id }],
        }),
        findGroupChatUsersByChatId: builder.query<GroupChatUser[], number>({
            query: chatId => `/GroupChatUser/findByChatId/${chatId}`,
            providesTags: (result, error, id) => [{ type: 'GroupChatUser', id }],
        }),
    })
})

export const {
    useCreateGroupChatUserAsyncMutation,
    useRemoveGroupChatUserAsyncMutation,
    useGetGroupChatUserByIdQuery,
    useFindMeInChatQuery,
    useFindGroupChatUsersByUserIdQuery,
    useLazyFindGroupChatUsersByUserIdQuery,
    useFindGroupChatUsersByChatIdQuery,
} = GroupChatUserApi;