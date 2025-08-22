import logger from '@/utils/Logger';
import { useEffect, useState, type SetStateAction } from 'react';
import VerificationRestriction from '../../../../shared/components/VerificationRestriction';
import { useLazyGetUserByIdQuery } from '../../../user/api/Account.api';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useRemoveGroupChatAsyncMutation } from '../../api/GroupChat.api';
import { useCreateGroupChatMessageMutation } from '../../api/GroupChatMessage.api';
import { useGetGroupChatRulesByIdQuery, useUpdateGroupChatRulesAsyncMutation } from '../../api/GroupChatRules.api';
import {
    useRemoveGroupChatUserAsyncMutation
} from '../../api/GroupChatUser.api';
import type { GroupChatMessageModel } from '../../types/GroupChatMessageModel';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { GroupChatUserModel } from '../../types/GroupChatUserModel';
import type { SelectedChatModel } from '../../types/SelectedChatModel';
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
    setSelectedChat: (value: SetStateAction<SelectedChatModel>) => void;
    groupChatUsers: GroupChatUserModel[];
    groupChatUsersId: string[];
    IasGroupChatUser: GroupChatUserModel;
    chat: GroupChatModel;
    t: (key: string) => string;
}

const GroupChatMenu: React.FC<GroupChatMenuProps> = ({ myself, setSelectedChat, groupChatUsersId, groupChatUsers, IasGroupChatUser, chat, t }) => {
    const [showAddPeople, setShowAddPeople] = useState(false);
    const [peopleInspectionModeOn, setPeopleInspectionModeOn] = useState(false);
    const [rulesInspectionModeOn, setRulesInspectionModeOn] = useState(false);
    const [showRemoveChatAlert, setShowRemoveChatAlert] = useState(false);
    const [invitePeople, setInvitePeople] = useState(0);
    const [removePeople, setRemovePeople] = useState(0);
    const [pinMessage, setPinMessage] = useState(1);
    const [announcements, setAnnouncements] = useState(1);
    const [payload, setPayload] = useState(defaultPayload);

    const [removeGroupChatAsyncMut] = useRemoveGroupChatAsyncMutation();
    const [removeGroupChatUserAsyncMut] = useRemoveGroupChatUserAsyncMutation();
    const [createGroupChatMessageAsync] = useCreateGroupChatMessageMutation();
    const [getUserByIdAsync] = useLazyGetUserByIdQuery();
    const [updateGroupChatRulesMutAsync] = useUpdateGroupChatRulesAsyncMutation();

    const { data: rules, isLoading } = useGetGroupChatRulesByIdQuery(chat?.id);

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
            for (let i = 0; i < peopleToRemove.length; i++) {
                await removeGroupChatUserAsyncMut(peopleToRemove[i].id).unwrap();

                const user = await getUserByIdAsync(peopleToRemove[i].appUserId);
                if (user.data !== undefined) {
                    const systemMessage = `'${myself?.username}' removed '${user.data.username}' from chat`;
                    await createMessageAsync(chat?.id, systemMessage);
                }
            }

            setPeopleInspectionModeOn(false);
        } catch (e) {
            logger.error("Failed to remove group chat users", e);
        }
    }

    const createMessageAsync = async (chatId: number, message: string) => {
        const today = new Date();
        const newMessage: GroupChatMessageModel = {
            id: 0,
            username: myself.username,
            message: message,
            time: `${today.getUTCHours()}:${today.getUTCMinutes()}`,
            status: 0,
            type: 1,
            chatId: chatId,
            groupChatUserId: myself.id
        };

        await createGroupChatMessageAsync(newMessage);
    }

    const leaveFromChatAsync = async (groupChatUserId: string) => {
        try {
            await removeGroupChatUserAsyncMut(groupChatUserId).unwrap();
            setSelectedChat({ type: null, chat: null });
        } catch (e) {
            logger.error("Failed to leave from group chat", e);
        }
    }

    const removeChatAsync = async () => {
        try {
            await removeGroupChatAsyncMut(chat?.id);
            setSelectedChat({ type: null, chat: null });
        } catch (e) {
            logger.error("Failed to remove group chat", e);
        }
    }

    const updateGroupChatRulesAsync = async () => {
        try {
            const groupChatRules = {
                id: rules?.id ?? 0,
                invitePeople: invitePeople,
                removePeople: removePeople,
                pinMessage: pinMessage,
                announcements: announcements,
                chatId: rules?.chatId ?? 0
            };

            await updateGroupChatRulesMutAsync(groupChatRules).unwrap();
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

        return chat?.appUserId === myself?.id;
    }

    const canRemovePeople = (): boolean => {
        const canAnyone = rules?.removePeople === rulesEnum["anyone"];
        if (canAnyone) {
            return true;
        }

        return chat?.appUserId === myself?.id;
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
                    {chat.appUserId === myself.id &&
                        <div className="btn-border-shadow" onClick={() => setRulesInspectionModeOn((item) => !item)}>{t("Rules")}</div>
                    }
                    <div className="btn-border-shadow">{t("Documents")}</div>
                </div>
                <div className="danger-settings">
                    {myself.id === chat.appUserId &&
                        <div className="btn-border-shadow" onClick={() => setShowRemoveChatAlert((item) => !item)}>{t("RemoveChat")}</div>
                    }
                    {myself.id === chat.appUserId
                        ? <VerificationRestriction
                            contentText={t("Leave")}
                            infoText={t("YouShouldTransferRights")}
                        />
                        : <div className="btn-border-shadow" onClick={async () => await leaveFromChatAsync(IasGroupChatUser?.id)}>{t("Leave")}</div>
                    }
                </div>
            </div>
            {showAddPeople &&
                <GroupChatAddUser
                    me={myself}
                    chatId={chat?.id}
                    groupChatUsersId={groupChatUsersId}
                    setShowAddPeople={setShowAddPeople}
                    t={t}
                />
            }
            {peopleInspectionModeOn &&
                <GroupChatMembers
                    me={myself}
                    communicationUsers={groupChatUsers}
                    removeUsersAsync={removeGroupChatUsersAsync}
                    setShowMembers={setPeopleInspectionModeOn}
                    isPopup={true}
                    canRemovePeople={canRemovePeople}
                />
            }
            {peopleInspectionModeOn &&
                <GroupChatMembers
                    me={myself}
                    communicationUsers={groupChatUsers}
                    removeUsersAsync={removeGroupChatUsersAsync}
                    setShowMembers={setPeopleInspectionModeOn}
                    isPopup={true}
                    canRemovePeople={canRemovePeople}
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