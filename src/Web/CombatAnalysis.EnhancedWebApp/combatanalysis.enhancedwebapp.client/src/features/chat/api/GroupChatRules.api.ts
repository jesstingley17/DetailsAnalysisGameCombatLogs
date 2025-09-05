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
            invalidatesTags: result => result ? [{ type: 'GroupChatRules', id: result.id }] : [],
        }),
        updateGroupChatRulesAsync: builder.mutation<void, GroupChatRulesModel>({
            query: groupChatRules => ({
                body: groupChatRules,
                url: '/GroupChatRules',
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, groupChatRules) => [{ type: 'GroupChatRules', id: groupChatRules.id }],
        }),
        removeGroupChatRulesAsync: builder.mutation<void, number>({
            query: id => ({
                url: `/GroupChatRules/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'GroupChatRules', id }]
        }),
        getGroupChatRulesById: builder.query<GroupChatRulesModel, number>({
            query: id => `/GroupChatRules/findByChatId/${id}`,
            providesTags: result => result ? [{ type: 'GroupChatRules', id: result.id }] : [],
        }),
    })
})

export const {
    useCreateGroupChatRulesAsyncMutation,
    useUpdateGroupChatRulesAsyncMutation,
    useRemoveGroupChatRulesAsyncMutation,
    useGetGroupChatRulesByIdQuery,
} = GroupChatRulesApi;