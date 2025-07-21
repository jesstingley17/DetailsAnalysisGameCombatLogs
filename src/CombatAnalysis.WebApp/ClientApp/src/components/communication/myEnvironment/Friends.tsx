import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useFriendSearchMyFriendsQuery } from '../../../store/api/user/Friend.api';
import { Friend } from '../../../types/Friend';
import Loading from '../../Loading';
import CommunicationMenu from '../CommunicationMenu';
import User from '../User';
import RequestsToConnect from './RequestsToConnect';

import '../../../styles/communication/myEnvironment/friends.scss';

const Friends: React.FC = () => {
    const { t } = useTranslation("communication/myEnvironment/friends");

    const myself = useSelector((state: any) => state.user.value);

    const [userInformation, setUserInformation] = useState<any>(null);

    const { data: myFriends, isLoading } = useFriendSearchMyFriendsQuery(myself?.id);

    if (isLoading || !myFriends) {
        return (
            <>
                <CommunicationMenu
                    currentMenuItem={6}
                    hasSubMenu={true}
                />
                <Loading />
            </>
        );
    }

    return (
        <>
            <div className="friends">
                <RequestsToConnect />
                <div>
                    <div className="friends__title">{t("Friends")}</div>
                </div>
                <ul>
                    {myFriends.length > 0
                        ? myFriends.map((friend: Friend) => (
                            <li key={friend.id} className="friend">
                                <User
                                    myself={myself}
                                    targetUserId={friend.forWhomId === myself?.id ? friend.whoFriendId : friend.forWhomId}
                                    targetUsername={friend.forWhomId === myself?.id ? friend.whoFriendUsername : friend.forWhomUsername}
                                    setUserInformation={setUserInformation}
                                    friendId={friend.id}
                                />
                            </li>
                        ))
                        : <div className="friends__empty">{t("Empty")}</div>
                    }
                </ul>
                {userInformation}
            </div>
            <CommunicationMenu
                currentMenuItem={6}
                hasSubMenu={true}
            />
        </>
    );
}

export default Friends;