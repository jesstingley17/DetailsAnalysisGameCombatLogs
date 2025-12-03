import { faMagnifyingGlassMinus, faMagnifyingGlassPlus, faUserXmark, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState, type ChangeEvent, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import type { CommunityUserModel } from '../../types/CommunityUserModel';
import CommunityUsersItem from './CommunityUsersItem';

import './CommunityUsers.scss';

interface CommunityUsersProps {
    myself: AppUserModel;
    communityUsers: CommunityUserModel[];
    removeUsersAsync(peopleToRemove: CommunityUserModel[]): Promise<void>;
    setShowMembers(value: SetStateAction<boolean>): void;
    isPopup: boolean;
    canRemovePeople(): boolean;
}

const CommunityUsers: React.FC<CommunityUsersProps> = ({ myself, communityUsers, removeUsersAsync, setShowMembers, isPopup, canRemovePeople }) => {
    const { t } = useTranslation('communication/members');

    const [showRemoveUser, setShowRemoveUser] = useState(false);
    const [showSearchPeople, setShowSearchPeople] = useState(false);
    const [usersToRemove, setUsersToRemove] = useState<CommunityUserModel[]>([]);
    const [searchUsername, setSearchUsername] = useState("");

    const handleShowRemoveUsers = () => {
        setUsersToRemove([]);

        setShowRemoveUser((item) => !item);
    }

    const hidePeopleInspectionMode = () => {
        setUsersToRemove([]);

        setShowMembers(!isPopup);
        setShowRemoveUser(false);
    }

    const searchUsernameHandle = (event: ChangeEvent<HTMLInputElement>) => {
        const content = event.target.value;

        setSearchUsername(content);
    }

    const clear = () => {
        setSearchUsername("");
    }

    return (
        <div className={`people-inspection${isPopup ? "__popup" : "__window"} ${isPopup ? "box-shadow" : ""}`}>
            <div className="title">
                <FontAwesomeIcon
                    icon={showSearchPeople ? faMagnifyingGlassMinus : faMagnifyingGlassPlus}
                    title={(showSearchPeople ? t("HideSearchPeople") : t("ShowSearchPeople")) || ""}
                    onClick={() => setShowSearchPeople(prev => !prev)}
                />
                <div>{t("Members")}</div>
                {canRemovePeople() &&
                    <FontAwesomeIcon
                        icon={faUserXmark}
                        className={`remove${showRemoveUser ? "_active" : ""}`}
                        title={t("Remove") || ""}
                        onClick={handleShowRemoveUsers}
                    />
                }
            </div>
            <div className={`mb-3 add-new-people__search${showSearchPeople ? "_active" : ""}`}>
                <label htmlFor="inputUsername" className="form-label">{t("SearchPeople")}</label>
                <div className="add-new-people__search-input">
                    <input type="text" className="form-control" placeholder={t("TypeUsername") || ""} id="inputUsername" value={searchUsername}
                        onChange={searchUsernameHandle} />
                    <FontAwesomeIcon
                        icon={faXmark}
                        title={t("Clean") || ""}
                        onClick={clear}
                    />
                </div>
            </div>
            <div className="divide"></div>
            <ul className="list">
                {searchUsername === ""
                    ? communityUsers?.map((communityUser: CommunityUserModel) => (
                        <li className="user-target-community" key={communityUser.id}>
                            <CommunityUsersItem
                                me={myself}
                                communityUser={communityUser}
                                usersToRemove={usersToRemove}
                                setUsersToRemove={setUsersToRemove}
                                showRemoveUser={showRemoveUser}
                            />
                        </li>
                    ))
                    : communityUsers?.filter((communityUser: CommunityUserModel) => communityUser.username.toLowerCase().startsWith(searchUsername.toLowerCase())).map((communityUser: CommunityUserModel) => (
                        <li className="user-target-community" key={communityUser.id}>
                            <CommunityUsersItem
                                me={myself}
                                communityUser={communityUser}
                                usersToRemove={usersToRemove}
                                setUsersToRemove={setUsersToRemove}
                                showRemoveUser={showRemoveUser}
                            />
                        </li>
                    ))}
            </ul>
            <div className="item-result">
                {(canRemovePeople() && showRemoveUser) &&
                    <div className="btn-border-shadow" onClick={async () => await removeUsersAsync(usersToRemove)}>{t("Accept")}</div>
                }
                {isPopup &&
                    <div className="btn-border-shadow" onClick={hidePeopleInspectionMode}>{t("Close")}</div>
                }
            </div>
        </div>
    );
}

export default CommunityUsers;