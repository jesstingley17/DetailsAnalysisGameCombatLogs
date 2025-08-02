import { AppUser } from '../AppUser';

export type ChatHubContextType = {
    personalChatHubConnection: signalR.HubConnection | null;
    personalChatMessagesHubConnection: signalR.HubConnection | null;
    personalChatUnreadMessagesHubConnection: signalR.HubConnection | null;
    groupChatHubConnection: signalR.HubConnection | null;
    groupChatMessagesHubConnection: signalR.HubConnection | null;
    groupChatUnreadMessagesHubConnection: signalR.HubConnection | null;
    connectToPersonalChatAsync(): Promise<void>;
    connectToPersonalChatMessagesAsync(chatId: number): Promise<void>;
    connectToPersonalChatUnreadMessagesAsync(meInChats: AppUser[]): Promise<void>;
    subscribeToPersonalChat(callback: any): void;
    subscribeToPersonalChatMessages(callback: any): void;
    subscribeToPersonalMessageHasBeenRead(callback: any): void;
    subscribeToUnreadPersonalMessagesUpdated(callback: any): void;
    connectToGroupChatAsync(callback: any): Promise<void>;
    connectToGroupChatMessagesAsync(callback: any): Promise<void>;
    connectToGroupChatUnreadMessagesAsync(callback: any): Promise<void>;
    subscribeToGroupChat(callback: any): void;
    subscribeGroupChatUser(callback: any): void;
    subscribeToGroupChatMessages(callback: any): void;
    subscribeToGroupMessageHasBeenRead(callback: any): void;
    subscribeToUnreadGroupMessagesUpdated(callback: any): void;
    subscribeToGroupMessageDelivered(callback: any): void;
}