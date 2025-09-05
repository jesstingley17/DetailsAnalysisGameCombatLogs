import type { RootState } from '@/app/Store';
import { useChatHub } from '@/shared/hooks/useChatHub';
import { useRef, useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import ChatRulesItem from './ChatRulesItem';

import './Create.scss';

const payload = {
    invitePeople: 0,
    removePeople: 0,
    pinMessage: 1,
    announcements: 1,
};

const CreateGroupChat: React.FC<{ setShowCreateGroupChat: (value: SetStateAction<boolean>) => void; }> = ({ setShowCreateGroupChat }) => {
    const { t } = useTranslation('communication/create');

    const chatHub = useChatHub();

    const me = useSelector((state: RootState) => state.user.value);

    const chatNameRef = useRef<HTMLInputElement | null>(null);

    const [chatName, setChatName] = useState("");
    const [invitePeople, setInvitePeople] = useState(0);
    const [removePeople, setRemovePeople] = useState(0);
    const [pinMessage, setPinMessage] = useState(1);
    const [announcements, setAnnouncements] = useState(1);
    const [isCreating, setIsCreating] = useState(false);

    const createGroupChatAsync = async () => {
        if (!chatHub) {
            return;
        }

        const groupChat = {
            id: 0,
            name: chatNameRef.current?.value,
            appUserId: me?.id
        };

        const groupChatUser = {
            id: " ",
            username: me?.username,
            appUserId: me?.id
        };

        const groupChatRules = {
            id: 0,
            invitePeople: invitePeople,
            removePeople: removePeople,
            pinMessage: pinMessage,
            announcements: announcements,
        };

        const container = {
            groupChat,
            groupChatUser,
            groupChatRules
        };

        await chatHub.groupChatHubConnectionRef.current?.invoke("CreateGroupChat", container);
    }

    const handleCreateNewGroupChatAsync = async () => {
        setIsCreating(true);

        await createGroupChatAsync();

        setIsCreating(false);
        setShowCreateGroupChat(false);
    }

    const chatNameChangeHandler = () => {
        if (!chatNameRef || !chatNameRef.current) {
            return;
        }

        setChatName(chatNameRef.current.value);
    }

    return (
        <div className="communication-content create-communication-object box-shadow">
            <div>{t("CreateGroupChat")}</div>
            <div className="create-communication-object__content">
                <div className="create-communication-object__item">
                    <div className="form-group">
                        <label htmlFor="name">{t("Name")}</label>
                        <input type="text" className="form-control" name="name" id="name"
                            ref={chatNameRef} onChange={chatNameChangeHandler} required />
                    </div>
                    {chatName.length === 0 &&
                        <div className="chat-name-required">{t("NameRequired")}</div>
                    }
                </div>
                <ChatRulesItem
                    setInvitePeople={setInvitePeople}
                    setRemovePeople={setRemovePeople}
                    setPinMessage={setPinMessage}
                    setAnnouncements={setAnnouncements}
                    payload={payload}
                    t={t}
                />
            </div>
            <div className="actions">
                <div className={`btn-shadow create ${chatName.length > 0 ? '' : 'can-not-finish'}`} onClick={chatName.length > 0 ? handleCreateNewGroupChatAsync : () => {}}>{t("Create")}</div>
                <div className="btn-shadow" onClick={() => setShowCreateGroupChat(false)}>{t("Cancel")}</div>
            </div>
            {isCreating &&
                <>
                    <span className="creating"></span>
                    <div className="notify">{t("Creating")}</div>
                </>
            }
        </div>
    );
}

export default CreateGroupChat;