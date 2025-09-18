import { faCircle, faCircleUp, faClock, faCloudArrowUp, faEye, faFaceMeh, faUpRightFromSquare, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import * as signalR from '@microsoft/signalr';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';
import ChatMessageMenu from './ChatMessageMenu';
import ChatMessageTitle from './ChatMessageTitle';

const targetMessageStatus = {
    delivery: 0,
    delivered: 1,
    read: 2
};

interface DefaultChatMessageProps {
    chatUserAsUserId: string;
    chatUserUsername: string;
    reviewerId: string;
    messageOwnerId: string;
    message: GroupChatMessageModel | PersonalChatMessageModel;
    updateMessageAsync: (targetMessage: GroupChatMessageModel | PersonalChatMessageModel) => Promise<void>;
    subscribeToChatMessageHasBeenRead: (callback: (targetMessageId: number) => void) => void;
    hubConnection: signalR.HubConnection | null;
    lastReadMessageId?: number;
}

const DefaultChatMessage: React.FC<DefaultChatMessageProps> = ({ chatUserAsUserId, chatUserUsername, reviewerId, messageOwnerId, message, updateMessageAsync, subscribeToChatMessageHasBeenRead, hubConnection, lastReadMessageId }) => {
    const { t } = useTranslation('communication/chats/chatMessage');

    const [openMessageMenu, setOpenMessageMenu] = useState(false);
    const [editModeIsOn, setEditModeIsOn] = useState(false);
    const [targetMessage, setTargetMessage] = useState(message);

    const editMessageInput = useRef<HTMLInputElement | null>(null);

    useEffect(() => {
        subscribeToChatMessageHasBeenRead((targetMessageId: number) => {
            if (targetMessage.id !== targetMessageId) {
                return;
            }

            (async () => {
                if ("groupChatUserId" in targetMessage) {
                    await groupChatMessageHasBeenReadHandle();
                }
                else {
                    await personalChatMessageHasBeenReadHandle();
                }
            })();
        });
    }, []);

    useEffect(() => {
        if (!openMessageMenu) {
            setEditModeIsOn(false);
        }
    }, [openMessageMenu]);

    const handleUpdateMessageAsync = async () => {
        const updateForMessage = Object.assign({}, targetMessage);
        updateForMessage.message = editMessageInput.current?.value || "";
        updateForMessage.isEdited = true;

        await updateMessageAsync(updateForMessage);

        setEditModeIsOn(false);
        setOpenMessageMenu(false);
    }

    const updateMessageMarkedTypeAsync = async (type: number) => {
        const updateForMessage = Object.assign({}, targetMessage);
        updateForMessage.markedType = type;

        await updateMessageAsync(updateForMessage);
    }

    const handleOpenMessageMenu = () => {
        if (reviewerId !== messageOwnerId) {
            return;
        }

        setOpenMessageMenu((item) => !item);
    }

    const chatMessageHasBeenRead = (): void => {
        const updatedTargetMessage = Object.assign({}, targetMessage);
        updatedTargetMessage.status = targetMessageStatus["read"];

        setTargetMessage(updatedTargetMessage);
    }

    const personalChatMessageHasBeenReadHandle = async (): Promise<void> => {
        if (!hubConnection || reviewerId === messageOwnerId
            || targetMessage.status === targetMessageStatus["read"]) {
            return;
        }

        await hubConnection.invoke("SendMessageHasBeenRead", targetMessage.id);

        chatMessageHasBeenRead();
    }

    const groupChatMessageHasBeenReadHandle = async (): Promise<void> => {
        if (!hubConnection || reviewerId === messageOwnerId) {
            return;
        }

        await hubConnection.invoke("SendMessageHasBeenRead", targetMessage.id, reviewerId);

        chatMessageHasBeenRead();
    }

    const chatMessageHasBeenReadHandle = async () => {
        if ("groupChatUserId" in targetMessage) {
            await groupChatMessageHasBeenReadHandle();
        }
        else {
            await personalChatMessageHasBeenReadHandle();
        }
    }

    const getMessageStatus = () => {
        switch (targetMessage.status) {
            case targetMessageStatus["delivery"]:
                return <FontAwesomeIcon
                    icon={faClock}
                    className="status"
                    title={t("Delivery") || ""}
                />;
            case targetMessageStatus["delivered"]:
                return <FontAwesomeIcon
                    icon={faCircleUp}
                    className="status"
                    title={t("Delivered") || ""}
                />;
            case targetMessageStatus["read"]:
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

    const getMessage = () => {
        let isAlreadyRead = targetMessage.status === targetMessageStatus["read"];
        if ("groupChatUserId" in targetMessage && reviewerId !== targetMessage.groupChatUserId && lastReadMessageId) {
            isAlreadyRead = targetMessage.id <= lastReadMessageId;
        }
        else if ("groupChatUserId" in targetMessage && reviewerId !== targetMessage.groupChatUserId && !lastReadMessageId) {
            isAlreadyRead = false;
        }

        return (
            <div className={`text-of-message${isAlreadyRead ? "__read" : "__unread"} link 
                            ${targetMessage.markedType === 1 ? 'not-relevant' : targetMessage.markedType === 2 ? 'with-emotions' : ''}
                            ${openMessageMenu ? 'menu' : ''} `}
                            onMouseEnter={chatMessageHasBeenReadHandle}>
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
        );
    }

    return (
        <div className={`chat-messages__content${reviewerId === messageOwnerId ? ' my-message' : ''}`}>
            <ChatMessageTitle
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
                        : targetMessage.status === targetMessageStatus["delivered"] &&
                        <FontAwesomeIcon
                            icon={faCircle}
                            className="status"
                            title={t("Delivered") || ""}
                        />
                    }
                    {getMessage()}
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

export default DefaultChatMessage;