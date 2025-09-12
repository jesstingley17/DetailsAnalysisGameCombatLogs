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
        updateGroupChatRulesAsync: builder.mutation<void, { id: number, groupChatRules: GroupChatRulesModel }>({
            query: ({ id, groupChatRules }) => ({
                body: groupChatRules,
                url: `/GroupChatRules/${id}`,
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, args) => [{ type: 'GroupChatRules', id: args.id }],
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