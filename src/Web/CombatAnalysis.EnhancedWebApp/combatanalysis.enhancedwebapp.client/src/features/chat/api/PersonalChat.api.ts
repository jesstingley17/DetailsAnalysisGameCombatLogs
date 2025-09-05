import type { PersonalChatModel } from '../types/PersonalChatModel';
import { ChatApi } from './Chat.api';

export const PersonalChatApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createPersonalChatAsync: builder.mutation<PersonalChatModel, PersonalChatModel>({
            query: personalChat => ({
                body: personalChat,
                url: '/PersonalChat',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'PersonalChat', id: result.id }] : [],
        }),
        updatePersonalChatAsync: builder.mutation<void, PersonalChatModel>({
            query: personalChat => ({
                body: personalChat,
                url: '/PersonalChat',
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, personalChat) => [{ type: 'PersonalChat', id: personalChat.id }],
        }),
        removePersonalChatAsync: builder.mutation<void, number>({
            query: id => ({
                url: `/PersonalChat/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'PersonalChat', id }],
        }),
        isExist: builder.query<boolean, { userId: string, targetUserId: string }>({
            query: ({ userId, targetUserId }) => `/PersonalChat/isExist?initiatorId=${userId}&companionId=${targetUserId}`,
            providesTags: (_result, _error, { userId, targetUserId }) => [{ type: 'PersonalChat', id: `${userId}-${targetUserId}` }],
        }),
        getPersonalChatById: builder.query<PersonalChatModel, number>({
            query: id => `/PersonalChat/${id}`,
            providesTags: result => result ? [{ type: 'PersonalChat', id: result.id }] : [],
        }),
        getPersonalChatsByUserId: builder.query<PersonalChatModel[], string>({
            query: userId => `/PersonalChat/getByUserId/${userId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(chat => ({ type: 'PersonalChat' as const, id: chat.id })),
                        { type: 'PersonalChat', id: 'LIST' },
                    ]
                    : [{ type: 'PersonalChat', id: 'LIST' }]
        }),
    })
})

export const {
    useCreatePersonalChatAsyncMutation,
    useUpdatePersonalChatAsyncMutation,
    useRemovePersonalChatAsyncMutation,
    useLazyIsExistQuery,
    useLazyGetPersonalChatByIdQuery,
    useGetPersonalChatsByUserIdQuery,
    useLazyGetPersonalChatsByUserIdQuery,
} = PersonalChatApi;