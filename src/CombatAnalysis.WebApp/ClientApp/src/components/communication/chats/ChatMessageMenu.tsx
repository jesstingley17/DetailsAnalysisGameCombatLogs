import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { ChatMessageMenuProps } from '../../../types/components/communication/chats/ChatMessageMenuProps';

const ChatMessageMenu: React.FC<ChatMessageMenuProps> = ({ setEditModeIsOn, setOpenMessageMenu }) => {
    const { t } = useTranslation("communication/chats/chatMessage");

    return (
        <div className="message-menu">
            <FontAwesomeIcon
                icon={faXmark}
                onClick={() => setOpenMessageMenu(false)}
            />
            <ul className="message-menu__items">
                <li onClick={() => setEditModeIsOn((item) => !item)}>Edit</li>
                <li>Mark as "Not relevant"</li>
                <li>Mark as "With emotions"</li>
            </ul>
        </div>
    );
}

export default ChatMessageMenu;