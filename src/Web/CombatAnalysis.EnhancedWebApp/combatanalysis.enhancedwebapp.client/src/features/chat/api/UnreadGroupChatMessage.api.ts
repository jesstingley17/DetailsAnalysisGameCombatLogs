/* eslint-disable @typescript-eslint/no-unused-vars */
import { type UnreadGroupChatMessage } from '../../../types/UnreadGroupChatMessage';
import { ChatApi } from '../core/Chat.api';

export const UnreadGroupChatMessageApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        findUnreadGroupChatMessage: builder.query<UnreadGroupChatMessage, { messageId: number, groupChatUserId : string}>({
            query: ({ messageId, groupChatUserId }) => `/UnreadGroupChatMessage/find?messageId=${messageId}&groupChatUserId=${groupChatUserId}`,
        }),
        findUnreadGroupChatMessagesByMessageId: builder.query<UnreadGroupChatMessage[], number>({
            query: (id) => `/UnreadGroupChatMessage/findByMessageId/${id}`,
            providesTags: (result, error, id) => [{ type: 'UnreadGroupChatMessage', id}],
        }),
    })
})

export const {
    useFindUnreadGroupChatMessageQuery,
    useLazyFindUnreadGroupChatMessageQuery,
    useFindUnreadGroupChatMessagesByMessageIdQuery,
    useLazyFindUnreadGroupChatMessagesByMessageIdQuery,
} = UnreadGroupChatMessageApi;