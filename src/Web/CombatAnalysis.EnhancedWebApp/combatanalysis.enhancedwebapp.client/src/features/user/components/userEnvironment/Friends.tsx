import type { RootState } from '@/app/Store';
import CommunicationMenu from '@/shared/components/CommunicationMenu';
import Loading from '@/shared/components/Loading';
import { useState, type JSX } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useFindFriendByUserIdQuery } from '../../api/Friend.api';
import type { FriendModel } from '../../types/FriendModel';
import User from '../User';
import RequestsToConnect from './RequestsToConnect';

import './Friends.scss';

const Friends: React.FC = () => {
    const { t } = useTranslation('communication/myEnvironment/friends');

    const myself = useSelector((state: RootState) => state.user.value);

    const [userInformation, setUserInformation] = useState<JSX.Element | null>(null);

    const { data: myFriends, isLoading } = useFindFriendByUserIdQuery(myself?.id ?? "");

    if (isLoading || !myFriends || !myself) {
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
                        ? myFriends.map((friend: FriendModel) => (
                            <li key={friend.id} className="friend">
                                <User
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