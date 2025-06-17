import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { GroupChatMessage } from '../../../types/GroupChatMessage';
import { PersonalChatMessage } from '../../../types/PersonalChatMessage';

const apiURL = '/api/v1';

export const ChatApi = createApi({
    reducerPath: 'chatApi',
    tagTypes: [
        'GroupChatMessage',
        'PersonalChatMessage',
        'UnreadGroupChatMessage',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getMessagesByGroupChatId: builder.query<GroupChatMessage[], { chatId: number, pageSize: number }>({
            query: ({ chatId, pageSize }) => ({
                url: `/GroupChatMessage/getByChatId?chatId=${chatId}&pageSize=${pageSize}`,
            }),
            transformResponse: (response: GroupChatMessage[]) => response.reverse(),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'GroupChatMessage' as const, id })), { type: 'GroupChatMessage' }]
                    : [{ type: 'GroupChatMessage' }]
        }),
        getMoreMessagesByGroupChatId: builder.query<GroupChatMessage[], { chatId: number, offset: number, pageSize: number }>({
            query: ({ chatId, offset, pageSize }) => ({
                url: `/GroupChatMessage/getMoreByChatId?chatId=${chatId}&offset=${offset}&pageSize=${pageSize}`,
            }),
            transformResponse: (response: GroupChatMessage[]) => response.reverse(),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'GroupChatMessage' as const, id })), { type: 'GroupChatMessage' }]
                    : [{ type: 'GroupChatMessage' }]
        }),
        getMessagesByPersonalChatId: builder.query<PersonalChatMessage[], { chatId: number, pageSize: number }>({
            query: ({ chatId, pageSize }) => ({
                url: `/PersonalChatMessage/getByChatId?chatId=${chatId}&pageSize=${pageSize}`,
            }),
            transformResponse: (response: PersonalChatMessage[]) => response.reverse(),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'PersonalChatMessage' as const, id })), { type: 'PersonalChatMessage' }]
                    : [{ type: 'PersonalChatMessage' }]
        }),
        getMoreMessagesByPersonalChatId: builder.query<PersonalChatMessage[], { chatId: number, offset: number, pageSize: number }>({
            query: ({ chatId, offset, pageSize }) => ({
                url: `/PersonalChatMessage/getMoreByChatId?chatId=${chatId}&offset=${offset}&pageSize=${pageSize}`,
            }),
            transformResponse: (response: PersonalChatMessage[]) => response.reverse(),
            providesTags: (result, error, arg) =>
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