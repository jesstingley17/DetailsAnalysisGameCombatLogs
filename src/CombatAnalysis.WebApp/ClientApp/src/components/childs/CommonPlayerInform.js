import { faBolt, faBookOpenReader, faKhanda, faPlusCircle, faShieldHalved, faUser } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

const fixedNumberUntil = 2;

const CommonPlayerInform = ({ player, combatId, combatLogId, combatName }) => {
    const { t } = useTranslation("childs/playerInformation");

    const navigate = useNavigate();

    const detailsTypes = ["DamageDone", "HealDone", "DamageTaken", "ResourceRecovery"];

    const navigateToDetails = (detailsType) => {
        navigate(`/combat-details?id=${player.id}&detailsType=${detailsType}&combatId=${combatId}&combatLogId=${combatLogId}&name=${combatName}&tab=${1}`);
    }

    const getValueShortName = (value) => {
        const thousands = value / 1000;
        const millions = value / 1000000;

        if (millions >= 1) {
            return `${millions.toFixed(fixedNumberUntil)} M`;
        }
        else if (thousands >= 1) {
            return `${thousands.toFixed(fixedNumberUntil)} K`;
        }

        return value;
    }

    return (
        <ul className="player-information">
            <li className="list-group-item">
                <FontAwesomeIcon
                    icon={faKhanda}
                    className="list-group-item__player-statistic-item"
                    title={t("Damage")}
                />
                <div>{getValueShortName(player.damageDone)}</div>
                {player.damageDone > 0 &&
                    <div className="btn-shadow"
                        onClick={() => navigateToDetails(detailsTypes[0])}
                        title={t("OpenDamageAnalyzing")}>
                        <FontAwesomeIcon
                            icon={faBookOpenReader}
                        />
                        <div>{t("Damage")}</div>
                    </div>
                }
            </li>
            <li className="list-group-item">
                <FontAwesomeIcon
                    icon={faPlusCircle}
                    className="list-group-item__player-statistic-item"
                    title={t("Healing")}
                />
                <div>{getValueShortName(player.healDone)}</div>
                {player.healDone > 0 &&
                    <div className="btn-shadow"
                        onClick={() => navigateToDetails(detailsTypes[1])}
                        title={t("OpenHealingAnalyzing")}>
                        <FontAwesomeIcon
                            icon={faBookOpenReader}
                        />
                        <div>{t("Healing")}</div>
                    </div>
                }
            </li>
            <li className="list-group-item">
                <FontAwesomeIcon
                    icon={faShieldHalved}
                    className="list-group-item__player-statistic-item"
                    title={t("DamageTaken")}
                />
                <div>{getValueShortName(player.damageTaken)}</div>
                {player.damageTaken > 0 &&
                    <div className="btn-shadow"
                        onClick={() => navigateToDetails(detailsTypes[2])}
                        title={t("OpenDamageTakenAnalyzing")}>
                        <FontAwesomeIcon
                            icon={faBookOpenReader}
                        />
                        <div>{t("DamageTaken")}</div>
                    </div>
                }
            </li>
            <li className="list-group-item">
                <FontAwesomeIcon
                    icon={faBolt}
                    className="list-group-item__player-statistic-item"
                    title={t("ResourcesRecovery")}
                />
                <div>{getValueShortName(player.resourcesRecovery)}</div>
                {player.resourcesRecovery > 0 &&
                    <div className="btn-shadow"
                        onClick={() => navigateToDetails(detailsTypes[3])}
                        title={t("OpenResourcesRecoveryAnalyzing")}>
                        <FontAwesomeIcon
                            icon={faBookOpenReader}
                        />
                        <div>{t("Resources")}</div>
                    </div>
                }
            </li>
            <li className="list-group-item">
                <FontAwesomeIcon
                    icon={faUser}
                    className="list-group-item__player-statistic-item"
                    title={t("AverageItemLevel")}
                />
                <div>{player.averageItemLevel}</div>
            </li>
        </ul>
    );
}

export default memo(CommonPlayerInform);