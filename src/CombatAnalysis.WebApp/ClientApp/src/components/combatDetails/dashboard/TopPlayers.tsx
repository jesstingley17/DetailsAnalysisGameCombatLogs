import { faStar } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo } from "react";
import { CombatPlayerType } from "../../../types/components/combatDetails/CombatPlayerType";
import { TopPlayersProps } from '../../../types/components/combatDetails/dashboard/TopPlayersProps';
import { useTranslation } from 'react-i18next';

const TopPlayers: React.FC<TopPlayersProps> = ({ calculation, calculationValuePerTime, getDetailsValue, sortedPlayerData, detailsType }) => {
    const { t } = useTranslation("helpers/combatDetailsHelper");

    const dashboardDetailsType: any = {
        0: "damageDone",
        1: "healDone",
        2: "damageTaken",
        3: "resourcesRecovery"
    };

    const topPlayers = (): CombatPlayerType[] => {
        const top = sortedPlayerData.slice(0, 3);

        return top;
    }

    const getValuePerTimeType = (): string => {
        switch (detailsType) {
            case 0:
                return t("DPS");
            case 1:
                return t("HPS");
            case 2:
                return t("DTPS")
            case 3:
                return t("RRPS")
            default:
                return t("DPS")
        }
    }

    return (
        <ul className="top-players">
            {topPlayers().map((player, index) => (
                <li key={index} className="player">
                    <div className="value-per-time">{calculationValuePerTime(player, dashboardDetailsType[detailsType])} {getValuePerTimeType()}</div>
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
                    <div>{player.username.split('-')[0]}</div>
                    <div className="top-players__values">
                        <div className="actual-value">{getDetailsValue(player)}</div>
                        <div className="player-contribution">{calculation(player, dashboardDetailsType[detailsType])}%</div>
                    </div>
                </li>
            ))}
        </ul>
    );
}

export default memo(TopPlayers);