import { useGetGroupChatUserByIdQuery } from '../../../../store/api/chat/GroupChatUser.api';
import { GroupChatMessageProps } from '../../../../types/components/communication/chats/GroupChatMessageProps';
import ChatMessage from '../ChatMessage';

const GroupChatMessage: React.FC<GroupChatMessageProps> = ({ me, reviewerId, messageOwnerId, message, updateMessageAsync, hubConnection, subscribeToChatMessageHasBeenRead }) => {
    const { data: groupChatUser, isLoading } = useGetGroupChatUserByIdQuery(messageOwnerId);

    if (isLoading) {
        return (<></>);
    }

    return (
        <ChatMessage
            chatType={1}
            me={me}
            meInChatId={groupChatUser.appUserId}
            reviewerId={reviewerId}
            messageOwnerId={messageOwnerId}
            message={message}
            updateMessageAsync={updateMessageAsync}
            hubConnection={hubConnection}
            subscribeToChatMessageHasBeenRead={subscribeToChatMessageHasBeenRead}
        />
    );
}

export default GroupChatMessage;