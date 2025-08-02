import { type GroupChatModel } from '../types/GroupChatModel';
import { ChatApi } from './Chat.api';

export const GroupChatApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChat: builder.mutation<GroupChatModel, GroupChatModel>({
            query: groupChat => ({
                body: groupChat,
                url: '/GroupChat',
                method: 'POST'
            }),
            invalidatesTags: (result) => [{ type: 'GroupChat', result }],
        }),
        updateGroupChatAsync: builder.mutation<number, GroupChatModel>({
            query: groupChat => ({
                body: groupChat,
                url: '/GroupChat',
                method: 'PUT'
            }),
            invalidatesTags: (result) => [{ type: 'GroupChat', result }],
        }),
        removeGroupChatAsync: builder.mutation<number, number>({
            query: id => ({
                url: `/GroupChat/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'GroupChat', arg }]
        }),
        getGroupChatById: builder.query<GroupChatModel, number>({
            query: (id) => `/GroupChat/${id}`,
            providesTags: (result, error, id) => [{ type: 'GroupChat', id }],
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