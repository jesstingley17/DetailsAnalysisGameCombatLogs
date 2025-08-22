import type { ChangeEvent, SetStateAction } from 'react';

interface ChatRulesItemProps {
    setInvitePeople: (value: SetStateAction<number>) => void;
    setRemovePeople: (value: SetStateAction<number>) => void;
    setPinMessage: (value: SetStateAction<number>) => void;
    setAnnouncements: (value: SetStateAction<number>) => void;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    payload: any;
    t: (key: string) => string;
}

const ChatRulesItem: React.FC<ChatRulesItemProps> = ({ setInvitePeople, setRemovePeople, setPinMessage, setAnnouncements, payload, t }) => {
    const handleInviteChange = (e: ChangeEvent<HTMLInputElement> | undefined) => {
        setInvitePeople(e?.target.value ? +e?.target.value : 0);
    }

    const handleRemoveChange = (e: ChangeEvent<HTMLInputElement> | undefined) => {
        setRemovePeople(e?.target.value ? +e?.target.value : 0);
    }

    const handlePinMessageChange = (e: ChangeEvent<HTMLInputElement> | undefined) => {
        setPinMessage(e?.target.value ? +e?.target.value : 0);
    }

    const handleAnnounceChange = (e: ChangeEvent<HTMLInputElement> | undefined) => {
        setAnnouncements(e?.target.value ? +e?.target.value : 0);
    }

    return (
        <ul className="rules">
            <li>
                <div>{t("InviteOtherPeople")}</div>
                <div className="rules__content">
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="invite-people" id="invite-people-anyone" value="0"
                            onChange={handleInviteChange} defaultChecked={payload["invitePeople"] === 0} />
                        <label className="form-check-label" htmlFor="invite-people-anyone">{t("Owner")}</label>
                    </div>
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="invite-people" id="invite-people-special" value="1"
                            onChange={handleInviteChange} defaultChecked={payload["invitePeople"] === 1} />
                        <label className="form-check-label" htmlFor="invite-people-special">{t("Anyone")}</label>
                    </div>
                </div>
            </li>
            <li>
                <div>{t("RemoveAnotherPeople")}</div>
                <div className="rules__content">
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="remove-people" id="remove-people-anyone" value="0"
                            onChange={handleRemoveChange} defaultChecked={payload["removePeople"] === 0} />
                        <label className="form-check-label" htmlFor="remove-people-anyone">{t("Owner")}</label>
                    </div>
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="remove-people" id="remove-people-special" value="1"
                            onChange={handleRemoveChange} defaultChecked={payload["removePeople"] === 1} />
                        <label className="form-check-label" htmlFor="remove-people-special">{t("Anyone")}</label>
                    </div>
                </div>
            </li>
            <li>
                <div>{t("PinMessage")}</div>
                <div className="rules__content">
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="pin-message" id="pin-message-anyone" value="0"
                            onChange={handlePinMessageChange} defaultChecked={payload["pinMessage"] === 0} />
                        <label className="form-check-label" htmlFor="pin-message-anyone">{t("Owner")}</label>
                    </div>
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="pin-message" id="pin-message-special" value="1"
                            onChange={handlePinMessageChange} defaultChecked={payload["pinMessage"] === 1} />
                        <label className="form-check-label" htmlFor="pin-message-special">{t("Anyone")}</label>
                    </div>
                </div>
            </li>
            <li>
                <div>{t("Announcements")}</div>
                <div className="rules__content">
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="announce" id="announce-anyone" value="0"
                            onChange={handleAnnounceChange} defaultChecked={payload["announcements"] === 0} />
                        <label className="form-check-label" htmlFor="announce-anyone">{t("Owner")}</label>
                    </div>
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="announce" id="announce-special" value="1"
                            onChange={handleAnnounceChange} defaultChecked={payload["announcements"] === 1} />
                        <label className="form-check-label" htmlFor="announce-special">{t("Anyone")}</label>
                    </div>
                </div>
            </li>
        </ul>
    );
}

export default ChatRulesItem;