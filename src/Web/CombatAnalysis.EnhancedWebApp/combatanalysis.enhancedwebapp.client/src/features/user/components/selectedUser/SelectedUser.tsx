import type { RootState } from '@/app/Store';
import CommunicationMenu from '@/shared/components/CommunicationMenu';
import { faComments, faEnvelopesBulk, faUser, faUserGroup } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useLocation } from 'react-router-dom';
import { useLazyGetUserPostsByUserIdQuery } from '../../../feed/api/Post.api';
import UserPost from '../../../feed/components/post/UserPost';
import type { UserPostModel } from '../../../feed/types/UserPostModel';
import { useLazyGetUserByIdQuery } from '../../api/Account.api';
import type { AppUserModel } from '../../types/AppUserModel';
import Friends from '../userEnvironment/Friends';
import SelectedUserCommunities from './SelectedUserCommunities';
import SelectedUserProfile from './SelectedUserProfile';

import './SelectedUser.scss';

const SelectedUser: React.FC = () => {
    const { t } = useTranslation('communication/people/user');

    const myself = useSelector((state: RootState) => state.user.value);

    const location = useLocation();

    const [getUserPosts] = useLazyGetUserPostsByUserIdQuery();
    const [getUserById] = useLazyGetUserByIdQuery();

    const [personId, setPersonId] = useState<string>("0");
    const [person, setPerson] = useState<AppUserModel | null>(null);
    const [currentMenuItem, setMenuItem] = useState(0);
    const [allPosts, setAllPosts] = useState<UserPostModel[]>([]);

    useEffect(() => {
        const queryParams = new URLSearchParams(location.search);
        const id: string = queryParams.get("id") || "0";

        setPersonId(id);
    }, [])

    useEffect(() => {
        if (personId === "0") {
            return;
        }

        const getUser = async () => {
            await getUserByIdAsync();
        }

        getUser();
    }, [personId])

    const getUserByIdAsync = async () => {
        const user = await getUserById(personId);

        if (user.data !== undefined) {
            setPerson(user.data);

            getAllPostsAsync(user.data.id);
        }
    }

    const getAllPostsAsync = async (appUserId: string) => {
        const userPosts = await getUserPosts({ appUserId, pageSize: 10 });

        if (userPosts.data !== undefined) {
            setAllPosts(userPosts.data);
        }
    }

    if (!myself) {
        return (<></>);
    }

    return (
        <div className="communication">
            <CommunicationMenu
                currentMenuItem={7}
                hasSubMenu={false}
            />
            <div className="communication-content user">
                <div className="user-information__username">
                    {person?.username}
                </div>
                <div className="user__container">
                    <div className="menu-container">
                        <ul className="user__menu">
                            <li className="sub-menu" onClick={() => setMenuItem(0)}>
                                <FontAwesomeIcon
                                    className={`current${currentMenuItem === 0 ? "_active" : ""}`}
                                    title={t("Profile") || ""}
                                    icon={faUser}
                                />
                            </li>
                            <li className="sub-menu" onClick={() => setMenuItem(1)}>
                                <FontAwesomeIcon
                                    className={`current${currentMenuItem === 1 ? "_active" : ""}`}
                                    title={t("Posts") || ""}
                                    icon={faEnvelopesBulk}
                                />
                            </li>
                            <li className="sub-menu" onClick={() => setMenuItem(2)}>
                                <FontAwesomeIcon
                                    className={`current${currentMenuItem === 2 ? "_active" : ""}`}
                                    title={t("Friends") || ""}
                                    icon={faUserGroup}
                                />
                            </li>
                            <li className="sub-menu" onClick={() => setMenuItem(3)}>
                                <FontAwesomeIcon
                                    className={`current${currentMenuItem === 3 ? "_active" : ""}`}
                                    title={t("Communities") || ""}
                                    icon={faComments}
                                />
                            </li>
                        </ul>
                    </div>
                    <div className="user__content">
                        {currentMenuItem === 0 &&
                            <SelectedUserProfile
                                person={person}
                            />
                        }
                        {currentMenuItem === 1 &&
                            <ul className="posts">
                                {allPosts.length === 0
                                    ? <div>{t("Empty")}</div>
                                    : allPosts?.map(post => (
                                        <li key={post.id}>
                                            <UserPost
                                                myself={myself}
                                                post={post}
                                            />
                                        </li>
                                    ))
                                }
                            </ul>
                        }
                        {currentMenuItem === 2 &&
                            <Friends />
                        }
                        {currentMenuItem === 3 &&
                            <SelectedUserCommunities
                                user={person}
                                t={t}
                            />
                        }
                    </div>
                </div>
            </div>
        </div>
    );
}

export default SelectedUser;