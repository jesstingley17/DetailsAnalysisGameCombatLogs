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
            invalidatesTags: result => [{ type: 'PersonalChat', result }],
        }),
        updatePersonalChatAsync: builder.mutation<void, PersonalChatModel>({
            query: personalChat => ({
                body: personalChat,
                url: '/PersonalChat',
                method: 'PUT'
            }),
            invalidatesTags: result => [{ type: 'PersonalChat', result }],
        }),
        removePersonalChatAsync: builder.mutation<void, number>({
            query: id => ({
                url: `/PersonalChat/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: result => [{ type: 'PersonalChat', result }],
        }),
        isExist: builder.query<boolean, { userId: string, targetUserId: string }>({
            query: ({ userId, targetUserId }) => `/PersonalChat/isExist?initiatorId=${userId}&companionId=${targetUserId}`,
            providesTags: (result, error, { userId, targetUserId }) => [{ type: 'PersonalChat', id: `${userId}-${targetUserId}` }],
        }),
        getPersonalChatById: builder.query<PersonalChatModel, number>({
            query: id => `/PersonalChat/${id}`,
            providesTags: (result, error, id) => [{ type: 'PersonalChat', id }],
        }),
        getPersonalChatsByUserId: builder.query<PersonalChatModel[], string>({
            query: userId => `/PersonalChat/getByUserId/${userId}`,
            providesTags: (result, error, id) => [{ type: 'PersonalChat', id }],
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