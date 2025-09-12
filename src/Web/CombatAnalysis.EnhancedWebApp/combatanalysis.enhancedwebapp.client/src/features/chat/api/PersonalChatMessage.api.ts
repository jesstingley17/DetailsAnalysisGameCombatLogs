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
        updatePersonalChatMessage: builder.mutation<void, { id: number, message: PersonalChatMessageModel }>({
            query: ({ id, message }) => ({
                body: message,
                url: `/PersonalChatMessage/${id}`,
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, args) => [{ type: 'PersonalChatMessage', id: args.id }],
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