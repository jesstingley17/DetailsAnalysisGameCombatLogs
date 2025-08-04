import { faCirclePlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { format } from 'date-fns';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import type { RootState } from '../../../app/Store';
import type { CombatLogGroupChatListProps } from '../../../types/components/combatDetails/gameCombatLogs/CombatLogGroupChatListProps';
import { useGetGroupChatByIdQuery } from '../../chat/api/GroupChat.api';
import { useCreateGroupChatMessageMutation } from '../../chat/api/GroupChatMessage.api';
import type { GroupChatMessageModel } from '../../chat/types/GroupChatMessageModel';

const messageType = {
    default: 0,
    system: 1,
    log: 2
};

const CombatLogGroupChatList: React.FC<CombatLogGroupChatListProps> = ({ log, chatId }) => {
    const { t } = useTranslation("combatDetails/mainInformation");

    const myself = useSelector((state: RootState) => state.user.value);
    const { data: groupChat, isLoading } = useGetGroupChatByIdQuery(chatId);

    const [createGroupChatMessageAsync] = useCreateGroupChatMessageMutation();

    const [sent, showSent] = useState(false);

    const createMessageAsync = async () => {
        const date = format(new Date(log.date), 'MM/dd/yyyy HH:mm');
        const message = `${log.id};${log.name};${date}`;

        const today = new Date();
        const newMessage: GroupChatMessageModel = {
            username: myself?.username || "",
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            type: messageType["log"],
            chatId: groupChat?.id || 0,
            appUserId: myself?.id || ""
        };

        const createdMessage = await createGroupChatMessageAsync(newMessage);
        if (createdMessage.data !== undefined) {
            showSent(true);
        }
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="chat">
            <div>{groupChat?.name}</div>
            {sent
                ? <div className="sent">
                    <div>{t("Sent")}</div>
                </div>
                : <div className="btn-shadow" title={t("SendToChat")} onClick={async () => await createMessageAsync()}>
                    <FontAwesomeIcon
                        icon={faCirclePlus}
                    />
                    <div>{t("Add")}</div>
                </div>
            }
        </div>
    );
}

export default CombatLogGroupChatList;