
export type NotificationHubContextType = {
    notificationHubConnection: signalR.HubConnection | null;
    connectToNotificationAsync(): Promise<void>;
    subscribeToNotifications(callback: any): void;
    subscribeToRecipientNotifications(callback: any): void;
}