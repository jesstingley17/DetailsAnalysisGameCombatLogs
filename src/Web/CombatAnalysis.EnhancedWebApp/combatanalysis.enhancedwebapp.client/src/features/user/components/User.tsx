import logger from '@/utils/Logger';
import { faCircleXmark, faUser } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState, type JSX, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyGetUserByIdQuery } from '../api/Account.api';
import { useRemoveFriendAsyncMutation } from '../api/Friend.api';
import UserInformation from './UserInformation';

import './User.scss';

interface UserProps {
    targetUserId: string;
    setUserInformation: (value: SetStateAction<JSX.Element | null>) => void;
    targetUsername: string | "";
    friendId?: number | 0;
}

const User: React.FC<UserProps> = ({ targetUserId, setUserInformation, targetUsername = "", friendId = 0 }) => {
    const { t } = useTranslation('communication/myEnvironment/friends');

    const [getUserById] = useLazyGetUserByIdQuery();
    const [removeFriend] = useRemoveFriendAsyncMutation();

    const [userActive, setUserActive] = useState("");
    const [username, setUsername] = useState(targetUsername);

    useEffect(() => {
        if (targetUserId && (!targetUsername || targetUsername.length === 0)) {
            const getUsernameByUserId = async () => {
                await getUsernameByUserIdAsync();
            }

            getUsernameByUserId();
        }
    }, []);

    const getUsernameByUserIdAsync = async () => {
        try {
            const user = await getUserById(targetUserId).unwrap();
            console.log(user);
            setUsername(user.username);
        } catch (e) {
            logger.error(`Failed to get username for user: ${targetUserId}`, e);
        }
    }

    const removeFriendAsync = async () => {
        await removeFriend(friendId);
    }

    const openUserInformation = () => {
        setUserInformation(
            <UserInformation
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
            <div className="username" title={username}>{username}</div>
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