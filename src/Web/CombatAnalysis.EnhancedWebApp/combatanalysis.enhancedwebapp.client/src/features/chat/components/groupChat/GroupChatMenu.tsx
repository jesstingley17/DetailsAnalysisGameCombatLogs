import VerificationRestriction from '@/shared/components/VerificationRestriction';
import { useChatHub } from '@/shared/hooks/useChatHub';
import logger from '@/utils/Logger';
import { useEffect, useState, type SetStateAction } from 'react';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useRemoveGroupChatMutation } from '../../api/GroupChat.api';
import { useGetGroupChatRulesByChatIdQuery, useUpdateGroupChatRulesAsyncMutation } from '../../api/GroupChatRules.api';
import {
    useRemoveGroupChatUserAsyncMutation
} from '../../api/GroupChatUser.api';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { GroupChatUserModel } from '../../types/GroupChatUserModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';
import ChatRulesItem from '../create/ChatRulesItem';
import GroupChatAddUser from './GroupChatAddUser';
import GroupChatMembers from './GroupChatMembers';

const rulesEnum = {
    owner: 0,
    anyone: 1
};

const defaultPayload = {
    invitePeople: 0,
    removePeople: 0,
    pinMessage: 1,
    announcements: 1,
};

interface GroupChatMenuProps {
    myself: AppUserModel;
    setSelectedChat: (value: SetStateAction<PersonalChatModel | GroupChatModel | null>) => void;
    groupChatUsersId: string[];
    IasGroupChatUser: GroupChatUserModel;
    chat: GroupChatModel;
    t: (key: string) => string;
}

