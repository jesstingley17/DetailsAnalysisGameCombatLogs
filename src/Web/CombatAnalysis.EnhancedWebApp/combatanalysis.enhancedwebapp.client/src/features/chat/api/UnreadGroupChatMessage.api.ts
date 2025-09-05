import type { UnreadGroupChatMessageModel } from '../types/UnreadGroupChatMessageModel';
import { ChatApi } from './Chat.api';

export const UnreadGroupChatMessageApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        findUnreadGroupChatMessage: builder.query<UnreadGroupChatMessageModel, { messageId: number, groupChatUserId : string}>({
            query: ({ messageId, groupChatUserId }) => `/UnreadGroupChatMessage/find?messageId=${messageId}&groupChatUserId=${groupChatUserId}`,
            providesTags: result => result ? [{ type: 'UnreadGroupChatMessage', id: result.id }] : [],
        }),
        findUnreadGroupChatMessagesByMessageId: builder.query<UnreadGroupChatMessageModel[], number>({
            query: id => `/UnreadGroupChatMessage/findByMessageId/${id}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(unreadMessage => ({ type: 'UnreadGroupChatMessage' as const, id: unreadMessage.id })),
                        { type: 'UnreadGroupChatMessage', id: 'LIST' },
                    ]
                    : [{ type: 'UnreadGroupChatMessage', id: 'LIST' }]
        }),
    })
})

export const {
    useFindUnreadGroupChatMessageQuery,
    useLazyFindUnreadGroupChatMessageQuery,
    useFindUnreadGroupChatMessagesByMessageIdQuery,
    useLazyFindUnreadGroupChatMessagesByMessageIdQuery,
} = UnreadGroupChatMessageApi;