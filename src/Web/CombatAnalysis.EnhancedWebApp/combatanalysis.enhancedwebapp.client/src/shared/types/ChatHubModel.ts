import type { GroupChatMessageModel } from '@/features/chat/types/GroupChatMessageModel';
import type { GroupChatUserModel } from '@/features/chat/types/GroupChatUserModel';
import type { PersonalChatMessageModel } from '@/features/chat/types/PersonalChatMessageModel';
import type { PersonalChatModel } from '@/features/chat/types/PersonalChatModel';
import * as signalR from '@microsoft/signalr';
import type { RefObject } from 'react';

export type ChatHubContextModel = {
    personalChatHubConnectionRef: RefObject<signalR.HubConnection | null>;
    personalChatMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>;
    personalChatUnreadMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>;
    groupChatHubConnectionRef: RefObject<signalR.HubConnection | null>;
    groupChatMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>;
    groupChatUnreadMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>;
    connectToPersonalChatAsync: () => Promise<void>;
    connectToPersonalChatMessagesAsync: (myPersonalChatId: number) => Promise<void>;
    connectToPersonalChatUnreadMessagesAsync: (myPersonalChatsId: number[]) => Promise<void>;
    subscribeToPersonalChat: (callback: (chat: PersonalChatModel) => void) => void;
    subscribeToPersonalChatMessages: (callback: (message: PersonalChatMessageModel) => void) => void;
    subscribeToPersonalMessageHasBeenRead: (callback: (messageId: number) => void) => void;
    subscribeToUnreadPersonalMessagesUpdated: (callback: (targetChatId: number, targetMeInChatId: string, count: number) => void) => void;
    connectToGroupChatAsync: () => Promise<void>;
    connectToGroupChatMessagesAsync: (chatId: number) => Promise<void>;
    connectToGroupChatUnreadMessagesAsync: (myGroupChatsId: number[]) => Promise<void>;
    subscribeToGroupChat: (callback: (groupChatUser: GroupChatUserModel) => void) => void;
    subscribeGroupChatUser: () => void;
    subscribeToGroupChatMessages: (callback: (message: GroupChatMessageModel) => void) => void;
    subscribeToGroupMessageDelivered: (chatId: number) => void;
    subscribeToUnreadGroupMessagesUpdated: (callback: (targetChatId: number, targetMeInChatId: string, count: number) => void) => void;
    subscribeToGroupMessageHasBeenRead: (callback: (messageId: number) => void) => void;
    disconnectFromPersonalChatHubAsync: () => Promise<void>;
    disconnectFromPersonalChatUnreadMessagesHubAsync: () => Promise<void>;
    disconnectFromGroupChatHubAsync: () => Promise<void>;
    disconnectFromGroupChatUnreadMessagesHubAsync: () => Promise<void>;
}