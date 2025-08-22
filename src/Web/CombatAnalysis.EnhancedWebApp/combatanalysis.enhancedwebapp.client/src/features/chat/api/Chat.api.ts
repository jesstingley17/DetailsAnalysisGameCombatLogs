import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';

const apiURL = '/api/v1';

export const ChatApi = createApi({
    reducerPath: 'chatApi',
    tagTypes: [
        'PersonalChat',
        'PersonalChatMessage',
        'GroupChat',
        'GroupChatMessage',
        'UnreadGroupChatMessage',
        'GroupChatUser',
        'GroupChatRules',
        'VoiceChat',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getMessagesByGroupChatId: builder.query<GroupChatMessageModel[], { chatId: number, groupChatUserId: string, pageSize: number }>({
            query: ({ chatId, groupChatUserId, pageSize }) => ({
                url: `/GroupChatMessage/getByChatId?chatId=${chatId}&groupChatUserId=${groupChatUserId}&pageSize=${pageSize}`,
            }),
            transformResponse: (response: GroupChatMessageModel[]) => response.reverse(),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'GroupChatMessage' as const, id })), { type: 'GroupChatMessage' }]
                    : [{ type: 'GroupChatMessage' }]
        }),
        getMoreMessagesByGroupChatId: builder.query<GroupChatMessageModel[], { chatId: number, groupChatUserId: string, offset: number, pageSize: number }>({
            query: ({ chatId, groupChatUserId, offset, pageSize }) => ({
                url: `/GroupChatMessage/getMoreByChatId?chatId=${chatId}&groupChatUserId=${groupChatUserId}&offset=${offset}&pageSize=${pageSize}`,
            }),
            transformResponse: (response: GroupChatMessageModel[]) => response.reverse(),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'GroupChatMessage' as const, id })), { type: 'GroupChatMessage' }]
                    : [{ type: 'GroupChatMessage' }]
        }),
        getMessagesByPersonalChatId: builder.query<PersonalChatMessageModel[], { chatId: number, pageSize: number }>({
            query: ({ chatId, pageSize }) => ({
                url: `/PersonalChatMessage/getByChatId?chatId=${chatId}&pageSize=${pageSize}`,
            }),
            transformResponse: (response: PersonalChatMessageModel[]) => response.reverse(),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'PersonalChatMessage' as const, id })), { type: 'PersonalChatMessage' }]
                    : [{ type: 'PersonalChatMessage' }]
        }),
        getMoreMessagesByPersonalChatId: builder.query<PersonalChatMessageModel[], { chatId: number, offset: number, pageSize: number }>({
            query: ({ chatId, offset, pageSize }) => ({
                url: `/PersonalChatMessage/getMoreByChatId?chatId=${chatId}&offset=${offset}&pageSize=${pageSize}`,
            }),
            transformResponse: (response: PersonalChatMessageModel[]) => response.reverse(),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'PersonalChatMessage' as const, id })), { type: 'PersonalChatMessage' }]
                    : [{ type: 'PersonalChatMessage' }]
        }),
    })
})

export const {
    useGetMessagesByGroupChatIdQuery,
    useLazyGetMessagesByGroupChatIdQuery,
    useGetMoreMessagesByGroupChatIdQuery,
    useLazyGetMoreMessagesByGroupChatIdQuery,
    useGetMessagesByPersonalChatIdQuery,
    useLazyGetMessagesByPersonalChatIdQuery,
    useGetMoreMessagesByPersonalChatIdQuery,
    useLazyGetMoreMessagesByPersonalChatIdQuery,
} = ChatApi;