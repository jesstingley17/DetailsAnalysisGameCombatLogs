import type { PersonalChatMessagePatch } from '../types/patches/PersonalChatMessagePatch';
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
        partialUpdatePersonalChatMessage: builder.mutation<void, { id: number, message: PersonalChatMessagePatch }>({
            query: ({ id, message }) => ({
                body: message,
                url: `/PersonalChatMessage/${id}`,
                method: 'PATCH'
            }),
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
    usePartialUpdatePersonalChatMessageMutation,
    useRemovePersonalChatMessageMutation,
    useRemovePersonalChatMessageByChatIdMutation,
    useGetPersonalChatMessageCountByChatIdQuery,
} = PersonalChatMessageApi;