import * as signalR from '@microsoft/signalr';
import { createContext, useContext, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { AppUser } from '../types/AppUser';
import { GroupChatUser } from '../types/GroupChatUser';
import { ChatHubContextType } from '../types/context/ChatHubType';

const ChatHubContext = createContext<ChatHubContextType | null>(null);

const messageType = {
    default: 0,
    system: 1
};

export const ChatHubProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const personalChatHubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_PERSONAL_CHAT_ADDRESS}`;
    const personalChatMessagesHubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_PERSONAL_CHAT_MESSAGES_ADDRESS}`;
    const personalChatUnreadMessagesHubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_PERSONAL_CHAT_UNREAD_MESSAGES_ADDRESS}`;
    const groupChatHubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_GROUP_CHAT_ADDRESS}`;
    const groupChatMessagesHubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_GROUP_CHAT_MESSAGES_ADDRESS}`;
    const groupChatUnreadMessagesHubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_GROUP_CHAT_UNREAD_MESSAGES_ADDRESS}`;

    const me = useSelector((state: any) => state.user.value);

    const [personalChatHubConnection, setPersonalChatHubConnection] = useState<signalR.HubConnection | null>(null);
    const [personalChatMessagesHubConnection, setPersonalChatMessagesHubConnection] = useState<signalR.HubConnection | null>(null);
    const [personalChatUnreadMessagesHubConnection, setPersonalChatUnreadMessagesHubConnection] = useState<signalR.HubConnection | null>(null);
    const [groupChatHubConnection, setGroupChatHubConnection] = useState<signalR.HubConnection | null>(null);
    const [groupChatMessagesHubConnection, setGroupChatMessagesHubConnection] = useState<signalR.HubConnection | null>(null);
    const [groupChatUnreadMessagesHubConnection, setGroupChatUnreadMessagesHubConnection] = useState<signalR.HubConnection | null>(null);

    useEffect(() => {
        if (!me) {
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
    }, [me]);

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
            if (personalChatHubConnection) {
                return;
            }

            const connection = createHubConnection(personalChatHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", me.id);

            setPersonalChatHubConnection(connection);
        } catch (e) {
            console.error(e);
        }
    }

    const connectToPersonalChatMessagesAsync = async (chatId: number) => {
        try {
            if (personalChatMessagesHubConnection) {
                return;
            }

            const connection = createHubConnection(personalChatMessagesHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", chatId);

            setPersonalChatMessagesHubConnection(connection);
        } catch (e) {
            console.error(e);
        }
    }

    const connectToPersonalChatUnreadMessagesAsync = async (meInChats: AppUser[]) => {
        try {
            if (personalChatUnreadMessagesHubConnection) {
                return;
            }

            const connection = createHubConnection(personalChatUnreadMessagesHubURL);

            await connection.start();
            for (let i = 0; i < meInChats.length; i++) {
                await connection.invoke("JoinRoom", meInChats[i].id);
            }

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
            await connection.invoke("JoinRoom", me.id);

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

    const connectToGroupChatUnreadMessagesAsync = async (meInChats: GroupChatUser[]) => {
        try {
            if (groupChatUnreadMessagesHubConnection) {
                return;
            }

            const connection = createHubConnection(groupChatUnreadMessagesHubURL);

            await connection.start();
            for (let i = 0; i < meInChats.length; i++) {
                await connection.invoke("JoinRoom", meInChats[i].chatId);
            }

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
            callback(groupChatUser);
        });
    }

    const subscribeGroupChatUser = () => {
        groupChatHubConnection?.on("ReceiveAddedUserToChat", async (groupChatUser) => {
            const systemMessage = `'${me?.username}' added '${groupChatUser.username}' to chat`;
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
        groupChatMessagesHubConnection?.on("ReceiveMessageHasBeenRead", async (chatMessage: number) => {
            callback(chatMessage);
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

export const useChatHub = () => useContext(ChatHubContext);