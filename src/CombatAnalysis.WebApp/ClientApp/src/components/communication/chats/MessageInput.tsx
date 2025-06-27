import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from "react";
import { useChatHub } from '../../../context/ChatHubProvider';
import { MessageInputProps } from '../../../types/components/communication/chats/MessageInputProps';

const chatType = {
    personal: 0,
    group: 1
};

const emptyMessageNotificationTimeout = 4000;

const MessageInput: React.FC<MessageInputProps> = ({ chatId, IasGroupChatUser, setAreLoadingOldMessages, targetChatType, companionsId, t }) => {
    const chatHub = useChatHub();

    const messageInput = useRef<any>(null);

    const [isEmptyMessage, setIsEmptyMessage] = useState(false);

    useEffect(() => {
        if (!chatHub) {
            return;
        }

        if (targetChatType === chatType["group"]) {
            chatHub.subscribeToGroupMessageDelivered(chatId);
        }
    }, []);

    const handleSendMessageByKeyAsync = async (e: any) => {
        if (e.code !== "Enter") {
            return;
        }

        await sendMessageAsync();
    }

    const handleSendMessageAsync = async () => {
        if (!chatHub || !IasGroupChatUser) {
            return;
        }

        await sendMessageAsync();
    }

    const sendMessageAsync = async () => {
        if (!chatHub || !IasGroupChatUser) {
            return;
        }

        if (messageInput.current.value === "") {
            sentEmptyMessage();

            return;
        }

        setAreLoadingOldMessages(false);

        if (targetChatType === chatType["personal"]) {
            await chatHub.personalChatMessagesHubConnection?.invoke("SendMessage", messageInput.current.value, chatId, IasGroupChatUser.id, IasGroupChatUser.username);
        }
        else {
            await chatHub.groupChatMessagesHubConnection?.invoke("SendMessage", messageInput.current.value, chatId, 0, IasGroupChatUser.id, IasGroupChatUser.username);
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