import { faCircleXmark, faUser } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import { useRemoveFriendAsyncMutation } from '../api/Friend.api';
import type { AppUserModel } from '../types/AppUserModel';
import UserInformation from './UserInformation';

import './User.scss';

interface UserProps {
    myself: AppUserModel;
    targetUserId: string;
    targetUsername: string;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    setUserInformation: (value: SetStateAction<any>) => void;
    friendId?: number | 0;
}

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

    const userActiveHandler = () => {
        setUserActive("_active");
    }

    const userInactiveHandler = () => {
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