const GroupChatMenu: React.FC<GroupChatMenuProps> = ({ myself, setSelectedChat, groupChatUsersId, IasGroupChatUser, chat, t }) => {
    const [showAddPeople, setShowAddPeople] = useState(false);
    const [peopleInspectionModeOn, setPeopleInspectionModeOn] = useState(false);
    const [rulesInspectionModeOn, setRulesInspectionModeOn] = useState(false);
    const [showRemoveChatAlert, setShowRemoveChatAlert] = useState(false);
    const [invitePeople, setInvitePeople] = useState(0);
    const [removePeople, setRemovePeople] = useState(0);
    const [pinMessage, setPinMessage] = useState(1);
    const [announcements, setAnnouncements] = useState(1);
    const [payload, setPayload] = useState(defaultPayload);

    const chatHub = useChatHub();

    const [removeGroupChatAsync] = useRemoveGroupChatMutation();
    const [removeGroupChatUserAsyncMut] = useRemoveGroupChatUserAsyncMutation();
    const [updateGroupChatRulesMutAsync] = useUpdateGroupChatRulesAsyncMutation();

    const { data: rules, isLoading } = useGetGroupChatRulesByChatIdQuery(chat.id);

    useEffect(() => {
        if (!rules) {
            return;
        }

        setPayload({
            invitePeople: rules.invitePeople,
            removePeople: rules.removePeople,
            pinMessage: rules.pinMessage,
            announcements: rules.announcements,
        });
    }, [rules])

    const removeGroupChatUsersAsync = async (peopleToRemove: GroupChatUserModel[]) => {
        try {
            if (!chatHub || !chatHub.groupChatHubConnectionRef.current) {
                return;
            }

            for (let i = 0; i < peopleToRemove.length; i++) {
                await chatHub.groupChatHubConnectionRef.current.invoke("RemoveUserFromChat", chat.id, peopleToRemove[i].id, peopleToRemove[i].username);
            }

            setPeopleInspectionModeOn(false);
        } catch (e) {
            logger.error("Failed to remove group chat users", e);
        }
    }

    const leaveFromChatAsync = async (groupChatUserId: string) => {
        try {
            await removeGroupChatUserAsyncMut(groupChatUserId).unwrap();
            setSelectedChat(null);
        } catch (e) {
            logger.error("Failed to leave from group chat", e);
        }
    }

    const removeChatAsync = async () => {
        try {
            await removeGroupChatAsync(chat.id);
            setSelectedChat(null);
        } catch (e) {
            logger.error("Failed to remove group chat", e);
        }
    }

    const updateGroupChatRulesAsync = async () => {
        if (!rules) {
            return;
        }

        try {
            const groupChatRules = {
                id: rules.id,
                invitePeople: invitePeople,
                removePeople: removePeople,
                pinMessage: pinMessage,
                announcements: announcements,
                groupChatId: rules?.groupChatId ?? 0
            };

            await updateGroupChatRulesMutAsync({ id: rules.id, groupChatRules }).unwrap();
            setRulesInspectionModeOn((item) => !item);
        } catch (e) {
            logger.error("Failed to update group chat rules", e);
        }
    }

    const canInvitePeople = (): boolean => {
        const canAnyone = rules?.invitePeople === rulesEnum["anyone"];
        if (canAnyone) {
            return true;
        }

        return chat?.ownerId === myself?.id;
    }

    const canRemovePeople = (): boolean => {
        const canAnyone = rules?.removePeople === rulesEnum["anyone"];
        if (canAnyone) {
            return true;
        }

        return chat?.ownerId === myself?.id;
    }

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <>
            <div className="settings__content">
                <div className="main-settings">
                    <div className="btn-border-shadow" onClick={() => setPeopleInspectionModeOn((item) => !item)}>{t("Members")}</div>
                    {canInvitePeople() &&
                        <div className="btn-border-shadow" onClick={() => setShowAddPeople((item) => !item)}>{t("Invite")}</div>
                    }
                    {chat.ownerId === myself.id &&
                        <div className="btn-border-shadow" onClick={() => setRulesInspectionModeOn((item) => !item)}>{t("Rules")}</div>
                    }
                    <div className="btn-border-shadow">{t("Documents")}</div>
                </div>
                <div className="danger-settings">
                    {myself.id === chat.ownerId &&
                        <div className="btn-border-shadow" onClick={() => setShowRemoveChatAlert((item) => !item)}>{t("RemoveChat")}</div>
                    }
                    {myself.id === chat.ownerId
                        ? <VerificationRestriction
                            contentText={t("Leave")}
                            infoText={t("YouShouldTransferRights")}
                        />
                        : <div className="btn-border-shadow" onClick={async () => await leaveFromChatAsync(IasGroupChatUser?.id ?? "")}>{t("Leave")}</div>
                    }
                </div>
            </div>
            {showAddPeople &&
                <GroupChatAddUser
                    myself={myself}
                    chatId={chat?.id}
                    groupChatUsersId={groupChatUsersId}
                    setShowAddPeople={setShowAddPeople}
                    chatHub={chatHub}
                    t={t}
                />
            }
            {peopleInspectionModeOn &&
                <GroupChatMembers
                    myself={myself}
                    communicationId={chat.id}
                    removeUsersAsync={removeGroupChatUsersAsync}
                    setShowMembers={setPeopleInspectionModeOn}
                    isPopup={true}
                    canRemovePeople={canRemovePeople}
                    chatHub={chatHub}
                />
            }
            {rulesInspectionModeOn &&
                <div className="rules-container box-shadow">
                    <ChatRulesItem
                        setInvitePeople={setInvitePeople}
                        setRemovePeople={setRemovePeople}
                        setPinMessage={setPinMessage}
                        setAnnouncements={setAnnouncements}
                        payload={payload}
                        t={t}
                    />
                    <div className="item-result">
                        <div className="btn-border-shadow save" onClick={updateGroupChatRulesAsync}>{t("SaveChanges")}</div>
                        <div className="btn-border-shadow" onClick={() => setRulesInspectionModeOn((item) => !item)}>{t("Cancel")}</div>
                    </div>
                </div>
            }
            {showRemoveChatAlert &&
                <div className="remove-chat-alert box-shadow">
                    <p>{t("AreYouSureRemoveChat")}</p>
                    <p>{t("ThatWillBeRemoveChat")}</p>
                    <div className="remove-chat-alert__actions">
                        <div className="btn-border-shadow remove" onClick={removeChatAsync}>{t("Remove")}</div>
                        <div className="btn-border-shadow cancel" onClick={() => setShowRemoveChatAlert((item) => !item)}>{t("Cancel")}</div>
                    </div>
                </div>
            }
        </>
    );
}

export default GroupChatMenu;