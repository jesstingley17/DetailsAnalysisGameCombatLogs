import { UnreadGroupChatMessage } from "../../../types/UnreadGroupChatMessage";
import { ChatApi } from "../core/Chat.api";

export const UnreadGroupChatMessageApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createUnreadGroupChatMessageAsync: builder.mutation<UnreadGroupChatMessage, UnreadGroupChatMessage>({
            query: unreadMessage => ({
                body: unreadMessage,
                url: '/UnreadGroupChatMessage',
                method: 'POST'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'UnreadGroupChatMessage', arg }],
        }),
        updateUnreadGroupChatMessageAsync: builder.mutation<number, UnreadGroupChatMessage>({
            query: unreadMessage => ({
                body: unreadMessage,
                url: '/UnreadGroupChatMessage',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'UnreadGroupChatMessage', result }],
        }),
        removeUnreadGroupChatMessageAsync: builder.mutation<number, number>({
            query: id => ({
                url: `/UnreadGroupChatMessage/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'UnreadGroupChatMessage', result }],
        }),
        findUnreadGroupChatMessage: builder.query<UnreadGroupChatMessage, { messageId: number, groupChatUserId : string}>({
            query: ({ messageId, groupChatUserId }) => `/UnreadGroupChatMessage/find?messageId=${messageId}&groupChatUserId=${groupChatUserId}`,
        }),
        findUnreadGroupChatMessageByMessageId: builder.query<UnreadGroupChatMessage[], number>({
            query: (id) => `/UnreadGroupChatMessage/findByMessageId/${id}`,
            providesTags: (result, error, id) => [{ type: 'UnreadGroupChatMessage', id}],
        }),
    })
})

export const {
    useCreateUnreadGroupChatMessageAsyncMutation,
    useUpdateUnreadGroupChatMessageAsyncMutation,
    useRemoveUnreadGroupChatMessageAsyncMutation,
    useFindUnreadGroupChatMessageQuery,
    useLazyFindUnreadGroupChatMessageQuery,
    useFindUnreadGroupChatMessageByMessageIdQuery,
    useLazyFindUnreadGroupChatMessageByMessageIdQuery,
} = UnreadGroupChatMessageApi;