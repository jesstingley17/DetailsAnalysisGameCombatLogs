import { useChatHub } from '@/shared/hooks/useChatHub';
import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import type { AppUserModel } from '../../user/types/AppUserModel';
import type { GroupChatUserModel } from '../types/GroupChatUserModel';

const chatType = {
    personal: 0,
    group: 1
};

const emptyMessageNotificationTimeout = 4000;

interface MessageInputProps {
    chatId: number;
    initiator: GroupChatUserModel | AppUserModel;
    targetChatType: number;
    t: (key: string) => string;
    recipientId?: string;
}

const MessageInput: React.FC<MessageInputProps> = ({ chatId, initiator, targetChatType, t, recipientId }) => {
    const chatHub = useChatHub();

    const messageInput = useRef<HTMLInputElement | null>(null);

    const [isEmptyMessage, setIsEmptyMessage] = useState(false);

    useEffect(() => {
        if (!chatHub) {
            return;
        }

        if (targetChatType === chatType["group"]) {
            chatHub.subscribeToGroupMessageDelivered(chatId);
        }
    }, []);

    const sendMessageByKeyHandle = async (code: string) => {
        if (code !== "Enter") {
            return;
        }

        await sendMessageAsync();
    };

    const sendMessageHandle = async () => {
        if (!chatHub) {
            return;
        }

        await sendMessageAsync();
    };

    const sendMessageAsync = async () => {
        if (!chatHub || !messageInput || !messageInput.current) {
            return;
        }

        if (messageInput.current.value === "") {
            sentEmptyMessage();

            return;
        }

        if (targetChatType === chatType["group"]) {
            const groupChatMessage = {
                id: 0,
                username: initiator.username,
                message: messageInput.current.value,
                time: new Date(),
                status: 0,
                type: 0,
                markedType: 0,
                isEdited: false,
                groupChatId: chatId,
                groupChatUserId: initiator.id
            };

            await chatHub.groupChatMessagesHubConnectionRef.current?.invoke("SendMessage", groupChatMessage);
        }
        else {
            const personalChatMessage = {
                id: 0,
                username: initiator.username,
                message: messageInput.current.value,
                time: new Date(),
                status: 0,
                type: 0,
                markedType: 0,
                isEdited: false,
                personalChatId: chatId,
                appUserId: initiator.id
            };

            await chatHub.personalChatMessagesHubConnectionRef.current?.invoke("SendMessage", personalChatMessage, recipientId);
        }

        messageInput.current.value = "";
    };

    const sentEmptyMessage = () => {
        setIsEmptyMessage(true);

        setTimeout(() => {
            setIsEmptyMessage(false);
        }, emptyMessageNotificationTimeout);
    };

    return (
        <div>
            <div className={`empty-message${isEmptyMessage ? "_show" : ""}`}>{t("CanNotSendEmpty")}</div>
            <div className="form-group input-message">
                <input type="text" className="form-control" placeholder={t("TypeYourMessage")}
                    ref={messageInput} onKeyDown={async (e) => await sendMessageByKeyHandle(e.code)} />
                <FontAwesomeIcon
                    icon={faPaperPlane}
                    title={t("SendMessage")}
                    onClick={sendMessageHandle} />
            </div>
        </div>
    );
}

export default MessageInput;
