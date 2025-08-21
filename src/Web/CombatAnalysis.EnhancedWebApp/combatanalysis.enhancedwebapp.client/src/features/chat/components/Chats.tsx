import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useLocation } from 'react-router-dom';
import type { RootState } from '../../../app/Store';
import CommunicationMenu from '../../../shared/components/CommunicationMenu';
import Loading from '../../../shared/components/Loading';
import { useLazyGetGroupChatByIdQuery } from '../api/GroupChat.api';
import { useLazyGetPersonalChatByIdQuery } from '../api/PersonalChat.api';
import type { PersonalChatModel } from '../types/PersonalChatModel';
import type { SelectedChatModel } from '../types/SelectedChatModel';
import CreateGroupChat from './create/CreateGroupChat';
import GroupChat from './groupChat/GroupChat';
import GroupChatList from './groupChat/GroupChatList';
import PersonalChat from './personalChat/PersonalChat';
import PersonalChatList from './personalChat/PersonalChatList';

import './Chats.scss';

const Chats: React.FC = () => {
    const { t } = useTranslation("communication/chats/chats");

    const location = useLocation();

    const myself = useSelector((state: RootState) => state.user.value);

    const [getPersonalChatByIdAsync] = useLazyGetPersonalChatByIdQuery();
    const [getGroupChatByIdAsync] = useLazyGetGroupChatByIdQuery();

    const [selectedChat, setSelectedChat] = useState<SelectedChatModel>({ type: null, chat: null });
    const [personalChatsHidden, setPersonalChatsHidden] = useState(false);
    const [groupChatsHidden, setGroupChatsHidden] = useState(false);
    const [showCreateGroupChat, setShowCreateGroupChat] = useState(false);

    const maxWidth = 425;
    const screenSize = useMemo(() => ({
        width: window.innerWidth,
        height: window.innerHeight
    }), []);

    useEffect(() => {
        const checkSelectedChat = () => {
            const searchParams = new URLSearchParams(location.search);
            if (searchParams.get("personal") !== null) {
                const getPersonalChatById = async () => {
                    const id = parseInt(searchParams.get("personal") ?? "1");
                    const chat = await getPersonalChatByIdAsync(id).unwrap();
                    setSelectedChat({ type: "personal", chat: chat });
                }
  
                getPersonalChatById();
            } else if (searchParams.get("group") !== null) {
                const getPersonalChatById = async () => {
                    const id = parseInt(searchParams.get("group") ?? "1");
                    const chat = await getGroupChatByIdAsync(id).unwrap();
                    setSelectedChat({ type: "group", chat: chat });
                }

                getPersonalChatById();
            }
        }

        checkSelectedChat();
    }, []);

    const getCompanionId = (chat: PersonalChatModel | null) => {
        if (!chat) {
            return "1";
        }

        const id = chat.initiatorId === myself?.id ? chat.companionId : chat.initiatorId;

        return id;
    }

    if (!myself) {
        return (
            <>
                <CommunicationMenu
                    currentMenuItem={1}
                    hasSubMenu={false}
                />
                <Loading />
            </>
        );
    }

    if (selectedChat && selectedChat.type && screenSize.width <= maxWidth) {
        return (
            <>
                {showCreateGroupChat &&
                    <CreateGroupChat
                        setShowCreateGroupChat={setShowCreateGroupChat}
                    />
                }
                <div className="communication-content">
                    <div className="chats">
                        <div className="chats__title">
                            <FontAwesomeIcon
                                icon={faArrowLeft}
                                onClick={() => setSelectedChat({ type: null, chat: null })}
                            />
                        </div>
                        {selectedChat.type === "group"
                            ? <GroupChat
                                chat={selectedChat.chat}
                                myself={myself}
                                setSelectedChat={setSelectedChat}
                            />
                            : (selectedChat.type === "personal")
                                ? <PersonalChat
                                    chat={selectedChat.chat}
                                    myself={myself}
                                    setSelectedChat={setSelectedChat}
                                    companionId={getCompanionId((selectedChat.chat && "initiatorId" in selectedChat.chat) ? selectedChat.chat : null)}
                                />
                                : <div className="select-chat">
                                    {t("SelectChat")} <span onClick={() => setShowCreateGroupChat(true)}>{t("Create")}</span> {t("NewChat")}
                                </div>
                        }
                    </div>
                </div>
                <CommunicationMenu
                    currentMenuItem={1}
                    hasSubMenu={false}
                />
            </>
        );
    }

    return (
        <>
            {showCreateGroupChat &&
                <CreateGroupChat
                    setShowCreateGroupChat={setShowCreateGroupChat}
                />
            }
            <div className="communication-content">
                <div className="chats">
                    <div className="chats__my-chats">
                        <GroupChatList
                            meId={myself?.id}
                            t={t}
                            selectedChat={selectedChat}
                            setSelectedChat={setSelectedChat}
                            chatsHidden={groupChatsHidden}
                            toggleChatsHidden={() => setGroupChatsHidden(prev => !prev)}
                            setShowCreateGroupChat={setShowCreateGroupChat}
                        />
                        <PersonalChatList
                            meId={myself?.id}
                            t={t}
                            selectedChat={selectedChat}
                            setSelectedChat={setSelectedChat}
                            chatsHidden={personalChatsHidden}
                            toggleChatsHidden={() => setPersonalChatsHidden(prev => !prev)}
                        />
                    </div>
                    {selectedChat.type === "group"
                        ? <GroupChat
                            chat={selectedChat.chat}
                            myself={myself}
                            setSelectedChat={setSelectedChat}
                        />
                        : selectedChat.type === "personal"
                            ? <PersonalChat
                                chat={selectedChat.chat}
                                myself={myself}
                                setSelectedChat={setSelectedChat}
                                companionId={getCompanionId((selectedChat.chat && "initiatorId" in selectedChat.chat) ? selectedChat.chat : null)}
                            />
                            : <div className="select-chat">
                                {t("SelectChat")} <span onClick={() => setShowCreateGroupChat(true)}>{t("Create")}</span> {t("NewChat")}
                            </div>
                    }
                </div>
            </div>
            <CommunicationMenu
                currentMenuItem={1}
                hasSubMenu={false}
            />
        </>
    );
}

export default memo(Chats);