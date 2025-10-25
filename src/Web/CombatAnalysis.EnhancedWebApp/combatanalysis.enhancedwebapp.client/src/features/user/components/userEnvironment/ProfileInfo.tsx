import type { RootState } from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import logger from '@/utils/Logger';
import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState, type SetStateAction } from 'react';
import { useSelector } from 'react-redux';
import { useLazyVerifyEmailQuery } from '../../api/User.api';
import type { AppUserModel } from '../../types/AppUserModel';

interface ProfileInfoProps {
    setIsEditMode: (value: SetStateAction<boolean>) => void;
    getDate: (user: AppUserModel) => string;
    t: (key: string) => string;
}

const ProfileInfo: React.FC<ProfileInfoProps> = ({ setIsEditMode, getDate, t }) => {
    const privacy = useSelector((state: RootState) => state.userPrivacy.value);
    const user = useSelector((state: RootState) => state.user.value);

    const [privacyHidden, setPrivacyHidden] = useState(false);
    const [generalHidden, setGeneralHidden] = useState(false);

    const [verifyEmailAsync] = useLazyVerifyEmailQuery();

    const goToVerifyEmailAsync = async () => {
        try {
            const identityServerVerifyEmailPath = APP_CONFIG.identity.verifyEmail;

            const response = await verifyEmailAsync({ identityPath: identityServerVerifyEmailPath ?? "", email: privacy?.email ?? "" }).unwrap();
            if (response !== undefined) {
                const uri = response.uri;
                window.location.href = uri;
            }
        } catch (e) {
            logger.error("Failed to verify email", e);
        }
    }

    if (!privacy || !user) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="profile__information">
            <div className="title">
                <div>{t("Privacy")}</div>
                <FontAwesomeIcon
                    icon={privacyHidden ? faArrowDown : faArrowUp}
                    onClick={() => setPrivacyHidden(!privacyHidden)}
                />
                <div className="actions">
                    <div className="btn-shadow" onClick={() => setIsEditMode(true)}>{t("Edit")}</div>
                </div>
            </div>
            {!privacyHidden &&
                <div className="privacy">
                    <div className="mb-3">
                        <label className="form-label">{t("Email")}</label>
                        <div className="privacy__email-container">
                            {privacy.emailVerified
                                ? <div className="verified-email">{privacy.email}</div>
                                : <>
                                    <div className="email">{privacy.email}</div>
                                    <div className="verification" onClick={goToVerifyEmailAsync}>{t("VerifyAccount")}</div>
                                </>
                            }
                        </div>
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputPhoneNumber" className="form-label">{t("PhoneNumber")}</label>
                        <input type="number" className="form-control" id="inputPhoneNumber" value={user.phoneNumber} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputBithday" className="form-label">{t("Birthday")}</label>
                        <input type="date" className="form-control" id="inputBithday" value={getDate(user)} disabled />
                    </div>
                </div>
            }
            <div className="title">
                <div>{t("General")}</div>
                <FontAwesomeIcon
                    icon={generalHidden ? faArrowDown : faArrowUp}
                    onClick={() => setGeneralHidden(!generalHidden)}
                />
            </div>
            {!generalHidden &&
                <div className="general">
                    <div className="mb-3">
                        <label htmlFor="inputUsername" className="form-label">{t("Username")}</label>
                        <input type="text" className="form-control" id="inputUsername" value={user.username} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputAboutMe" className="form-label">{t("AboutMe")}</label>
                        <input type="text" className="form-control" id="inputAboutMe" value={user.aboutMe} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inutFirstName" className="form-label">{t("FirstName")}</label>
                        <input type="text" className="form-control" id="inutFirstName" value={user.firstName} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputLastName" className="form-label">{t("LastName")}</label>
                        <input type="text" className="form-control" id="inputLastName" value={user.lastName} disabled />
                    </div>
                </div>
            }
        </div>
    );
}

export default ProfileInfo;