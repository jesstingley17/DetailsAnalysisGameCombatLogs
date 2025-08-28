import type { RootState } from '@/app/Store';
import { ChatHubContext } from '@/shared/hooks/useChatHub';
import useGroupChatHub from '@/shared/hooks/useGroupChatHub';
import usePersonalChatHub from '@/shared/hooks/usePersonalChatHub';
import * as signalR from '@microsoft/signalr';
import { useEffect, useRef } from 'react';
import { useSelector } from 'react-redux';

const ChatHubProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const myself = useSelector((state: RootState) => state.user.value);

    const personalChatHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const personalChatMessagesHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const personalChatUnreadMessagesHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const groupChatHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const groupChatMessagesHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const groupChatUnreadMessagesHubConnectionRef = useRef<signalR.HubConnection | null>(null);

    const {
        connectToPersonalChatAsync, connectToPersonalChatMessagesAsync, connectToPersonalChatUnreadMessagesAsync,
        subscribeToPersonalChat, subscribeToPersonalChatMessages, subscribeToPersonalMessageHasBeenRead, subscribeToUnreadPersonalMessagesUpdated,
        disconnectFromPersonalChatHub, disconnectFromPersonalChatUnreadMessagesHub
    } = usePersonalChatHub(myself, personalChatHubConnectionRef, personalChatMessagesHubConnectionRef, personalChatUnreadMessagesHubConnectionRef);

    const {
        connectToGroupChatAsync, connectToGroupChatMessagesAsync, connectToGroupChatUnreadMessagesAsync,
        subscribeToGroupChat, subscribeGroupChatUser, subscribeToGroupChatMessages, subscribeToGroupMessageDelivered,
        subscribeToUnreadGroupMessagesUpdated, subscribeToGroupMessageHasBeenRead,
        disconnectFromGroupChatHub, disconnectFromGroupChatUnreadMessagesHub
    } = useGroupChatHub(myself, groupChatHubConnectionRef, groupChatMessagesHubConnectionRef, groupChatUnreadMessagesHubConnectionRef);

    useEffect(() => {
        if (!myself) {
            return;
        }

        return () => {
            const stopConnection = async () => {
                if (personalChatHubConnectionRef.current) {
                    await personalChatHubConnectionRef.current.stop();
                    personalChatHubConnectionRef.current = null;
                }

                if (groupChatHubConnectionRef.current) {
                    await groupChatHubConnectionRef.current.stop();
                    groupChatHubConnectionRef.current = null;
                }
            }

            stopConnection();
        }
    }, [myself]);

    return (
        <ChatHubContext.Provider value={{
            personalChatHubConnectionRef, personalChatMessagesHubConnectionRef, personalChatUnreadMessagesHubConnectionRef,
            connectToPersonalChatAsync, connectToPersonalChatMessagesAsync, connectToPersonalChatUnreadMessagesAsync,
            subscribeToPersonalChat, subscribeToPersonalChatMessages, subscribeToPersonalMessageHasBeenRead, subscribeToUnreadPersonalMessagesUpdated,
            disconnectFromPersonalChatHub, disconnectFromPersonalChatUnreadMessagesHub,
            groupChatHubConnectionRef, groupChatMessagesHubConnectionRef, groupChatUnreadMessagesHubConnectionRef,
            connectToGroupChatAsync, connectToGroupChatMessagesAsync, connectToGroupChatUnreadMessagesAsync,
            subscribeToGroupChat, subscribeGroupChatUser, subscribeToGroupChatMessages, subscribeToGroupMessageHasBeenRead, subscribeToUnreadGroupMessagesUpdated, subscribeToGroupMessageDelivered,
            disconnectFromGroupChatHub, disconnectFromGroupChatUnreadMessagesHub
        }}>
            {children}
        </ChatHubContext.Provider>
    );
}

export default ChatHubProvider;