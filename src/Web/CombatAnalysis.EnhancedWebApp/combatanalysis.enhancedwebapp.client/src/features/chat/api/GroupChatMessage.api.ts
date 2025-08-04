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
            invalidatesTags: result => [{ type: 'GroupChatMessage', result }],
        }),
        updateGroupChatMessage: builder.mutation<void, GroupChatMessageModel>({
            query: message => ({
                body: message,
                url: '/GroupChatMessage',
                method: 'PUT'
            }),
            invalidatesTags: result => [{ type: 'GroupChatMessage', result }],
        }),
        removeGroupChatMessage: builder.mutation<void, number>({
            query: id => ({
                url: `/GroupChatMessage/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: result => [{ type: 'GroupChatMessage', result }],
        }),
        removeGroupChatMessageByChatId: builder.mutation<void, number>({
            query: id => ({
                url: `/GroupChatMessage/deleteByChatId/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: result => [{ type: 'GroupChatMessage', result }],
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