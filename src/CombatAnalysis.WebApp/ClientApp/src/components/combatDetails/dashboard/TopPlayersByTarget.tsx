import { faStar } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo } from "react";
import { useTranslation } from 'react-i18next';
import { TopPlayersByTargetProps } from '../../../types/components/combatDetails/dashboard/TopPlayersByTargetProps';

const TopPlayersByTarget: React.FC<TopPlayersByTargetProps> = ({ calculation, calculationDamagePerTimeByTarget, goToCombatGeneralDetails, getValueShortName, targetTopPlayers }) => {
    const { t } = useTranslation("helpers/combatDetailsHelper");

    return (
        <ul className="top-players">
            {targetTopPlayers.map((player, index) => (
                <li key={index} className="player">
                    <div className="value-per-time">{calculationDamagePerTimeByTarget(player.sum)} {t("DPS")}</div>
                    <div className="stars">
                        {index <= 0 &&
                            <FontAwesomeIcon
                                className={`${index <= 0 ? 'gold' : index <= 1 ? 'silver' : 'bronza'}`}
                                icon={faStar}
                            />
                        }
                        {index <= 1 &&
                            <FontAwesomeIcon
                                className={`${index <= 0 ? 'gold' : index <= 1 ? 'silver' : 'bronza'}`}
                                icon={faStar}
                            />
                        }
                        {index <= 2 &&
                            <FontAwesomeIcon
                                className={`${index <= 0 ? 'gold' : index <= 1 ? 'silver' : 'bronza'}`}
                                icon={faStar}
                            />
                        }
                    </div>
                    <div className="username" onClick={() => goToCombatGeneralDetails(player.id)}>{player.username.split('-')[0]}</div>
                    <div className="top-players__values">
                        <div className="actual-value">{getValueShortName(player.sum)}</div>
                        <div className="player-contribution">{calculation(player.sum)}%</div>
                    </div>
                </li>
            ))}
        </ul>
    );
}

export default memo(TopPlayersByTarget);