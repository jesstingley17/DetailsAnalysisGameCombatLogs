import type { UnreadGroupChatMessageModel } from '../types/UnreadGroupChatMessageModel';
import { ChatApi } from './Chat.api';

export const UnreadGroupChatMessageApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        findUnreadGroupChatMessage: builder.query<UnreadGroupChatMessageModel, { messageId: number, groupChatUserId : string}>({
            query: ({ messageId, groupChatUserId }) => `/UnreadGroupChatMessage/find?messageId=${messageId}&groupChatUserId=${groupChatUserId}`,
        }),
        findUnreadGroupChatMessagesByMessageId: builder.query<UnreadGroupChatMessageModel[], number>({
            query: id => `/UnreadGroupChatMessage/findByMessageId/${id}`,
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