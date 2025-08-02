import { faArrowDown, faArrowUp, faCircleXmark, faMagnifyingGlassChart, faSpinner, faTriangleExclamation } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { format } from 'date-fns';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import type { CombatLogItemProps } from '../../../types/components/combatDetails/gameCombatLogs/CombatLogItemProps';
import CombatLogItemDiscussion from './CombatLogItemDiscussion';

const CombatLogItem: React.FC<CombatLogItemProps> = ({ log, isAuth }) => {
    const { t } = useTranslation("combatDetails/mainInformation");W

    const navigate = useNavigate();

    const [showChats, setShowChats] = useState(false);
    const [showGroupChats, setShowGroupChats] = useState(true);
    const [showPersonalChats, setShowPersonalChats] = useState(true);

    return (
        <div className="card">
            <ul className="list-group list-group-flush">
                <li className="list-group-item title">
                    <div className="title__main">
                        {log.combatsInQueue > 0 &&
                            <>
                                <FontAwesomeIcon
                                    icon={faSpinner}
                                    title={t("Uploading")}
                                />
                                <div>{log.numberReadyCombats} / {log.combatsInQueue}</div>
                            </>
                        }
                        <div>{log.name}</div>
                    </div>
                    <div className="actions">
                        {!isAuth &&
                            <FontAwesomeIcon
                                icon={faTriangleExclamation}
                                className="authorization"
                                title={t("ShouldAuthorize")}
                            />
                        }
                        <CombatLogItemDiscussion />
                    </div>
                </li>
                <li className="list-group-item">{format(new Date(log.date), 'MM/dd/yyyy HH:mm')}</li>
            </ul>
            <div className="card-body">
                {log.numberReadyCombats > 0 &&
                    <div className="btn-shadow" onClick={() => navigate(`/general-analysis?id=${log.id}`)}>
                        <FontAwesomeIcon
                            icon={faMagnifyingGlassChart}
                        />
                        <div>{t("Analyzing")}</div>
                    </div>
                }
            </div>
            {showChats &&
                <div className="chat-list">
                    <div className="chat-list__close">
                        <FontAwesomeIcon
                            icon={faCircleXmark}
                            onClick={() => setShowChats(false)}
                            title={t("Close")}
                        />
                    </div>
                    <div>{t("Chats")}</div>
                    <div className="chat-list__chats">
                        <div className="title">
                            <div className="name">{t("GroupChats")}</div>
                            <FontAwesomeIcon
                                icon={showGroupChats ? faArrowUp : faArrowDown}
                                onClick={() => setShowGroupChats(!showGroupChats)}
                                title={showGroupChats ? t("HideChats") : t("ShowChats")}
                            />
                        </div>
                        <div className="title">
                            <div className="name">{t("PersonalChats")}</div>
                            <FontAwesomeIcon
                                icon={showPersonalChats ? faArrowUp : faArrowDown}
                                onClick={() => setShowPersonalChats(!showPersonalChats)}
                                title={showPersonalChats ? t("HideChats") : t("ShowChats")}
                            />
                        </div>
                    </div>
                    <input type="button" value={t("Close")} className="btn btn-light" onClick={() => setShowChats(false)} />
                </div>
            }
        </div>
    );
}

export default CombatLogItem;