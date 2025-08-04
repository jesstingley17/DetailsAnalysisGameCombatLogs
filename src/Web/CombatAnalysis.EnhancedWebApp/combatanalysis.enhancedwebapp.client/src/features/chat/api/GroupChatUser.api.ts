import type { GroupChatModel } from '../types/GroupChatUserModel';
import { ChatApi } from './Chat.api';

export const GroupChatUserApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChatUserAsync: builder.mutation<GroupChatModel, GroupChatModel>({
            query: groupChatUser => ({
                body: groupChatUser,
                url: '/GroupChatUser',
                method: 'POST'
            }),
            invalidatesTags: result => [{ type: 'GroupChatUser', result }]
        }),
        removeGroupChatUserAsync: builder.mutation<void, number>({
            query: id => ({
                url: `/GroupChatUser/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: result => [{ type: 'GroupChatUser', result }],
        }),
        getGroupChatUserById: builder.query<GroupChatModel, number>({
            query: id => `/GroupChatUser/${id}`,
            providesTags: (result, error, id) => [{ type: 'GroupChatUser', id }],
        }),
        findMeInChat: builder.query<GroupChatModel, { chatId: number, appUserId: string }>({
            query: ({ chatId, appUserId }) => `/GroupChatUser/findMeInChat?chatId=${chatId}&appUserId=${appUserId}`,
            providesTags: (result, error, { chatId, appUserId }) => [{ type: 'GroupChatUser', id: `${chatId}-${appUserId}` }],
        }),
        findGroupChatUsersByUserId: builder.query<GroupChatModel[], string>({
            query: userId => `/GroupChatUser/findByUserId/${userId}`,
            providesTags: (result, error, id) => [{ type: 'GroupChatUser', id }],
        }),
        findGroupChatUsersByChatId: builder.query<GroupChatModel[], number>({
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