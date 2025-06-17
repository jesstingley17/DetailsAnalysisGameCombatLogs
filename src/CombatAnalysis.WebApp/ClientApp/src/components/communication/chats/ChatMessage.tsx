import { ChatMessageProps } from '../../../types/components/communication/chats/ChatMessageProps';
import DefaultChatMessage from './DefaultChatMessage';
import DefaultGroupChatMessage from './DefaultGroupChatMessage';
import LogChatMessage from './LogChatMessage';
import SystemChatMessage from './SystemChatMessage';

import '../../../styles/communication/chats/chatMessage.scss';

const messageType = {
    default: 0,
    system: 1,
    log: 2
};

const ChatMessage: React.FC<ChatMessageProps> = ({ chatType, me, meInChatId, reviewerId, messageOwnerId, message, updateMessageAsync, hubConnection, subscribeToChatMessageHasBeenRead }) => {
    return (
        <>
            {message.type === messageType["default"]
                ? chatType === 0 ?
                    <DefaultChatMessage
                        me={me}
                        meInChatId={meInChatId}
                        reviewerId={reviewerId}
                        messageOwnerId={messageOwnerId}
                        message={message}
                        updateMessageAsync={updateMessageAsync}
                        hubConnection={hubConnection}
                        subscribeToChatMessageHasBeenRead={subscribeToChatMessageHasBeenRead}
                    />
                    : <DefaultGroupChatMessage
                        me={me}
                        meInChatId={meInChatId}
                        reviewerId={reviewerId}
                        messageOwnerId={messageOwnerId}
                        message={message}
                        updateMessageAsync={updateMessageAsync}
                        hubConnection={hubConnection}
                        subscribeToChatMessageHasBeenRead={subscribeToChatMessageHasBeenRead}
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