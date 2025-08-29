import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { AppNotificationModel } from '../types/AppNotificationModel';

const apiURL = '/api/v1';

export const NotificationApi = createApi({
    reducerPath: 'notificationApi',
    tagTypes: [
        'Notification',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getNotificationsByRecipientId: builder.query<AppNotificationModel[], string>({
            query: recipientId => ({
                url: `/Notification/getByRecipientId/${recipientId}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Notification' as const, id })), { type: 'Notification' }]
                    : [{ type: 'Notification' }]
        }),
        getUnreadNotificationsByRecipientId: builder.query<AppNotificationModel[], string>({
            query: recipientId => ({
                url: `/Notification/getUnreadByRecipientId/${recipientId}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Notification' as const, id })), { type: 'Notification' }]
                    : [{ type: 'Notification' }]
        }),
    })
})

export const {
    useLazyGetNotificationsByRecipientIdQuery,
    useLazyGetUnreadNotificationsByRecipientIdQuery,
} = NotificationApi;