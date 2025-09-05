import type { GroupChatModel } from '../types/GroupChatModel';
import { ChatApi } from './Chat.api';

export const GroupChatApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChat: builder.mutation<GroupChatModel, GroupChatModel>({
            query: groupChat => ({
                body: groupChat,
                url: '/GroupChat',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'GroupChat', id: result.id }] : [],
        }),
        updateGroupChatAsync: builder.mutation<void, GroupChatModel>({
            query: groupChat => ({
                body: groupChat,
                url: '/GroupChat',
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, groupChat) => [{ type: 'GroupChat', id: groupChat.id }],
        }),
        removeGroupChatAsync: builder.mutation<void, number>({
            query: id => ({
                url: `/GroupChat/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'GroupChat', id }]
        }),
        getGroupChatById: builder.query<GroupChatModel, number>({
            query: id => `/GroupChat/${id}`,
            providesTags: result => result ? [{ type: 'GroupChat', id: result.id }] : [],
        }),
    })
})

export const {
    useCreateGroupChatMutation,
    useUpdateGroupChatAsyncMutation,
    useRemoveGroupChatAsyncMutation,
    useGetGroupChatByIdQuery,
    useLazyGetGroupChatByIdQuery,
} = GroupChatApi;