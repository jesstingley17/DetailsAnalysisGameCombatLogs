import { useState } from 'react';
import { useGetUserByIdQuery } from '../../../store/api/user/Account.api';
import { ChatMessageTitleProps } from '../../../types/components/communication/chats/ChatMessageTitleProps';
import User from '../User';

const ChatMessageTitle: React.FC<ChatMessageTitleProps> = ({ me, itIsMe, message, meInChatId }) => {
    const [userInformation, setUserInformation] = useState(null);

    const { data: user, isLoading } = useGetUserByIdQuery(meInChatId);

    const getMessageTime = () => {
        const getDate = new Date(message?.time);
        const time = `${getDate.getHours()}:${getDate.getMinutes() }`;

        return time;
    }

    if (isLoading) {
        return (<></>);
    }

    return (
        <>
            <div className={`message-title ${itIsMe ? 'me' : 'another'}`}>
                <div className="content">
                    <div className="message-time">
                        <div>{getMessageTime()}</div>
                    </div>
                    <User
                        me={me}
                        targetUserId={user?.id}
                        setUserInformation={setUserInformation}
                    />
                </div>
                <div className="chat-user-information">
                    {userInformation}
                </div>
            </div>
        </>
    );
}

export default ChatMessageTitle;