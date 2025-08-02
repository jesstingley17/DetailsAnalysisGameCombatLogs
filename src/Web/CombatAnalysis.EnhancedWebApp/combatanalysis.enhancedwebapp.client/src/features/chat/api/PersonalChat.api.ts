/* eslint-disable @typescript-eslint/no-unused-vars */
import type { PersonalChatType } from '../../../types/PersonalChatType';
import { ChatApi } from '../core/Chat.api';

export const PersonalChatApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createPersonalChatAsync: builder.mutation<PersonalChatType, PersonalChatType>({
            query: personalChat => ({
                body: personalChat,
                url: '/PersonalChat',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChat', result }],
        }),
        updatePersonalChatAsync: builder.mutation<void, PersonalChatType>({
            query: personalChat => ({
                body: personalChat,
                url: '/PersonalChat',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChat', result }],
        }),
        removePersonalChatAsync: builder.mutation<void, number>({
            query: id => ({
                url: `/PersonalChat/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChat', result }],
        }),
        isExist: builder.query<boolean, { userId: string, targetUserId: string }>({
            query: ({ userId, targetUserId }) => `/PersonalChat/isExist?initiatorId=${userId}&companionId=${targetUserId}`,
            providesTags: (result, error, { userId, targetUserId }) => [{ type: 'PersonalChat', id: `${userId}-${targetUserId}` }],
        }),
        getPersonalChatById: builder.query<PersonalChatType, number>({
            query: id => `/PersonalChat/${id}`,
            providesTags: (result, error, id) => [{ type: 'PersonalChat', id }],
        }),
        getPersonalChatsByUserId: builder.query<PersonalChatType[], string>({
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