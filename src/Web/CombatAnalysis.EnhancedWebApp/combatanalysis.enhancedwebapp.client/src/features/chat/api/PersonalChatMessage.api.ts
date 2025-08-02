/* eslint-disable @typescript-eslint/no-unused-vars */
import type { PersonalChatMessage } from '../../../types/PersonalChatMessage';
import { ChatApi } from '../core/Chat.api';

export const PersonalChatMessageApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createPersonalChatMessage: builder.mutation<PersonalChatMessage, PersonalChatMessage>({
            query: personalMessage => ({
                body: personalMessage,
                url: '/PersonalChatMessage',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChatMessage', result }],
        }),
        updatePersonalChatMessage: builder.mutation<void, PersonalChatMessage>({
            query: message => ({
                body: message,
                url: '/PersonalChatMessage',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChatMessage', result }],
        }),
        removePersonalChatMessage: builder.mutation<void, number>({
            query: id => ({
                url: `/PersonalChatMessage/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChatMessage', result }],
        }),
        removePersonalChatMessageByChatId: builder.mutation<void, number>({
            query: id => ({
                url: `/PersonalChatMessage/deleteByChatId/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChatMessage', result }],
        }),
        getPersonalChatMessageCountByChatId: builder.query<number, number>({
            query: chatId => `/PersonalChatMessage/count/${chatId}`
        }),
    })
})

export const {
    useCreatePersonalChatMessageMutation,
    useUpdatePersonalChatMessageMutation,
    useRemovePersonalChatMessageMutation,
    useRemovePersonalChatMessageByChatIdMutation,
    useGetPersonalChatMessageCountByChatIdQuery,
} = PersonalChatMessageApi;