import { faPlus, faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../features/user/api/Account.api';
import type { AppUserModel } from '../../features/user/types/AppUserModel';

interface AddFriendItemProps {
    friendUserId: string;
    addUserToList: (user: AppUserModel) => void;
    removeUserToList: (user: AppUserModel) => void;
    filterContent: string;
    peopleIdToJoin: AppUserModel[];
}

const AddFriendItem: React.FC<AddFriendItemProps> = ({ friendUserId, addUserToList, removeUserToList, filterContent, peopleIdToJoin }) => {
    const { t } = useTranslation('addFriendItem');

    const { data: user, isLoading } = useGetUserByIdQuery(friendUserId);

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        user?.username.toLowerCase().startsWith(filterContent.toLowerCase()) &&
        <>
            <div>{user?.username}</div>
            {peopleIdToJoin.filter(x => x.id === user.id).length > 0
                ? <FontAwesomeIcon
                    icon={faUserPlus}
                    title={t("CancelRequest") || ""}
                    onClick={() => removeUserToList(user)}
                />
                : <FontAwesomeIcon
                    icon={faPlus}
                    title={t("SendInvite") || ""}
                    onClick={() => addUserToList(user)}
                />
            }
        </>
    );
}

export default AddFriendItem;