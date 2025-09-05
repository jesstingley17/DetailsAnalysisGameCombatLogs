import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';
import { ChatApi } from './Chat.api';

export const PersonalChatMessageApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createPersonalChatMessage: builder.mutation<PersonalChatMessageModel, PersonalChatMessageModel>({
            query: personalMessage => ({
                body: personalMessage,
                url: '/PersonalChatMessage',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'PersonalChatMessage', id: result.id }] : [],
        }),
        updatePersonalChatMessage: builder.mutation<void, PersonalChatMessageModel>({
            query: message => ({
                body: message,
                url: '/PersonalChatMessage',
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, message) => [{ type: 'PersonalChatMessage', id: message.id }],
        }),
        removePersonalChatMessage: builder.mutation<void, number>({
            query: id => ({
                url: `/PersonalChatMessage/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'PersonalChatMessage', id }],
        }),
        removePersonalChatMessageByChatId: builder.mutation<void, number>({
            query: id => ({
                url: `/PersonalChatMessage/deleteByChatId/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'PersonalChatMessage', id }],
        }),
        getPersonalChatMessageCountByChatId: builder.query<number, number>({
            query: chatId => `/PersonalChatMessage/count/${chatId}`,
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