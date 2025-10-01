import { faCircleCheck, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { type JSX, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../api/Account.api';
import type { RequestToConnectModel } from '../../types/RequestToConnectModel';
import User from '../User';

interface RequestItemProps {
    request: RequestToConnectModel;
    acceptRequestAsync: (request: RequestToConnectModel) => Promise<void>;
    rejectRequestAsync: (requestId: number) => Promise<void>;
}

const RequestItem: React.FC<RequestItemProps> = ({ request, acceptRequestAsync, rejectRequestAsync }) => {
    const { t } = useTranslation('communication/myEnvironment/requestItem');

    const { data: user, isLoading } = useGetUserByIdQuery(request.appUserId);

    const [userInformation, setUserInformation] = useState<JSX.Element | null>(null);

    if (isLoading) {
        return <></>;
    }

    return (
        <span className="request-to-connect">
            <div className="request-to-connect__username">
                <User
                    targetUserId={user?.id ?? ""}
                    targetUsername={""}
                    setUserInformation={setUserInformation}
                />
            </div>
            <div className="request-to-connect__answer">
                <div className="accept">
                    <FontAwesomeIcon
                        icon={faCircleCheck}
                        title={t("Accept")}
                        onClick={async () => await acceptRequestAsync(request)}
                    />
                </div>
                <div className="reject">
                    <FontAwesomeIcon
                        icon={faCircleXmark}
                        title={t("Reject")}
                        onClick={async () => await rejectRequestAsync(request.id)}
                    />
                </div>
            </div>
            {userInformation}
        </span>
    );
}

export default RequestItem;