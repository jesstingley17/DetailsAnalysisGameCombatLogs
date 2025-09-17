import type { GroupChatRulesModel } from '../types/GroupChatRulesModel';
import { ChatApi } from './Chat.api';

export const GroupChatRulesApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        updateGroupChatRules: builder.mutation<void, { chatId: number, groupChatRules: GroupChatRulesModel }>({
            query: ({ chatId, groupChatRules }) => ({
                body: groupChatRules,
                url: `/GroupChat/updateRules/${chatId}`,
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, args) => [{ type: 'GroupChatRules', id: args.chatId }],
        }),
        getGroupChatRulesByChatId: builder.query<GroupChatRulesModel, number>({
            query: chatId => `/GroupChat/getRules/${chatId}`,
            providesTags: result => result ? [{ type: 'GroupChatRules', id: result.id }] : [],
        }),
    })
})

export const {
    useUpdateGroupChatRulesMutation,
    useGetGroupChatRulesByChatIdQuery,
} = GroupChatRulesApi;