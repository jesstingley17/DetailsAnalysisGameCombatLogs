import { faCircleXmark, faCommentDots, faPersonCircleQuestion, faSquarePlus, faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useChatHub } from '../../../shared/hooks/useChatHub';
import { useLazyIsExistQuery } from '../../chat/api/PersonalChat.api';
import { useGetUserByIdQuery } from '../api/Account.api';
import { useFriendSearchMyFriendsQuery } from '../api/Friend.api';
import { useCreateRequestAsyncMutation, useLazyRequestIsExistQuery } from '../api/RequestToConnect.api';
import type { AppUserModel } from '../types/AppUserModel';
import type { FriendModel } from '../types/FriendModel';
import type { RequestToConnectModel } from '../types/RequestToConnectModel';
import PeopleInvitesToCommunity from './PeopleInvitesToCommunity';
import SelectedUserProfile from './SelectedUserProfile';

import './UserInformation.scss';

const successNotificationTimeout = 2000;
const failedNotificationTimeout = 2000;

interface UserInformationProps {
    myself: AppUserModel;
    personId: string;
    closeUserInformation(): void;
}

const UserInformation: React.FC<UserInformationProps> = ({ myself, personId, closeUserInformation }) => {
    const { t } = useTranslation("communication/userInformation");

    const chatHub = useChatHub();

    const navigate = useNavigate();

    const { data: person, isLoading: personIsLoading } = useGetUserByIdQuery(personId);

    const [isExistAsync] = useLazyIsExistQuery();
    const [createRequestAsync] = useCreateRequestAsyncMutation();
    const [isRequestExistAsync] = useLazyRequestIsExistQuery();

    const [showSuccessNotification, setShowSuccessNotification] = useState(false);
    const [showFailedNotification, setShowFailedNotification] = useState(false);
    const [openInviteToCommunity, setOpenInviteToCommunity] = useState(false);

    const { data: myFriends, isLoading } = useFriendSearchMyFriendsQuery(myself?.id);

    const checkExistOfChatsAsync = async (targetUser: AppUserModel) => {
        const queryParams = {
            userId: myself?.id,
            targetUserId: targetUser?.id
        };

        const isExist = await isExistAsync(queryParams);
        return isExist.data !== undefined ? isExist.data : true;
    }

    const createChatAsync = async (targetUser: AppUserModel) => {
        if (!chatHub) {
            return;
        }

        const isExist = await checkExistOfChatsAsync(targetUser);
        if (isExist) {
            navigate("/chats");
            return;
        }

        chatHub.subscribeToPersonalChat(() => {
            navigate("/chats");
        });

        await chatHub.personalChatHubConnection?.invoke("CreateChat", myself?.id, targetUser.id);
    }

    const checkIfRequestExistAsync = async (targetUserId: string) => {
        const arg = {
            userId: myself?.id,
            targetUserId: targetUserId
        };

        const isExist = await isRequestExistAsync(arg);
        if (isExist.error !== undefined) {
            return true;
        }

        return isExist.data;
    }

    const createRequestToConnectAsync = async (people: AppUserModel) => {
        const isExist = await checkIfRequestExistAsync(people.id);
        if (isExist) {
            setShowFailedNotification(true);

            setTimeout(() => {
                setShowFailedNotification(false);
            }, failedNotificationTimeout);

            return;
        }

        const newRequest: RequestToConnectModel = {
            id: 0,
            toAppUserId: people.id,
            when: new Date(),
            appUserId: myself?.id,
        };

        const createdRequest = await createRequestAsync(newRequest).unwrap();
        if (createdRequest) {
            setShowSuccessNotification(true);

            setTimeout(() => {
                setShowSuccessNotification(false);
            }, successNotificationTimeout);
        }
    }

    const moreDetails = () => {
        navigate(`/user?id=${person?.id}`)
    }

    if (isLoading || personIsLoading || !myFriends || !person) {
        return (<></>);
    }

    return (
        <div className="user-information">
            <div className={`alert alert-success sent-request${showSuccessNotification ? "_active" : ""}`} role="alert">
                <div>{t("SentRequest")}</div>
                <p>{person.username}</p>
            </div>
            <div className={`alert alert-warning sent-request${showFailedNotification ? "_active" : ""}`} role="alert">
                <div>{t("AlreadySentRequest")}</div>
                <p>{person.username}</p>
            </div>
            <div className="user-information__container box-shadow">
                <div className="user-information__menu">
                    <FontAwesomeIcon
                        icon={faCircleXmark}
                        title={t("Close") || ""}
                        onClick={closeUserInformation}
                    />
                </div>
                <div className="user-information__username">
                    {person.username}
                </div>
                <SelectedUserProfile
                    person={person}
                />
                <ul className="links">
                    <li>
                        <FontAwesomeIcon
                            icon={faCommentDots}
                            title={t("StartChat") || ""}
                            onClick={async () => await createChatAsync(person)}
                        />
                    </li>
                    <li>
                        {myFriends.filter((friend: FriendModel) => friend.whoFriendId === person.id || friend.forWhomId === person.id).length > 0
                            ? <FontAwesomeIcon
                                icon={faUserPlus}
                                title={t("AlreadyFriend") || ""}
                                className="user-friend"
                            />
                            : <FontAwesomeIcon
                                icon={faUserPlus}
                                title={t("RequestToConnect") || ""}
                                onClick={async () => await createRequestToConnectAsync(person)}
                            />
                        }
                    </li>
                    <li>
                        <FontAwesomeIcon
                            icon={faSquarePlus}
                            title={t("InviteToCommunity") || ""}
                            onClick={() => setOpenInviteToCommunity((item) => !item)}
                        />
                    </li>
                </ul>
                <div className="details">
                    <div className="btn-shadow" onClick={moreDetails}>
                        <FontAwesomeIcon
                            icon={faPersonCircleQuestion}
                        />
                        <div>{t("MoreDetails")}</div>
                    </div>
                </div>
            </div>
            {openInviteToCommunity &&
                <div className="invite">
                    <PeopleInvitesToCommunity
                        me={myself}
                        targetUser={person}
                        setOpenInviteToCommunity={setOpenInviteToCommunity}
                    />
                </div>
            }
        </div>
    );
}

export default memo(UserInformation);