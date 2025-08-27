/* eslint-disable @typescript-eslint/no-explicit-any */
import * as signalR from '@microsoft/signalr';

export type ChatHubModel = {
    personalChatHubConnection: signalR.HubConnection | null;
    personalChatMessagesHubConnection: signalR.HubConnection | null;
    personalChatUnreadMessagesHubConnection: signalR.HubConnection | null;
    groupChatHubConnection: signalR.HubConnection | null;
    groupChatMessagesHubConnection: signalR.HubConnection | null;
    groupChatUnreadMessagesHubConnection: signalR.HubConnection | null;
    connectToPersonalChatAsync(): Promise<void>;
    connectToPersonalChatMessagesAsync(myPersonalChatId: number): Promise<void>;
    connectToPersonalChatUnreadMessagesAsync(myPersonalChatId: number): Promise<void>;
    subscribeToPersonalChat(callback: any): void;
    subscribeToPersonalChatMessages(callback: any): void;
    subscribeToPersonalMessageHasBeenRead(callback: any): void;
    subscribeToUnreadPersonalMessagesUpdated(callback: any): void;
    connectToGroupChatAsync(callback: any): Promise<void>;
    connectToGroupChatMessagesAsync(callback: any): Promise<void>;
    connectToGroupChatUnreadMessagesAsync(myGroupChatId: number): Promise<void>;
    subscribeToGroupChat(callback: any): void;
    subscribeGroupChatUser(callback: any): void;
    subscribeToGroupChatMessages(callback: any): void;
    subscribeToGroupMessageHasBeenRead(callback: any): void;
    subscribeToUnreadGroupMessagesUpdated(callback: any): void;
    subscribeToGroupMessageDelivered(callback: any): void;
}