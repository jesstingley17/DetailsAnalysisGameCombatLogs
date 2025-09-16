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
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getMessagesByGroupChatId: builder.query<GroupChatMessageModel[], { chatId: number, page: number, pageSize: number }>({
            query: ({ chatId, page, pageSize }) => ({
                url: `/GroupChatMessage/getByChatId?chatId=${chatId}&page=${page}&pageSize=${pageSize}`,
            }),
            transformResponse: (response: GroupChatMessageModel[]) => response.reverse(),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'GroupChatMessage' as const, id })), { type: 'GroupChatMessage' }]
                    : [{ type: 'GroupChatMessage' }]
        }),
        getMessagesByPersonalChatId: builder.query<PersonalChatMessageModel[], { chatId: number, page: number, pageSize: number }>({
            query: ({ chatId, page, pageSize }) => ({
                url: `/PersonalChatMessage/getByChatId?chatId=${chatId}&page=${page}&pageSize=${pageSize}`,
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
    useGetMessagesByPersonalChatIdQuery,
    useLazyGetMessagesByPersonalChatIdQuery,
} = ChatApi;