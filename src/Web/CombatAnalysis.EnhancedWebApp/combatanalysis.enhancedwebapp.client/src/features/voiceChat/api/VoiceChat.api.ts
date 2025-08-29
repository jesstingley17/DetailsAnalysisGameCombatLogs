import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { VoiceChatModel } from '../types/VoiceChatModel';

const apiURL = '/api/v1';

export const VoiceChatApi = createApi({
    reducerPath: 'voidChatApi',
    tagTypes: [
        'VoiceChat',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        createCall: builder.mutation<VoiceChatModel, VoiceChatModel>({
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
        getCalls: builder.query<VoiceChatModel[], void>({
            query: () => `/VoiceChat`,
        }),
        getCallById: builder.query<VoiceChatModel, string>({
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