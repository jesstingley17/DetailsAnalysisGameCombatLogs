import * as signalR from '@microsoft/signalr';
import { createContext, useContext, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { NotificationHubContextType } from '../types/context/NotificationHubContextType';

const NotificationHubContext = createContext<NotificationHubContextType | null>(null);

export const NotificationProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const notificationHubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_NOTIFICATION_ADDRESS}`;

    const me = useSelector((state: any) => state.user.value);

    const [notificationHubConnection, setNotificationHubConnection] = useState<signalR.HubConnection | null>(null);

    useEffect(() => {
        if (!me) {
            return;
        }

        const connectToNotifications = async () => {
            await connectToNotificationAsync();
        }

        connectToNotifications();

        return () => {
            const stopConnection = async () => {
                if (notificationHubConnection) {
                    await notificationHubConnection.stop();
                    setNotificationHubConnection(null);
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

    const connectToNotificationAsync = async () => {
        try {
            if (notificationHubConnection) {
                return;
            }

            const connection = createHubConnection(notificationHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", me.id);

            setNotificationHubConnection(connection);
        } catch (e) {
            console.error(e);
        }
    }

    const subscribeToNotifications = (callback: any) => {
        notificationHubConnection?.on("ReceiveNotification", (notification) => {
            callback(notification);
        });
    }

    return (
        <NotificationHubContext.Provider value={{
            notificationHubConnection, connectToNotificationAsync, subscribeToNotifications
        }}>
            {children}
        </NotificationHubContext.Provider>
    );
}

export const useNotificationHub = () => useContext(NotificationHubContext);