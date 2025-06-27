import { useState } from 'react';
import { ChatMessageTitleProps } from '../../../types/components/communication/chats/ChatMessageTitleProps';
import User from '../User';

const ChatMessageTitle: React.FC<ChatMessageTitleProps> = ({ myself, itIsMe, message, chatUserAsUserId, chatUserUsername }) => {
    const [userInformation, setUserInformation] = useState(null);

    const getMessageTime = () => {
        const getDate = new Date(message?.time);
        const time = `${getDate.getHours()}:${getDate.getMinutes() }`;

        return time;
    }

    return (
        <>
            <div className={`message-title ${itIsMe ? 'me' : 'another'}`}>
                <div className="content">
                    <div className="message-time">
                        <div>{getMessageTime()}</div>
                    </div>
                    <User
                        myself={myself}
                        targetUserId={chatUserAsUserId}
                        targetUsername={chatUserUsername}
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