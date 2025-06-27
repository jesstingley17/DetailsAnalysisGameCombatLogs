import { faCircle, faCircleUp, faClock, faCloudArrowUp, faEye, faFaceMeh, faUpRightFromSquare, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { DefaultChatMessageProps } from '../../../types/components/communication/chats/DefaultChatMessageProps';
import ChatMessageMenu from './ChatMessageMenu';
import ChatMessageTitle from './ChatMessageTitle';

import '../../../styles/communication/chats/chatMessage.scss';

const messageStatus = {
    delivery: 0,
    delivered: 1,
    read: 2
};

const DefaultGroupChatMessage: React.FC<DefaultChatMessageProps> = ({ myself, chatUserAsUserId, chatUserUsername, reviewerId, messageOwnerId, message, updateMessageAsync, hubConnection, subscribeToChatMessageHasBeenRead }) => {
    const { t } = useTranslation("communication/chats/chatMessage");

    const [targetMessage, setTargetMessage] = useState(message);
    const [openMessageMenu, setOpenMessageMenu] = useState(false);
    const [editModeIsOn, setEditModeIsOn] = useState(false);

    const editMessageInput = useRef<HTMLInputElement | null>(null);

    useEffect(() => {
        subscribeToChatMessageHasBeenRead((messageId: number) => {
            if (message.id !== messageId) {
                return;
            }

            const updatedTargetMessage = Object.assign({}, targetMessage);
            updatedTargetMessage.groupChatMessageId = undefined;
            setTargetMessage(updatedTargetMessage);
        });
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

    const messageHasBeenReadHandle = async () => {
        if (!hubConnection || reviewerId === messageOwnerId || !targetMessage.groupChatMessageId) {
            return;
        }

        await hubConnection?.invoke("SendMessageHasBeenRead", targetMessage.id, reviewerId);
    }

    const handleOpenMessageMenu = () => {
        if (reviewerId !== messageOwnerId) {
            return;
        }

        setOpenMessageMenu((item) => !item);
    }

    const getMessageStatus = () => {
        switch (message.status) {
            case messageStatus["delivery"]:
                return <FontAwesomeIcon
                    icon={faClock}
                    className="status"
                    title={t("Delivery") || ""}
                />;
            case messageStatus["delivered"]:
                return <FontAwesomeIcon
                    icon={faCircleUp}
                    className="status"
                    title={t("Delivered") || ""}
                />;
            case messageStatus["read"]:
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
                myself={myself}
                itIsMe={reviewerId !== messageOwnerId}
                message={targetMessage}
                chatUserAsUserId={chatUserAsUserId}
                chatUserUsername={chatUserUsername}
            />
            {editModeIsOn && reviewerId === messageOwnerId
                ? <div className="edit-message">
                    <input className="form-control" type="text" defaultValue={targetMessage.message} ref={editMessageInput} />
                    <FontAwesomeIcon
                        icon={faCloudArrowUp}
                        title={t("Save") || ""}
                        onClick={handleUpdateMessageAsync}
                    />
                </div>
                : <div className="message">
                    {reviewerId === messageOwnerId
                        ? getMessageStatus()
                        : targetMessage.status === messageStatus["delivered"] &&
                        <FontAwesomeIcon
                            icon={faCircle}
                            className="status"
                            title={t("Delivered") || ""}
                        />
                    }
                    <div className={`text-of-message${!targetMessage.groupChatMessageId ? "__read" : "__unread"} link 
                            ${targetMessage.markedType === 1 ? 'not-relevant' : targetMessage.markedType === 2 ? 'with-emotions' : ''}
                            ${openMessageMenu ? 'menu' : ''} `}
                            onMouseOver={messageHasBeenReadHandle}>
                        <div className="content" onClick={handleOpenMessageMenu}>{targetMessage?.message}</div>
                        {targetMessage?.message.startsWith("http") &&
                            <FontAwesomeIcon
                                icon={faUpRightFromSquare}
                                className="open-link"
                                onClick={() => openLink(targetMessage?.message)}
                                title={t("OpenLink") || ""}
                            />
                        }
                    </div>
                    {targetMessage.markedType === 1 &&
                        <FontAwesomeIcon
                            icon={faXmark}
                            className="not-relevant"
                            title={t("NotRelevant") || ""}
                        />
                    }
                    {targetMessage.markedType === 2 &&
                        <FontAwesomeIcon
                            icon={faFaceMeh}
                            className="emotions"
                            title={t("WithEmotions") || ""}
                        />
                    }
                </div>
            }
            {targetMessage?.isEdited &&
                <div className="chat-messages__edited">{t("Edited") || ""}</div>
            }
            {openMessageMenu &&
                <ChatMessageMenu
                    message={targetMessage}
                    setEditModeIsOn={setEditModeIsOn}
                    setOpenMessageMenu={setOpenMessageMenu}
                    updateMessageMarkedTypeAsync={updateMessageMarkedTypeAsync}
                />
            }
        </div>
    );
}

export default DefaultGroupChatMessage;