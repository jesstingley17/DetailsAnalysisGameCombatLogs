import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from "react";
import { useChatHub } from '../../../context/ChatHubProvider';
import { MessageInputProps } from '../../../types/components/communication/chats/MessageInputProps';

const chatType = {
    personal: 0,
    group: 1
}

const emptyMessageNotificationTimeout = 4000;

const MessageInput: React.FC<MessageInputProps> = ({ chat, meInChat, setAreLoadingOldMessages, targetChatType, t }) => {
    const chatHub = useChatHub();

    const messageInput = useRef<any>(null);

    const [isEmptyMessage, setIsEmptyMessage] = useState(false);

    useEffect(() => {
        if (!chatHub) {
            return;
        }

        if (targetChatType === chatType["group"]) {
            chatHub.subscribeToGroupMessageDelivered(chat.id);
        }
    }, []);

    const handleSendMessageByKeyAsync = async (e: any) => {
        if (!chatHub || e.code !== "Enter") {
            return;
        }
        else if (messageInput.current.value === "") {
            sentEmptyMessage();

            return;
        }

        setAreLoadingOldMessages(false);

        if (targetChatType === chatType["personal"]) {
            await chatHub.personalChatMessagesHubConnection?.invoke("SendMessage", messageInput.current.value, chat.id, meInChat?.id, meInChat?.username);
        }
        else {
            await chatHub.groupChatMessagesHubConnection?.invoke("SendMessage", messageInput.current.value, chat.id, 0, meInChat?.id, meInChat?.username);
        }

        messageInput.current.value = "";
    }

    const handleSendMessageAsync = async () => {
        if (!chatHub) {
            return;
        }

        if (messageInput.current.value === "") {
            sentEmptyMessage();

            return;
        }

        setAreLoadingOldMessages(false);

        if (targetChatType === chatType["personal"]) {
            await chatHub.personalChatMessagesHubConnection?.invoke("SendMessage", messageInput.current.value, chat.id, meInChat?.id, meInChat?.username);
        }
        else {
            await chatHub.groupChatMessagesHubConnection?.invoke("SendMessage", messageInput.current.value, chat.id, 0, meInChat?.id, meInChat?.username);
        }

        messageInput.current.value = "";
    }

    const sentEmptyMessage = () => {
        setIsEmptyMessage(true);

        setTimeout(() => {
            setIsEmptyMessage(false);
        }, emptyMessageNotificationTimeout);
    }

    return (
        <div>
            <div className={`empty-message${isEmptyMessage ? "_show" : ""}`}>{t("CanNotSendEmpty")}</div>
            <div className="form-group input-message">
                <input type="text" className="form-control" placeholder={t("TypeYourMessage")}
                    ref={messageInput} onKeyDown={handleSendMessageByKeyAsync} />
                <FontAwesomeIcon
                    icon={faPaperPlane}
                    title={t("SendMessage")}
                    onClick={handleSendMessageAsync}
                />
            </div>
        </div>
    );
}

export default MessageInput;