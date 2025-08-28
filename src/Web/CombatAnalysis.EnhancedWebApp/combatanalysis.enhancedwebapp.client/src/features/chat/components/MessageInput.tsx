import { useChatHub } from '@/shared/hooks/useChatHub';
import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState, type SetStateAction } from 'react';
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
    setAreLoadingOldMessages: (value: SetStateAction<boolean>) => void;
    t: (key: string) => string;
    targetChatType?: number;
    companionId?: string;
}

const MessageInput: React.FC<MessageInputProps> = ({ chatId, initiator, setAreLoadingOldMessages, t, targetChatType = 0, companionId }) => {
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

        setAreLoadingOldMessages(false);

        if (targetChatType === chatType["group"]) {
            await chatHub.groupChatMessagesHubConnectionRef.current?.invoke("SendMessage", messageInput.current.value, chatId, 0, initiator.id, initiator.username);
        }
        else {
            await chatHub.personalChatMessagesHubConnectionRef.current?.invoke("SendMessage", messageInput.current.value, chatId, initiator.id, initiator.username, companionId);
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
