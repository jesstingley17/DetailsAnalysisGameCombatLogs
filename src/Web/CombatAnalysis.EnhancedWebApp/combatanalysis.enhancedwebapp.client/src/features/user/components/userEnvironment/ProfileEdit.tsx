import type { RootState } from '@/app/Store';
import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState, type SetStateAction } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useEditAsyncMutation } from '../..//api/Account.api';
import { updateUser } from '../../store/UserSlice';
import type { AppUserModel } from '../../types/AppUserModel';

interface ProfileEditProps {
    setIsEditMode: (value: SetStateAction<boolean>) => void;
    getDate: (user: AppUserModel) => string;
    t: (key: string) => string;
}

const ProfileEdit: React.FC<ProfileEditProps> = ({ setIsEditMode, getDate, t }) => {
    const dispatch = useDispatch();

    const user = useSelector((state: RootState) => state.user.value);

    const [privacyHidden, setPrivacyHidden] = useState(false);
    const [generalHidden, setGeneralHidden] = useState(false);

    const [sealedUser, setSealedUser] = useState < AppUserModel | null>(null);

    const [editUserAsyncMut] = useEditAsyncMutation();

    useEffect(() => {
        setSealedUser(Object.assign({}, user));
    }, [user]);

    const updateUserAsync = async () => {
        if (!sealedUser) {
            return;
        }

        const updatedUser = await editUserAsyncMut(sealedUser);
        if (updatedUser.data !== undefined) {
            dispatch(updateUser(updatedUser.data));
        }

        setIsEditMode(false);
    }

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const handleForm = (e: any) => {
        if (!sealedUser) {
            return;
        }

        setSealedUser({
            ...sealedUser,
            [e.target.name]: e.target.value
        });
    }

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const handleSubmitAsync = async (e: any) => {
        e?.preventDefault();

        await updateUserAsync();
    }

    if (!sealedUser) {
        return (<div>Loading...</div>);
    }

    return (
        <form className="profile__edit-profile" onSubmit={handleSubmitAsync}>
            <div className="title">
                <div>{t("Privacy")}</div>
                {privacyHidden
                    ? <FontAwesomeIcon
                        icon={faArrowDown}
                        onClick={() => setPrivacyHidden(!privacyHidden)}
                    />
                    : <FontAwesomeIcon
                        icon={faArrowUp}
                        onClick={() => setPrivacyHidden(!privacyHidden)}
                    />
                }
                <div className="actions">
                    <div className="btn-shadow save" onClick={handleSubmitAsync}>{t("Save")}</div>
                    <div className="btn-shadow" onClick={() => setIsEditMode(false)}>{t("Cancel")}</div>
                </div>
            </div>
            {!privacyHidden &&
                <div className="privacy">
                    <div className="mb-3">
                        <label htmlFor="inputPhoneNumber" className="form-label">{t("PhoneNumber")}</label>
                        <input type="number" className="form-control" id="inputPhoneNumber" name="phoneNumber" onChange={handleForm} value={sealedUser.phoneNumber} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputBithdayl" className="form-label">{t("Birthday")}</label>
                        <input type="date" className="form-control" id="inputBithdayl" name="birthday" onChange={handleForm} value={getDate(sealedUser)} />
                    </div>
                </div>
            }
            <div className="title">
                <div>{t("General")}</div>
                {generalHidden
                    ? <FontAwesomeIcon
                        icon={faArrowDown}
                        onClick={() => setGeneralHidden(!generalHidden)}
                    />
                    : <FontAwesomeIcon
                        icon={faArrowUp}
                        onClick={() => setGeneralHidden(!generalHidden)}
                    />
                }
            </div>
            {!generalHidden &&
                <div className="general">
                    <div className="mb-3">
                        <label htmlFor="inputUsername" className="form-label">{t("Username")}</label>
                        <input type="text" className="form-control" id="inputUsername" name="username" onChange={handleForm} value={sealedUser.username} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputAboutMe" className="form-label">{t("AboutMe")}</label>
                        <input type="text" className="form-control" id="inputAboutMe" name="aboutMe" onChange={handleForm} value={sealedUser.aboutMe} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inutFirstName" className="form-label">{t("FirstName")}</label>
                        <input type="text" className="form-control" id="inutFirstName" name="firstName" onChange={handleForm} value={sealedUser.firstName} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputLastName" className="form-label">{t("LastName")}</label>
                        <input type="text" className="form-control" id="inputLastName" name="lastName" onChange={handleForm} value={sealedUser.lastName} />
                    </div>
                </div>
            }
        </form>
    );
}

export default ProfileEdit;