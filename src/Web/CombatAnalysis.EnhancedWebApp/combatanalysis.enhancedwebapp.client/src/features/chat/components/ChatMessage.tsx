import * as signalR from '@microsoft/signalr';
import { useState } from 'react';
import type { AppUserModel } from '../../user/types/AppUserModel';
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

const messageStatus = {
    delivery: 0,
    delivered: 1,
    read: 2
};

interface ChatMessageProps {
    user: AppUserModel;
    chatUserAsUserId: string;
    chatUserUsername: string;
    reviewerId: string;
    messageOwnerId: string;
    message: PersonalChatMessageModel | GroupChatMessageModel;
    updateMessageAsync: (message: PersonalChatMessageModel | GroupChatMessageModel) => Promise<void>;
    hubConnection: signalR.HubConnection | null;
    subscribeToChatMessageHasBeenRead: (callback: (messageId: number) => void) => void;
}

const ChatMessage: React.FC<ChatMessageProps> = ({ user, chatUserAsUserId, chatUserUsername, reviewerId, messageOwnerId, message, updateMessageAsync, hubConnection, subscribeToChatMessageHasBeenRead }) => {
    const [targetMessage, setTargetMessage] = useState(message);

    const personalChatMessageHasBeenRead = (): void => {
        const updatedTargetMessage = Object.assign({}, targetMessage);
        updatedTargetMessage.status = 2;

        setTargetMessage(updatedTargetMessage);
    }

    const groupChatHasMessageBeenRead = (): void => {
        const updatedTargetMessage = Object.assign({}, targetMessage);
        if ("groupChatMessageId" in updatedTargetMessage) {
            updatedTargetMessage.groupChatMessageId = undefined;
        }

        setTargetMessage(updatedTargetMessage);
    }

    const personalChatMessageHasBeenReadHandle = async (): Promise<void> => {
        if (!hubConnection || reviewerId === messageOwnerId || targetMessage.status === messageStatus["read"]) {
            return;
        }

        await hubConnection?.invoke("SendMessageHasBeenRead", message.id, reviewerId);
    }

    const groupChatMessageHasBeenReadHandle = async (): Promise<void> => {
        if (!hubConnection || reviewerId === messageOwnerId || ('groupChatMessageId' in targetMessage && !targetMessage.groupChatMessageId)) {
            return;
        }

        await hubConnection?.invoke("SendMessageHasBeenRead", targetMessage.id, reviewerId);
    }

    return (
        <>
            {message.type === messageType["default"]
                ? <DefaultChatMessage
                    user={user}
                    chatUserAsUserId={chatUserAsUserId}
                    chatUserUsername={chatUserUsername}
                    reviewerId={reviewerId}
                    messageOwnerId={messageOwnerId}
                    message={targetMessage}
                    updateMessageAsync={updateMessageAsync}
                    subscribeToChatMessageHasBeenRead={subscribeToChatMessageHasBeenRead}
                    chatMessageHasMessageBeenRead={'groupChatMessageId' in targetMessage ? groupChatHasMessageBeenRead : personalChatMessageHasBeenRead}
                    messageHasBeenReadHandle={'groupChatMessageId' in targetMessage ? groupChatMessageHasBeenReadHandle : personalChatMessageHasBeenReadHandle}
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