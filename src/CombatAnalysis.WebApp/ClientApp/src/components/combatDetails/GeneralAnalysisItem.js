import { faBolt, faCheck, faCircleNodes, faClock, faDatabase, faHourglassStart, faKhanda, faPlusCircle, faShieldHalved } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

const getCombatDuration = (duration) => duration.substring(3);

const GeneralAnalysisItem = ({ uniqueCombats, combatLogId, getValueShortName }) => {
    const { t } = useTranslation("combatDetails/generalAnalysis");

    const navigate = useNavigate();

    const [selectedCombatIndex, setSelectedCombatIndex] = useState(0);
    const [selectedCombat, setSelectedCombat] = useState(uniqueCombats[selectedCombatIndex]);

    useEffect(() => {
        setSelectedCombat(uniqueCombats[selectedCombatIndex]);
    }, [selectedCombatIndex]);

    const formatDate = (dateString) => {
        const date = new Date(dateString);
        const hoursMins = `${date.getUTCHours()}:${date.getUTCMinutes()}:${date.getUTCSeconds()}`;

        return hoursMins;
    }

    if (selectedCombat === null) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="card">
            <ul className="unique-combats__all">
                {uniqueCombats?.map((combat, index) => (
                    <li key={combat.id + 2} className={`unique-combats__${combat.isWin ? 'win' : 'lose'}`} onClick={() => setSelectedCombatIndex(index)}>
                        <div className="combat-number">{index + 1}</div>
                        <div className="combat-time">
                            <div className="combat-time__range">
                                <div>
                                    <div>{formatDate(combat.startDate)}</div>
                                </div>
                                <div>-</div>
                                <div>
                                    <div>{formatDate(combat.finishDate)}</div>
                                </div>
                            </div>
                            <div className="combat-time__lasts">
                                <div>{getCombatDuration(combat.duration)}</div>
                                <FontAwesomeIcon
                                    icon={faHourglassStart}
                                    className="list-group-item__duration"
                                    title={t("Duration")}
                                />
                            </div>
                        </div>
                    </li>
                ))}
            </ul>
            <div className="unique-combats__selected">
                <div className="combat-title">
                    <div className={`status combat-title__${selectedCombat.isWin ? 'win' : 'lose'}`}>
                        <div className="combat-number">{selectedCombatIndex + 1}</div>
                        <div className="card-body">
                            <h5 className="card-title">{selectedCombat.name}</h5>
                            <p className="card-text">{selectedCombat.dungeonName}</p>
                        </div>
                    </div>
                    <FontAwesomeIcon
                        icon={selectedCombat.isReady ? faCheck : faClock}
                        className="list-group-item__player-statistic-item"
                        title={selectedCombat.isReady ? t("Ready") : t("NotReady")}
                    />
                </div>
                <div className="combat-time">
                    <div className="combat-time__range">
                        <div className="list-group-item">
                            <div>
                                <div>{formatDate(selectedCombat?.startDate)}</div>
                            </div>
                        </div>
                        <div>-</div>
                        <div className="list-group-item">
                            <div>
                                <div>{formatDate(selectedCombat?.finishDate)}</div>
                            </div>
                        </div>
                    </div>
                    <div className="combat-time__lasts">
                        <div>{getCombatDuration(selectedCombat.duration)}</div>
                        <FontAwesomeIcon
                            icon={faHourglassStart}
                            className="list-group-item__player-statistic-item"
                            title={t("Duration")}
                        />
                    </div>
                </div>
            </div>
            <ul className="information">
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faKhanda}
                        className="list-group-item__player-statistic-item"
                        title={t("Damage")}
                    />
                    <div>{getValueShortName(uniqueCombats[selectedCombatIndex].damageDone)}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faPlusCircle}
                        className="list-group-item__player-statistic-item"
                        title={t("Healing")}
                    />
                    <div>{getValueShortName(uniqueCombats[selectedCombatIndex].healDone)}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faShieldHalved}
                        className="list-group-item__player-statistic-item"
                        title={t("DamageTaken")}
                    />
                    <div>{getValueShortName(uniqueCombats[selectedCombatIndex].damageTaken)}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faBolt}
                        className="list-group-item__player-statistic-item"
                        title={t("EnergyRecovery")}
                    />
                    <div>{getValueShortName(uniqueCombats[selectedCombatIndex].energyRecovery)}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faCircleNodes}
                        className="list-group-item__player-statistic-item"
                        title={t("Buffs")}
                    />
                    <div className="auras-details" onClick={() => navigate(`/general-analysis/auras?combat=${selectedCombat.id}&combatLog=${combatLogId}`)}>More...</div>
                </li>
            </ul>
            <div className="card-body details">
                {uniqueCombats[selectedCombatIndex].isReady
                    ? <div className="btn-shadow" onClick={() => navigate(`/details-specifical-combat?id=${uniqueCombats[selectedCombatIndex].id}&combatLogId=${combatLogId}&name=${uniqueCombats[selectedCombatIndex].name}&number=${selectedCombatIndex + 1}&isWin=${selectedCombat.isWin}`)}>
                        <FontAwesomeIcon
                            icon={faDatabase}
                        />
                        <div>{t("MoreDetails")}</div>
                    </div>
                    : <div>{t("NeedWait")}</div>
                }
            </div>
        </div>
    );
}

export default GeneralAnalysisItem;