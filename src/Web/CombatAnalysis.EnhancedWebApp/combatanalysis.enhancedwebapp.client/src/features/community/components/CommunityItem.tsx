import { faCirclePlus, faCircleQuestion } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { type JSX, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import User from '../../user/components/User';
import type { AppUserModel } from '../../user/types/AppUserModel';
import { useGetCommunityByIdQuery } from '../api/Community.api';
import { useCommunityUserSearchByUserIdQuery, useCreateCommunityUserMutation } from '../api/CommunityUser.api';
import type { CommunityUserModel } from '../types/CommunityUserModel';

interface CommunityItemProps {
    id: number;
    myself: AppUserModel | null;
}

const CommunityItem: React.FC<CommunityItemProps> = ({ id, myself }) => {
    const { t } = useTranslation('communication/community/Communities');

    const navigate = useNavigate();

    const { data: community, isLoading } = useGetCommunityByIdQuery(id);
    const { data: myCommunities, isLoading: myCommutiesIsLoading } = useCommunityUserSearchByUserIdQuery(myself?.id ?? "");

    const [canJoin, setCanJoin] = useState(true);
    const [userInformation, setUserInformation] = useState<JSX.Element | null>(null);

    const [createCommunityUserAsyncMut] = useCreateCommunityUserMutation();

    useEffect(() => {
        if (myCommunities?.length === 0) {
            return;
        }

        checkIfAlreadyJoined();
    }, [myCommunities]);

    const checkIfAlreadyJoined = () => {
        setCanJoin(myCommunities?.filter(x => x.communityId === id).length === 0)
    }

    const createCommunityUserAsync = async () => {
        const newCommunityUser: CommunityUserModel = {
            id: "",
            username: myself?.username ?? "",
            appUserId: myself?.id ?? "",
            communityId: id
        };

        const createdUser = await createCommunityUserAsyncMut(newCommunityUser);
        if (createdUser.data !== undefined) {
            navigate(`/community?id=${id}`);
        }
    }

    if (isLoading || myCommutiesIsLoading) {
        return <></>;
    }

    return (
        <div>
            {community?.policyType !== 0 &&
                <div className="private-container">
                    <div className="private">{t("Private")}</div>
                </div>
            }
            <div className="card box-shadow">
                <div className="card-body">
                    <h5 className="card-title">{community?.name}</h5>
                    <p className="card-text">{community?.description}</p>
                    {community?.policyType === 0 &&
                        <>
                            <div className="links">
                                <div className="open-community">
                                    <div className="btn-shadow" onClick={() => navigate(`/community?id=${community?.id}`)}>
                                        <FontAwesomeIcon
                                            icon={faCircleQuestion}
                                        />
                                        <div>{t("Open")}</div>
                                    </div>
                                </div>
                                {canJoin &&
                                    <div className="join-to-community">
                                        <div className="btn-shadow" onClick={async () => await createCommunityUserAsync()}>
                                            <FontAwesomeIcon
                                                icon={faCirclePlus}
                                            />
                                            <div>{t("Join")}</div>
                                        </div>
                                    </div>
                                }
                            </div>
                        </>
                    }
                </div>
            </div>
            <div className="owner-container">
                <div className="owner">
                    <User
                        myself={myself}
                        targetUserId={community?.appUserId ?? ""}
                        targetUsername={""}
                        setUserInformation={setUserInformation}
                    />
                </div>
            </div>
            {userInformation !== null &&
                <div className="owner-user-information">{userInformation}</div>
            }
        </div>
    );
}

export default CommunityItem;