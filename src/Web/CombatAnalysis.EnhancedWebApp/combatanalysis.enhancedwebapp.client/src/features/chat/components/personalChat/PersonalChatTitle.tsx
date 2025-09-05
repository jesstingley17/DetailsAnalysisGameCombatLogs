import logger from '@/utils/Logger';
import { faUserXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState, type SetStateAction } from 'react';
import { useRemovePersonalChatAsyncMutation } from '../../api/PersonalChat.api';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';

interface PersonalChatTitleProps {
    chat: PersonalChatModel;
    companionUsername: string;
    setSelectedChat: (value: SetStateAction<PersonalChatModel | GroupChatModel | null>) => void;
    haveMoreMessages: boolean;
    setHaveMoreMessage: (value: SetStateAction<boolean>) => void;
    loadMoreMessagesAsync: () => Promise<void>;
    t: (key: string) => string;
}

const PersonalChatTitle: React.FC<PersonalChatTitleProps> = ({ chat, companionUsername, setSelectedChat, haveMoreMessages, setHaveMoreMessage, loadMoreMessagesAsync, t }) => {
    const [removePersonalChatAsync] = useRemovePersonalChatAsyncMutation();

    const [showRemoveChatAlert, setShowRemoveChatAlert] = useState(false);

    const leaveFromChatAsync = async () => {
        try {
            await removePersonalChatAsync(chat.id).unwrap();
            setSelectedChat(null);
        } catch (e) {
            logger.error("Failed to remove personal chat", e);
        }
    }

    const handleLoadMoreMessagesAsync = async () => {
        setHaveMoreMessage(false);

        await loadMoreMessagesAsync();
    }

    return (
        <>
            <div className="title">
                <div className="name">{companionUsername}</div>
                <FontAwesomeIcon
                    icon={faUserXmark}
                    title={t("RemoveChat")}
                    className={`remove-chat${showRemoveChatAlert ? "_active" : ""}`}
                    onClick={() => setShowRemoveChatAlert((item) => !item)}
                />
                {showRemoveChatAlert &&
                    <div className="remove-chat-alert box-shadow">
                        <p>{t("AreYouSureRemoveChat")}</p>
                        <p>{t("ThatWillBeRemoveChat")}</p>
                        <div className="remove-chat-alert__actions">
                            <div className="btn-shadow remove" onClick={leaveFromChatAsync}>{t("Remove")}</div>
                            <div className="btn-shadow cancel" onClick={() => setShowRemoveChatAlert((item) => !item)}>{t("Cancel")}</div>
                        </div>
                    </div>
                }
            </div>
            {haveMoreMessages &&
                <div className="load-more" onClick={handleLoadMoreMessagesAsync}>Load more</div>
            }
        </>
    );
}

export default PersonalChatTitle;