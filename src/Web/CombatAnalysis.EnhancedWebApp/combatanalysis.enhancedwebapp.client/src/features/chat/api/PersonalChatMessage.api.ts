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
            invalidatesTags: result => [{ type: 'PersonalChatMessage', result }],
        }),
        updatePersonalChatMessage: builder.mutation<void, PersonalChatMessageModel>({
            query: message => ({
                body: message,
                url: '/PersonalChatMessage',
                method: 'PUT'
            }),
            invalidatesTags: result => [{ type: 'PersonalChatMessage', result }],
        }),
        removePersonalChatMessage: builder.mutation<void, number>({
            query: id => ({
                url: `/PersonalChatMessage/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: result => [{ type: 'PersonalChatMessage', result }],
        }),
        removePersonalChatMessageByChatId: builder.mutation<void, number>({
            query: id => ({
                url: `/PersonalChatMessage/deleteByChatId/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: result => [{ type: 'PersonalChatMessage', result }],
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