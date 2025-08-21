import { useState } from 'react';
import User from '../../user/components/User';
import type { AppUserModel } from '../../user/types/AppUserModel';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';

interface ChatMessageTitleProps {
    myself: AppUserModel;
    itIsMe: boolean;
    message: PersonalChatMessageModel | GroupChatMessageModel;
    chatUserAsUserId: string;
    chatUserUsername: string;
}

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