import { faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState, type JSX } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../api/Account.api';
import type { RequestToConnectModel } from '../../types/RequestToConnectModel';
import User from '../User';

interface MyRequestItemProps {
    request: RequestToConnectModel;
    cancelMyRequestAsync: (requestId: number) => Promise<void>;
}

const MyRequestItem: React.FC<MyRequestItemProps> = ({ request, cancelMyRequestAsync }) => {
    const { t } = useTranslation('communication/myEnvironment/myRequestItem');

    const { data: user, isLoading } = useGetUserByIdQuery(request.toAppUserId);

    const [userInformation, setUserInformation] = useState<JSX.Element | null>(null);

    if (!user || isLoading) {
        return <></>;
    }

    return (
        <div className="request-to-connect">
            <div className="request-to-connect__username">
                <User
                    targetUserId={user.id}
                    targetUsername={""}
                    setUserInformation={setUserInformation}
                />
            </div>
            <div className="request-to-connect__answer">
                <div className="reject">
                    <FontAwesomeIcon
                        icon={faCircleXmark}
                        title={t("Cancel")}
                        onClick={async () => await cancelMyRequestAsync(request.id)}
                    />
                </div>
            </div>
            {userInformation}
        </div>
    );
}

export default MyRequestItem;