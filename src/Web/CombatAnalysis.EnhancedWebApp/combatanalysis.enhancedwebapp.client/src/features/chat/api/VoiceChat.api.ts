import type { VoiceChat } from '../../../types/components/communication/chats/VoiceChat';
import { ChatApi } from '../core/Chat.api';

export const VoiceChatApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createCall: builder.mutation<VoiceChat, VoiceChat>({
            query: groupChat => ({
                body: groupChat,
                url: '/VoiceChat',
                method: 'POST'
            }),
        }),
        removeCall: builder.mutation<void, number>({
            query: id => ({
                url: `/VoiceChat/${id}`,
                method: 'DELETE'
            }),
        }),
        getCalls: builder.query<VoiceChat[], void>({
            query: () => `/VoiceChat`,
        }),
        getCallById: builder.query<VoiceChat, string>({
            query: id => `/VoiceChat/${id}`,
        }),
    })
})

export const {
    useCreateCallMutation,
    useRemoveCallMutation,
    useGetCallsQuery,
    useGetCallByIdQuery,
} = VoiceChatApi;