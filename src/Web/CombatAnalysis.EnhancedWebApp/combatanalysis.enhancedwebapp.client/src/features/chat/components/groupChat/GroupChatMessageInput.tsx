import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useRef, useState, type SetStateAction } from 'react';
import type { PersonalChatModel } from '../../types/PersonalChatModel';

interface GroupChatMessageInputProps {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    hubConnection: any;
    chat: PersonalChatModel;
    meId: string;
    setAreLoadingOldMessages(value: SetStateAction<boolean>): void;
    t(key: string): string;
}

const GroupChatMessageInput: React.FC<GroupChatMessageInputProps> = ({ hubConnection, chat, meId, setAreLoadingOldMessages, t }) => {
    const messageInput = useRef<HTMLInputElement | null>(null);

    const [isEmptyMessage, setIsEmptyMessage] = useState<boolean | null>(null);

    const handleSendMessageByKeyAsync = async (code: string) => {
        if (code !== "Enter" || !messageInput?.current) {
            return;
        }
        else if (messageInput.current.value === "") {
            sentEmptyMessage();

            return;
        }

        setAreLoadingOldMessages(false);

        await hubConnection?.invoke("SendMessage", messageInput.current.value, chat.id, meId);

        messageInput.current.value = "";
    }

    const handleSendMessageAsync = async () => {
        if (!messageInput.current || messageInput.current.value === "") {
            sentEmptyMessage();

            return;
        }

        setAreLoadingOldMessages(false);

        await hubConnection?.invoke("SendMessage", messageInput.current.value, chat.id, meId);

        messageInput.current.value = "";
    }

    const sentEmptyMessage = () => {
        setIsEmptyMessage(true);

        setTimeout(() => {
            setIsEmptyMessage(false);
        }, 4000);
    }

    return (
        <div className="send-message">
            <div className={`empty-message${isEmptyMessage ? "_show" : ""}`}>{t("CanNotSendEmpty")}</div>
            <div className="form-group input-message">
                <input type="text" className="form-control" placeholder={t("TypeYourMessage")}
                    ref={messageInput} onKeyDown={async (e) => await handleSendMessageByKeyAsync(e.code)} />
                <FontAwesomeIcon
                    icon={faPaperPlane}
                    title={t("SendMessage")}
                    onClick={handleSendMessageAsync}
                />
            </div>
        </div>
    );
}

export default memo(GroupChatMessageInput);