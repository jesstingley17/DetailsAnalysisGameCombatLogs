import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { AppNotification } from '../../../types/AppNotification';

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
        getNotificationsByRecipientId: builder.query<AppNotification[], string>({
            query: (recipientId) => ({
                url: `/Notification/getByRecipientId/${recipientId}`,
            }),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Notification' as const, id })), { type: 'Notification' }]
                    : [{ type: 'Notification' }]
        }),
    })
})

export const {
    useLazyGetNotificationsByRecipientIdQuery,
} = NotificationApi;