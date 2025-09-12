import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import { ChatApi } from './Chat.api';

export const GroupChatMessageApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChatMessage: builder.mutation<GroupChatMessageModel, GroupChatMessageModel>({
            query: message => ({
                body: message,
                url: '/GroupChatMessage',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'GroupChatMessage', id: result.id }] : [],
        }),
        updateGroupChatMessage: builder.mutation<void, { id: number, message: GroupChatMessageModel }>({
            query: ({ id, message }) => ({
                body: message,
                url: `/GroupChatMessage/${id}`,
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, args) => [{ type: 'GroupChatMessage', id: args.id }],
        }),
        removeGroupChatMessage: builder.mutation<void, number>({
            query: id => ({
                url: `/GroupChatMessage/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'GroupChatMessage', id }],
        }),
        removeGroupChatMessageByChatId: builder.mutation<void, number>({
            query: id => ({
                url: `/GroupChatMessage/deleteByChatId/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'GroupChatMessage', id }],
        }),
        getGroupChatMessageCountByChatId: builder.query<number, number>({
            query: chatId => `/GroupChatMessage/count/${chatId}`,
        }),
    })
})

export const {
    useCreateGroupChatMessageMutation,
    useUpdateGroupChatMessageMutation,
    useRemoveGroupChatMessageMutation,
    useRemoveGroupChatMessageByChatIdMutation,
    useGetGroupChatMessageCountByChatIdQuery,
} = GroupChatMessageApi;