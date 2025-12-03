import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import type { SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';

interface ChatMessageMenuProps {
    message: PersonalChatMessageModel | GroupChatMessageModel;
    setEditModeIsOn: (value: SetStateAction<boolean>) => void;
    setOpenMessageMenu: (value: SetStateAction<boolean>) => void;
    updateMessageMarkedTypeAsync: (type: number) => Promise<void>;
}

const ChatMessageMenu: React.FC<ChatMessageMenuProps> = ({ message, setEditModeIsOn, setOpenMessageMenu, updateMessageMarkedTypeAsync }) => {
    const { t } = useTranslation('communication/chats/chatMessage');

    const handleMarkAsNotRelevant = async () => {
        if (message.markedType === 1) {
            await updateMessageMarkedTypeAsync(0);
        } else {
            await updateMessageMarkedTypeAsync(1);
        }
    }

    const handleMarkAsWithEmotions = async () => {
        if (message.markedType === 2) {
            await updateMessageMarkedTypeAsync(0);
        } else {
            await updateMessageMarkedTypeAsync(2);
        }
    }

    return (
        <div className="message-menu">
            <FontAwesomeIcon
                icon={faXmark}
                onClick={() => setOpenMessageMenu(false)}
            />
            <ul className="message-menu__items">
                <li onClick={() => setEditModeIsOn((item) => !item)}>{t("Edit")}</li>
                {message.markedType === 1
                    ? <li onClick={handleMarkAsNotRelevant}>{t("UnmarkNotRelevant")}</li>
                    : <li onClick={handleMarkAsNotRelevant}>{t("MarkNotRelevant")}</li>
                }
                {message.markedType === 2
                    ? <li onClick={handleMarkAsWithEmotions}>{t("UnmarkWithEmotions")}</li>
                    : <li onClick={handleMarkAsWithEmotions}>{t("MarkWithEmotions")}</li>
                }
            </ul>
        </div>
    );
}

export default ChatMessageMenu;