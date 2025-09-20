import { faCircle, faCircleUp, faClock, faCloudArrowUp, faEye, faFaceMeh, faUpRightFromSquare, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import * as signalR from '@microsoft/signalr';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';
import type { GroupChatMessagePatch } from '../types/patches/GroupChatMessagePatch';
import type { PersonalChatMessagePatch } from '../types/patches/PersonalChatMessagePatch';
import ChatMessageMenu from './ChatMessageMenu';
import ChatMessageTitle from './ChatMessageTitle';

interface DefaultChatMessageProps {
    chatUserAsUserId: string;
    chatUserUsername: string;
    reviewerId: string;
    messageOwnerId: string;
    message: GroupChatMessageModel | PersonalChatMessageModel;
    updateMessageAsync: (targetMessage: GroupChatMessagePatch | PersonalChatMessagePatch) => Promise<void>;
    subscribeToChatMessageHasBeenRead: (callback: (targetMessageId: number) => void) => void;
    hubConnection: signalR.HubConnection | null;
    lastReadMessageId?: number;
}

const DefaultChatMessage: React.FC<DefaultChatMessageProps> = ({ chatUserAsUserId, chatUserUsername, reviewerId, messageOwnerId, message, updateMessageAsync, subscribeToChatMessageHasBeenRead, hubConnection, lastReadMessageId }) => {
    const { t } = useTranslation('communication/chats/chatMessage');

    const [openMessageMenu, setOpenMessageMenu] = useState(false);
    const [editModeIsOn, setEditModeIsOn] = useState(false);

    const editMessageInput = useRef<HTMLInputElement | null>(null);

    useEffect(() => {
        subscribeToChatMessageHasBeenRead((targetMessageId: number) => {
            if (message.id !== targetMessageId) {
                return;
            }

            (async () => {
                if ("groupChatUserId" in message) {
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
        const updatedMessage = {
            id: message.id,
            message: editMessageInput.current?.value,
        }

        await updateMessageAsync(updatedMessage);

        setEditModeIsOn(false);
        setOpenMessageMenu(false);
    }

    const updateMessageMarkedTypeAsync = async (type: number) => {
        const updatedMessage = {
            id: message.id,
            markedType: type,
        }

        await updateMessageAsync(updatedMessage);
    }

    const handleOpenMessageMenu = () => {
        if (reviewerId !== messageOwnerId) {
            return;
        }

        setOpenMessageMenu((item) => !item);
    }

    const personalChatMessageHasBeenReadHandle = async (): Promise<void> => {
        if (!hubConnection || reviewerId === messageOwnerId
            || message.status === "Read") {
            return;
        }

        await hubConnection.invoke("SendMessageHasBeenRead", message.id);
    }

    const groupChatMessageHasBeenReadHandle = async (): Promise<void> => {
        if (!hubConnection || reviewerId === messageOwnerId) {
            return;
        }

        await hubConnection.invoke("SendMessageHasBeenRead", message.id, reviewerId);
    }

    const chatMessageHasBeenReadHandle = async () => {
        if ("groupChatUserId" in message) {
            await groupChatMessageHasBeenReadHandle();
        }
        else {
            await personalChatMessageHasBeenReadHandle();
        }
    }

    const getMessageStatus = () => {
        switch (message.status) {
            case "Sending":
                return <FontAwesomeIcon
                    icon={faClock}
                    className="status"
                    title={t("Delivery") || ""}
                />;
            case "Sent":
                return <FontAwesomeIcon
                    icon={faCircleUp}
                    className="status"
                    title={t("Delivered") || ""}
                />;
            case "Read":
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
        let isAlreadyRead = message.status === "Read";
        if ("groupChatUserId" in message && reviewerId !== message.groupChatUserId && lastReadMessageId) {
            isAlreadyRead = message.id <= lastReadMessageId;
        }
        else if ("groupChatUserId" in message && reviewerId !== message.groupChatUserId && !lastReadMessageId) {
            isAlreadyRead = false;
        }

        return (
            <div className={`text-of-message${isAlreadyRead ? "__read" : "__unread"} link 
                            ${message.markedType === 1 ? 'not-relevant' : message.markedType === 2 ? 'with-emotions' : ''}
                            ${openMessageMenu ? 'menu' : ''} `}
                            onMouseEnter={chatMessageHasBeenReadHandle}>
                <div className="content" onClick={handleOpenMessageMenu}>{message.message}</div>
                {message.message.startsWith("http") &&
                    <FontAwesomeIcon
                        icon={faUpRightFromSquare}
                        className="open-link"
                    onClick={() => openLink(message.message)}
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
                message={message}
                chatUserAsUserId={chatUserAsUserId}
                chatUserUsername={chatUserUsername}
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
                        : message.status === "Sent" &&
                        <FontAwesomeIcon
                            icon={faCircle}
                            className="status"
                            title={t("Delivered") || ""}
                        />
                    }
                    {getMessage()}
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
            {message.isEdited &&
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