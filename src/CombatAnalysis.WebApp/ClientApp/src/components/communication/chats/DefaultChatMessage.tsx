import { faCircle, faCircleUp, faClock, faCloudArrowUp, faEye, faFaceMeh, faUpRightFromSquare, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { DefaultChatMessageProps } from '../../../types/components/communication/chats/DefaultChatMessageProps';
import ChatMessageMenu from './ChatMessageMenu';
import ChatMessageTitle from './ChatMessageTitle';

import '../../../styles/communication/chats/chatMessage.scss';

const chatStatus = {
    delivery: 0,
    delivered: 1,
    read: 2
};

const DefaultChatMessage: React.FC<DefaultChatMessageProps> = ({ me, meInChatId, reviewerId, messageOwnerId, message, updateMessageAsync, chatMessagesHubConnection, subscribeToMessageHasBeenRead }) => {
    const { t } = useTranslation("communication/chats/chatMessage");

    const [targetMessage, setTargetMessage] = useState(message);
    const [openMessageMenu, setOpenMessageMenu] = useState(false);
    const [editModeIsOn, setEditModeIsOn] = useState(false);

    const editMessageInput = useRef<HTMLInputElement | null>(null);

    useEffect(() => {
        subscribeToMessageHasBeenRead(message.chatId, reviewerId);
    }, []);

    useEffect(() => {
        if (!openMessageMenu) {
            setEditModeIsOn(false);
        }
    }, [openMessageMenu]);

    const handleUpdateMessageAsync = async () => {
        const updateForMessage = Object.assign({}, message);
        updateForMessage.message = editMessageInput.current?.value || "";
        updateForMessage.isEdited = true;

        await updateMessageAsync(updateForMessage);

        setEditModeIsOn(false);
        setOpenMessageMenu(false);
    }

    const updateMessageMarkedTypeAsync = async (type: number) => {
        const updateForMessage = Object.assign({}, message);
        updateForMessage.markedType = type;

        await updateMessageAsync(updateForMessage);
    }

    const updateMessageStatusAsync = async () => {
        if (reviewerId === messageOwnerId || targetMessage.status === chatStatus["read"]) {
            return;
        }

        await chatMessagesHubConnection?.invoke("SendMessageHasBeenRead", message.id, reviewerId);

        const updatedChat = Object.assign({}, targetMessage);
        updatedChat.status = 2;
        setTargetMessage(updatedChat);
    }

    const handleOpenMessageMenu = () => {
        if (reviewerId !== messageOwnerId) {
            return;
        }

        setOpenMessageMenu((item) => !item);
    }

    const getMessageStatus = () => {
        switch (message.status) {
            case chatStatus["delivery"]:
                return <FontAwesomeIcon
                    icon={faClock}
                    className="status"
                    title={t("Delivery") || ""}
                />;
            case chatStatus["delivered"]:
                return <FontAwesomeIcon
                    icon={faCircleUp}
                    className="status"
                    title={t("Delivered") || ""}
                />;
            case chatStatus["read"]:
                return <FontAwesomeIcon
                    icon={faEye}
                    className="status"
                    title={t("Read") || ""}
                />;
            default:
                return <FontAwesomeIcon
                    icon={faClock}
                    className="status"
                    title={t("Delivery") || ""}
                />;
        }
    }

    const openLink = (url: string) => {
        window.open(url, "_blink");
    }

    return (
        <div className={`chat-messages__content${reviewerId === messageOwnerId ? ' my-message' : ''}`}>
            <ChatMessageTitle
                me={me}
                itIsMe={reviewerId !== messageOwnerId}
                message={message}
                meInChatId={meInChatId}
            />
            {editModeIsOn && reviewerId === messageOwnerId
                ? <div className="edit-message">
                    <input className="form-control" type="text" defaultValue={message.message} ref={editMessageInput} />
                    <FontAwesomeIcon
                        icon={faCloudArrowUp}
                        title={t("Save") || ""}
                        onClick={handleUpdateMessageAsync}
                    />
                </div>
                : <div className="message">
                    {reviewerId === messageOwnerId
                        ? getMessageStatus()
                        : targetMessage.status === chatStatus["delivered"] &&
                        <FontAwesomeIcon
                            icon={faCircle}
                            className="status"
                            title={t("Delivered") || ""}
                        />
                    }
                    <div className={`text-of-message${targetMessage.status !== chatStatus["read"] ? "__unread" : "__read"} link 
                            ${message.markedType === 1 ? 'not-relevant' : message.markedType === 2 ? 'with-emotions' : ''}
                            ${openMessageMenu ? 'menu' : ''} `}
                            onMouseOver={updateMessageStatusAsync}>
                        <div className="content" onClick={handleOpenMessageMenu}>{message?.message}</div>
                        {message?.message.startsWith("http") &&
                            <FontAwesomeIcon
                                icon={faUpRightFromSquare}
                                className="open-link"
                                onClick={() => openLink(message?.message)}
                                title={t("OpenLink") || ""}
                            />
                        }
                    </div>
                    {message.markedType === 1 &&
                        <FontAwesomeIcon
                            icon={faXmark}
                            className="not-relevant"
                            title={t("NotRelevant") || ""}
                        />
                    }
                    {message.markedType === 2 &&
                        <FontAwesomeIcon
                            icon={faFaceMeh}
                            className="emotions"
                            title={t("WithEmotions") || ""}
                        />
                    }
                </div>
            }
            {message?.isEdited &&
                <div className="chat-messages__edited">{t("Edited") || ""}</div>
            }
            {openMessageMenu &&
                <ChatMessageMenu
                    message={message}
                    setEditModeIsOn={setEditModeIsOn}
                    setOpenMessageMenu={setOpenMessageMenu}
                    updateMessageMarkedTypeAsync={updateMessageMarkedTypeAsync}
                />
            }
        </div>
    );
}

export default DefaultChatMessage;