import type { GroupChatUserModel } from '../types/GroupChatUserModel';
import { ChatApi } from './Chat.api';

export const GroupChatUserApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChatUserAsync: builder.mutation<GroupChatUserModel, GroupChatUserModel>({
            query: groupChatUser => ({
                body: groupChatUser,
                url: '/GroupChatUser',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'GroupChatUser', id: result.id }] : [],
        }),
        removeGroupChatUserAsync: builder.mutation<void, string>({
            query: id => ({
                url: `/GroupChatUser/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'GroupChatUser', id }],
        }),
        getGroupChatUserById: builder.query<GroupChatUserModel, string>({
            query: id => `/GroupChatUser/${id}`,
            providesTags: result => result ? [{ type: 'GroupChatUser', id: result.id }] : [],
        }),
        findGroupChatUserByAppUserId: builder.query<GroupChatUserModel, { chatId: number, appUserId: string }>({
            query: ({ chatId, appUserId }) => `/GroupChatUser/findByAppUserId?chatId=${chatId}&appUserId=${appUserId}`,
        }),
        findGroupChatUsersByAppUserId: builder.query<GroupChatUserModel[], string>({
            query: appUserId => `/GroupChatUser/findAllByAppUserId/${appUserId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(chatUser => ({ type: 'GroupChatUser' as const, id: chatUser.id })),
                        { type: 'GroupChatUser', id: 'LIST' },
                    ]
                    : [{ type: 'GroupChatUser', id: 'LIST' }],
        }),
        findGroupChatUsersByChatId: builder.query<GroupChatUserModel[], number>({
            query: chatId => `/GroupChatUser/findAll/${chatId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(chatUser => ({ type: 'GroupChatUser' as const, id: chatUser.id })),
                        { type: 'GroupChatUser', id: 'LIST' },
                    ]
                    : [{ type: 'GroupChatUser', id: 'LIST' }],
        }),
    })
})

export const {
    useCreateGroupChatUserAsyncMutation,
    useRemoveGroupChatUserAsyncMutation,
    useGetGroupChatUserByIdQuery,
    useFindGroupChatUserByAppUserIdQuery,
    useFindGroupChatUsersByAppUserIdQuery,
    useLazyFindGroupChatUsersByAppUserIdQuery,
    useFindGroupChatUsersByChatIdQuery,
} = GroupChatUserApi;