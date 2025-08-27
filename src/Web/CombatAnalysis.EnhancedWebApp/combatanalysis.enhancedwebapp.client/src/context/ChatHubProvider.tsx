/* eslint-disable @typescript-eslint/no-explicit-any */
import APP_CONFIG from '@/config/appConfig';
import { ChatHubContext } from '@/shared/hooks/useChatHub';
import * as signalR from '@microsoft/signalr';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import type { RootState } from '../app/Store';

const messageType = {
    default: 0,
    system: 1
};

const ChatHubProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const personalChatHubURL = `${APP_CONFIG.hubs.url}${APP_CONFIG.hubs.personalChat}`;
    const personalChatMessagesHubURL = `${APP_CONFIG.hubs.url}${APP_CONFIG.hubs.personalChatMessages}`;
    const personalChatUnreadMessagesHubURL = `${APP_CONFIG.hubs.url}${APP_CONFIG.hubs.personalChatUnreadMessage}`;
    const groupChatHubURL = `${APP_CONFIG.hubs.url}${APP_CONFIG.hubs.groupChat}`;
    const groupChatMessagesHubURL = `${APP_CONFIG.hubs.url}${APP_CONFIG.hubs.groupChatMessages}`;
    const groupChatUnreadMessagesHubURL = `${APP_CONFIG.hubs.url}${APP_CONFIG.hubs.groupChatUnreadMessage}`;

    const myself = useSelector((state: RootState) => state.user.value);

    const [personalChatHubConnection, setPersonalChatHubConnection] = useState<signalR.HubConnection | null>(null);
    const [personalChatMessagesHubConnection, setPersonalChatMessagesHubConnection] = useState<signalR.HubConnection | null>(null);
    const [personalChatUnreadMessagesHubConnection, setPersonalChatUnreadMessagesHubConnection] = useState<signalR.HubConnection | null>(null);
    const [groupChatHubConnection, setGroupChatHubConnection] = useState<signalR.HubConnection | null>(null);
    const [groupChatMessagesHubConnection, setGroupChatMessagesHubConnection] = useState<signalR.HubConnection | null>(null);
    const [groupChatUnreadMessagesHubConnection, setGroupChatUnreadMessagesHubConnection] = useState<signalR.HubConnection | null>(null);

    useEffect(() => {
        if (!myself) {
            return;
        }

        connectToPersonalChatAsync().then(async () => {
            await connectToGroupChatAsync();
        });

        return () => {
            const stopConnection = async () => {
                if (personalChatHubConnection) {
                    await personalChatHubConnection.stop();
                    setPersonalChatHubConnection(null);
                }

                if (groupChatHubConnection) {
                    await groupChatHubConnection.stop();
                    setGroupChatHubConnection(null);
                }
            }

            stopConnection();
        }
    }, [myself]);

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

    const connectToPersonalChatAsync = async () => {
        try {
            if (personalChatHubConnection || !myself) {
                return;
            }

            const connection = createHubConnection(personalChatHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", myself.id);

            setPersonalChatHubConnection(connection);
        } catch (e) {
            console.error(e);
        }
    }

    const connectToPersonalChatMessagesAsync = async (myPersonalChatId: number) => {
        try {
            if (personalChatMessagesHubConnection) {
                return;
            }

            const connection = createHubConnection(personalChatMessagesHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", myPersonalChatId);

            setPersonalChatMessagesHubConnection(connection);
        } catch (e) {
            console.error(e);
        }
    }

    const connectToPersonalChatUnreadMessagesAsync = async (myPersonalChatId: number) => {
        try {
            if (personalChatUnreadMessagesHubConnection) {
                return;
            }

            const connection = createHubConnection(personalChatUnreadMessagesHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", myPersonalChatId);

            setPersonalChatUnreadMessagesHubConnection(connection);
        } catch (e) {
            console.error(e);
        }
    }

    const connectToGroupChatAsync = async () => {
        try {
            if (groupChatHubConnection) {
                return;
            }

            const connection = createHubConnection(groupChatHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", myself?.id);

            setGroupChatHubConnection(connection);
        } catch (e) {
            console.error(e);
        }
    }

    const connectToGroupChatMessagesAsync = async (chatId: number) => {
        try {
            if (groupChatMessagesHubConnection) {
                return;
            }

            const connection = createHubConnection(groupChatMessagesHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", chatId);

            setGroupChatMessagesHubConnection(connection);
        } catch (e) {
            console.error(e);
        }
    }

    const connectToGroupChatUnreadMessagesAsync = async (myGroupChatId: number) => {
        try {
            if (groupChatUnreadMessagesHubConnection) {
                return;
            }

            const connection = createHubConnection(groupChatUnreadMessagesHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", myGroupChatId);

            setGroupChatUnreadMessagesHubConnection(connection);
        } catch (e) {
            console.error(e);
        }
    }

    const subscribeToPersonalChat = (callback: any) => {
        personalChatHubConnection?.on("ReceivePersonalChat", (chat) => {
            callback(chat);
        });
    }

    const subscribeToPersonalChatMessages = (callback: any) => {
        personalChatMessagesHubConnection?.on("ReceiveMessage", (message) => {
            callback(message);
        });
    }

    const subscribeToPersonalMessageHasBeenRead = (callback: any) => {
        personalChatMessagesHubConnection?.on("ReceiveMessageHasBeenRead", (messageId: number) => {
            callback(messageId);
        });
    }

    const subscribeToUnreadPersonalMessagesUpdated = (callback: any) => {
        personalChatUnreadMessagesHubConnection?.on("ReceiveUnreadMessage", (targetChatId: number, targetMeInChatId: string, count: number) => {
            callback(targetChatId, targetMeInChatId, count);
        });
    }

    const subscribeToGroupChat = (callback: any) => {
        groupChatHubConnection?.on("ReceiveGroupChat", async (chatId, appUserId) => {
            await groupChatHubConnection?.invoke("RequestJoinedUser", chatId, appUserId);
        });

        groupChatHubConnection?.on("ReceiveJoinedUser", (groupChatUser) => {
            console.log(groupChatUser);
            callback(groupChatUser);
        });
    }

    const subscribeGroupChatUser = () => {
        groupChatHubConnection?.on("ReceiveAddedUserToChat", async (groupChatUser) => {
            const systemMessage = `'${myself?.username}' added '${groupChatUser.username}' to chat`;
            await groupChatMessagesHubConnection?.invoke("SendMessage", systemMessage, groupChatUser.chatId, messageType["system"], groupChatUser.id, groupChatUser.username);
            await groupChatHubConnection?.invoke("RequestJoinedUser", groupChatUser.chatId, groupChatUser.appUserId);
        });
    }

    const subscribeToGroupChatMessages = (callback: any) => {
        groupChatMessagesHubConnection?.on("ReceiveMessage", (message) => {
            callback(message);
        });
    }

    const subscribeToGroupMessageDelivered = (chatId: number) => {
        groupChatMessagesHubConnection?.on("ReceiveMessageDelivered", async () => {
            await groupChatUnreadMessagesHubConnection?.invoke("SendUnreadMessageUpdated", chatId);
        });
    }

    const subscribeToUnreadGroupMessagesUpdated = (callback: any) => {
        groupChatUnreadMessagesHubConnection?.on("ReceiveUnreadMessage", (targetChatId, targetMeInChatId, count) => {
            callback(targetChatId, targetMeInChatId, count);
        });
    }

    const subscribeToGroupMessageHasBeenRead = (callback: any) => {
        groupChatMessagesHubConnection?.on("ReceiveMessageHasBeenRead", async (messageId: number) => {
            callback(messageId);
        });
    }

    return (
        <ChatHubContext.Provider value={{
            personalChatHubConnection, personalChatMessagesHubConnection, personalChatUnreadMessagesHubConnection,
            connectToPersonalChatAsync, connectToPersonalChatMessagesAsync, connectToPersonalChatUnreadMessagesAsync,
            subscribeToPersonalChat, subscribeToPersonalChatMessages, subscribeToPersonalMessageHasBeenRead, subscribeToUnreadPersonalMessagesUpdated,
            subscribeToGroupChat, subscribeGroupChatUser, subscribeToGroupChatMessages, subscribeToGroupMessageHasBeenRead, subscribeToUnreadGroupMessagesUpdated, subscribeToGroupMessageDelivered,
            groupChatHubConnection, groupChatMessagesHubConnection, groupChatUnreadMessagesHubConnection,
            connectToGroupChatAsync, connectToGroupChatMessagesAsync, connectToGroupChatUnreadMessagesAsync
        }}>
            {children}
        </ChatHubContext.Provider>
    );
}

export default ChatHubProvider;