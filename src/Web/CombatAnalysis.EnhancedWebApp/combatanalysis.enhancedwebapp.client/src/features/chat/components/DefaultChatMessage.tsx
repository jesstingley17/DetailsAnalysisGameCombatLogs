import { faCircle, faCircleUp, faClock, faCloudArrowUp, faEye, faFaceMeh, faUpRightFromSquare, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import type { AppUserModel } from '../../user/types/AppUserModel';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';
import ChatMessageMenu from './ChatMessageMenu';
import ChatMessageTitle from './ChatMessageTitle';

const messageStatus = {
    delivery: 0,
    delivered: 1,
    read: 2
};

interface DefaultChatMessageProps {
    myself: AppUserModel;
    chatUserAsUserId: string;
    chatUserUsername: string;
    reviewerId: string;
    messageOwnerId: string;
    message: GroupChatMessageModel | PersonalChatMessageModel;
    updateMessageAsync: (message: GroupChatMessageModel | PersonalChatMessageModel) => Promise<void>;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    subscribeToChatMessageHasBeenRead: (callback: any) => void;
    chatMessageHasMessageBeenRead: () => void;
    messageHasBeenReadHandle: () => Promise<void>;
}

const DefaultChatMessage: React.FC<DefaultChatMessageProps> = ({ myself, chatUserAsUserId, chatUserUsername, reviewerId, messageOwnerId, message, updateMessageAsync, subscribeToChatMessageHasBeenRead, chatMessageHasMessageBeenRead, messageHasBeenReadHandle }) => {
    const { t } = useTranslation("communication/chats/chatMessage");

    const [openMessageMenu, setOpenMessageMenu] = useState(false);
    const [editModeIsOn, setEditModeIsOn] = useState(false);

    const editMessageInput = useRef<HTMLInputElement | null>(null);

    useEffect(() => {
        subscribeToChatMessageHasBeenRead((messageId: number) => {
            if (message.id !== messageId) {
                return;
            }

            chatMessageHasMessageBeenRead();
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
                        : message.status === messageStatus["delivered"] &&
                        <FontAwesomeIcon
                            icon={faCircle}
                            className="status"
                            title={t("Delivered") || ""}
                        />
                    }
                    <div className={`text-of-message${message.status !== messageStatus["read"] ? "__unread" : "__read"} link 
                            ${message.markedType === 1 ? 'not-relevant' : message.markedType === 2 ? 'with-emotions' : ''}
                            ${openMessageMenu ? 'menu' : ''} `}
                            onMouseOver={message.status !== messageStatus["read"] ? messageHasBeenReadHandle : () => { }}>
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