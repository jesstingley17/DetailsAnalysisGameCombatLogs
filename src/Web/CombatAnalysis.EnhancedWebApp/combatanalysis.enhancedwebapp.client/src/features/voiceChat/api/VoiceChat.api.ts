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
            invalidatesTags: result => result ? [{ type: 'VoiceChat', id: result.id }] : [],
        }),
        removeCall: builder.mutation<void, number>({
            query: id => ({
                url: `/VoiceChat/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'VoiceChat', id }]
        }),
        getCalls: builder.query<VoiceChatModel[], void>({
            query: () => `/VoiceChat`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(voiceChat => ({ type: 'VoiceChat' as const, id: voiceChat.id })),
                        { type: 'VoiceChat', id: 'LIST' },
                    ]
                    : [{ type: 'VoiceChat', id: 'LIST' }]
        }),
        getCallById: builder.query<VoiceChatModel, string>({
            query: id => `/VoiceChat/${id}`,
            providesTags: result => result ? [{ type: 'VoiceChat', id: result.id }] : []
        }),
    })
})

export const {
    useCreateCallMutation,
    useRemoveCallMutation,
    useGetCallsQuery,
    useGetCallByIdQuery,
} = VoiceChatApi;