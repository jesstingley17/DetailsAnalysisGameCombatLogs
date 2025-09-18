import * as signalR from '@microsoft/signalr';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';
import DefaultChatMessage from './DefaultChatMessage';
import LogChatMessage from './LogChatMessage';
import SystemChatMessage from './SystemChatMessage';

import './ChatMessage.scss';

const messageType = {
    default: 0,
    system: 1,
    log: 2
};

interface ChatMessageProps {
    reviewerId: string;
    chatUserAsUserId: string;
    chatUserUsername: string;
    messageOwnerId: string;
    message: PersonalChatMessageModel | GroupChatMessageModel;
    updateMessageAsync: (message: PersonalChatMessageModel | GroupChatMessageModel) => Promise<void>;
    hubConnection: signalR.HubConnection | null;
    subscribeToChatMessageHasBeenRead: (callback: (messageId: number) => void) => void;
    lastReadMessageId?: number;
}

const ChatMessage: React.FC<ChatMessageProps> = ({ reviewerId, chatUserAsUserId, chatUserUsername, messageOwnerId, message, updateMessageAsync, hubConnection, subscribeToChatMessageHasBeenRead, lastReadMessageId }) => {
    return (
        <>
            {message.type === messageType["default"]
                ? <DefaultChatMessage
                    chatUserAsUserId={chatUserAsUserId}
                    chatUserUsername={chatUserUsername}
                    reviewerId={reviewerId}
                    messageOwnerId={messageOwnerId}
                    message={message}
                    updateMessageAsync={updateMessageAsync}
                    subscribeToChatMessageHasBeenRead={subscribeToChatMessageHasBeenRead}
                    hubConnection={hubConnection}
                    lastReadMessageId={lastReadMessageId}
                />
                : message.type === messageType["log"]
                    ? <LogChatMessage
                        message={message.message}
                    />
                    : <SystemChatMessage
                        message={message.message}
                    />
            }
        </>
    );
}

export default ChatMessage;