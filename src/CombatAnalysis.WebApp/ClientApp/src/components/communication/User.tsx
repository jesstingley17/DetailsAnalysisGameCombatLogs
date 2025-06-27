import { faCircleXmark, faUser } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useRemoveFriendAsyncMutation } from '../../store/api/user/Friend.api';
import { UserProps } from '../../types/components/communication/UserProps';
import UserInformation from './UserInformation';

import '../../styles/communication/user.scss';

const User: React.FC<UserProps> = ({ myself, targetUserId, targetUsername, setUserInformation, friendId = 0 }) => {
    const { t } = useTranslation("communication/myEnvironment/friends");

    const [removeFriend] = useRemoveFriendAsyncMutation();

    const [userActive, setUserActive] = useState("");

    const removeFriendAsync = async () => {
        await removeFriend(friendId);
    }

    const openUserInformation = () => {
        setUserInformation(
            <UserInformation
                myself={myself}
                personId={targetUserId}
                closeUserInformation={closeUserInformation}
            />
        );
    }

    const userActiveHandler = (e: any) => {
        setUserActive("_active");
    }

    const userInactiveHandler = (e: any) => {
        setUserActive("");
    }

    const closeUserInformation = () => {
        setUserInformation(null);
    }

    return (
        <div className="special-user__another"
            onMouseOver={userActiveHandler}
            onMouseLeave={userInactiveHandler}>
            <FontAwesomeIcon
                icon={faUser}
                title={t("ShowDetails") || ""}
                className={`details${userActive}`}
                onClick={openUserInformation}
            />
            <div className="username" title={targetUsername}>{targetUsername}</div>
            {friendId > 0 &&
                <FontAwesomeIcon
                    icon={faCircleXmark}
                    title={t("Remove") || ""}
                    className="remove"
                    onClick={removeFriendAsync}
                />
            }
        </div>
    );
}

export default User;