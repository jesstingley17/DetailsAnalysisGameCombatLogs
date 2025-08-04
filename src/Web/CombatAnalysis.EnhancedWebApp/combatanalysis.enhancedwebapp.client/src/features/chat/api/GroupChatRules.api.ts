import type { GroupChatRulesModel } from '../types/GroupChatRulesModel';
import { ChatApi } from './Chat.api';

export const GroupChatRulesApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChatRulesAsync: builder.mutation<GroupChatRulesModel, GroupChatRulesModel>({
            query: groupChatRules => ({
                body: groupChatRules,
                url: '/GroupChatRules',
                method: 'POST'
            }),
            invalidatesTags: result => [{ type: 'GroupChatRules', result }],
        }),
        updateGroupChatRulesAsync: builder.mutation<void, GroupChatRulesModel>({
            query: groupChatRules => ({
                body: groupChatRules,
                url: '/GroupChatRules',
                method: 'PUT'
            }),
            invalidatesTags: result => [{ type: 'GroupChatRules', result }],
        }),
        removeGroupChatRulesAsync: builder.mutation<void, number>({
            query: id => ({
                url: `/GroupChatRules/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'GroupChatRules', arg }]
        }),
        getGroupChatRulesById: builder.query<GroupChatRulesModel, number>({
            query: id => `/GroupChatRules/findByChatId/${id}`,
            providesTags: (result, error, id) => [{ type: 'GroupChatRules', id }],
        }),
    })
})

export const {
    useCreateGroupChatRulesAsyncMutation,
    useUpdateGroupChatRulesAsyncMutation,
    useRemoveGroupChatRulesAsyncMutation,
    useGetGroupChatRulesByIdQuery,
} = GroupChatRulesApi;