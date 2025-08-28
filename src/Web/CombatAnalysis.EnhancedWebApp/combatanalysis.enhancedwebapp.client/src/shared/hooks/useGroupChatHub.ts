import APP_CONFIG from '@/config/appConfig';
import * as signalR from '@microsoft/signalr';
import type { RefObject } from 'react';
import type { AppUserModel } from '../../features/user/types/AppUserModel';
import type { GroupChatUserModel } from '../../features/chat/types/GroupChatUserModel';
import type { GroupChatMessageModel } from '../../features/chat/types/GroupChatMessageModel';

const messageType = {
    default: 0,
    system: 1
};

const useGroupChatHub = (
    myself: AppUserModel | null,
    groupChatHubConnectionRef: RefObject<signalR.HubConnection | null>,
    groupChatMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>,
    groupChatUnreadMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>,
) => {
    const groupChatHubURL = `${APP_CONFIG.hubs.url}${APP_CONFIG.hubs.groupChat}`;
    const groupChatMessagesHubURL = `${APP_CONFIG.hubs.url}${APP_CONFIG.hubs.groupChatMessages}`;
    const groupChatUnreadMessagesHubURL = `${APP_CONFIG.hubs.url}${APP_CONFIG.hubs.groupChatUnreadMessage}`;

    const createHubConnection = (url: string): signalR.HubConnection => {
        const hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(url, {
                withCredentials: true,
                transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
            })
            .withAutomaticReconnect()
            .build();

        return hubConnection;
    }

    const connectToGroupChatAsync = async () => {
        try {
            const connection = createHubConnection(groupChatHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", myself?.id);

            groupChatHubConnectionRef.current = connection;
        } catch (e) {
            console.error(e);
        }
    }

    const connectToGroupChatMessagesAsync = async (chatId: number) => {
        try {
            const connection = createHubConnection(groupChatMessagesHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", chatId);

            groupChatMessagesHubConnectionRef.current = connection;
        } catch (e) {
            console.error(e);
        }
    }

    const connectToGroupChatUnreadMessagesAsync = async (myGroupChatsId: number[]) => {
        try {
            const connection = createHubConnection(groupChatUnreadMessagesHubURL);

            await connection.start();
            for (let i = 0; i < myGroupChatsId.length; i++) {
                await connection.invoke("JoinRoom", myGroupChatsId[i]);
            }

            groupChatUnreadMessagesHubConnectionRef.current = connection;
        } catch (e) {
            console.error(e);
        }
    }

    const subscribeToGroupChat = (callback: (groupChatUser: GroupChatUserModel) => void) => {
        groupChatHubConnectionRef.current?.on("ReceiveGroupChat", async (chatId: number, appUserId: string) => {
            await groupChatHubConnectionRef.current?.invoke("RequestJoinedUser", chatId, appUserId);
        });

        groupChatHubConnectionRef.current?.on("ReceiveJoinedUser", (groupChatUser) => {
            callback(groupChatUser);
        });
    }

    const subscribeGroupChatUser = () => {
        groupChatHubConnectionRef.current?.on("ReceiveAddedUserToChat", async (groupChatUser) => {
            const systemMessage = `'${myself?.username}' added '${groupChatUser.username}' to chat`;
            await groupChatMessagesHubConnectionRef.current?.invoke("SendMessage", systemMessage, groupChatUser.chatId, messageType["system"], groupChatUser.id, groupChatUser.username);
            await groupChatHubConnectionRef.current?.invoke("RequestJoinedUser", groupChatUser.chatId, groupChatUser.appUserId);
        });
    }

    const subscribeToGroupChatMessages = (callback: (message: GroupChatMessageModel) => void) => {
        groupChatMessagesHubConnectionRef.current?.on("ReceiveMessage", (message) => {
            callback(message);
        });
    }

    const subscribeToGroupMessageDelivered = (chatId: number) => {
        groupChatMessagesHubConnectionRef.current?.on("ReceiveMessageDelivered", async () => {
            await groupChatMessagesHubConnectionRef.current?.invoke("SendUnreadMessageUpdated", chatId);
        });
    }

    const subscribeToUnreadGroupMessagesUpdated = (callback: (targetChatId: number, targetMeInChatId: string, count: number) => void) => {
        groupChatUnreadMessagesHubConnectionRef.current?.on("ReceiveUnreadMessage", (targetChatId: number, targetMeInChatId: string, count: number) => {
            callback(targetChatId, targetMeInChatId, count);
        });
    }

    const subscribeToGroupMessageHasBeenRead = (callback: (messageId: number) => void) => {
        groupChatMessagesHubConnectionRef.current?.on("ReceiveMessageHasBeenRead", (messageId: number) => {
            callback(messageId);
        });
    }

    const disconnectFromGroupChatHub = async () => {
        await groupChatHubConnectionRef.current?.stop();
        groupChatHubConnectionRef.current = null;

        await groupChatMessagesHubConnectionRef.current?.stop();
        groupChatMessagesHubConnectionRef.current = null;
    }

    const disconnectFromGroupChatUnreadMessagesHub = async () => {
        await groupChatUnreadMessagesHubConnectionRef.current?.stop();
        groupChatUnreadMessagesHubConnectionRef.current = null;
    }

    return {
        connectToGroupChatAsync, connectToGroupChatMessagesAsync, connectToGroupChatUnreadMessagesAsync,
        subscribeToGroupChat, subscribeGroupChatUser, subscribeToGroupChatMessages, subscribeToGroupMessageDelivered,
        subscribeToUnreadGroupMessagesUpdated, subscribeToGroupMessageHasBeenRead,
        disconnectFromGroupChatHub, disconnectFromGroupChatUnreadMessagesHub
    }
}

export default useGroupChatHub;