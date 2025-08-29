import * as signalR from '@microsoft/signalr';
import type { RefObject } from 'react';
import type { AppNotificationModel } from './AppNotificationModel';

export type NotificationHubContextModel = {
    notificationHubConnectionRef: RefObject<signalR.HubConnection | null>;
    connectToNotificationAsync: () => Promise<void>;
    subscribeToNotifications: (callback: (notification: AppNotificationModel) => void) => void;
    subscribeToRecipientNotifications: (callback: () => void) => void;
    disconnectNotificationsHubAsync: () => Promise<void>;
}