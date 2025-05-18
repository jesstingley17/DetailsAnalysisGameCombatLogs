import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo } from "react";
import { useTranslation } from 'react-i18next';
import { CombatPlayerType } from "../../../types/components/combatDetails/CombatPlayerType";
import { DashboardMinDetailsProps } from '../../../types/components/combatDetails/dashboard/DashboardMinDetailsProps';

const DashboardMinDetails: React.FC<DashboardMinDetailsProps> = ({ name, calculation, calculationValuePerTime, goToCombatGeneralDetails, getDetailsValue, sortedPlayerData, detailsType, itemCount, setItemCount }) => {
    const minCount = 4;

    const { t } = useTranslation("helpers/combatDetailsHelper");

    const dashboardDetailsType: any = {
        0: "damageDone",
        1: "healDone",
        2: "damageTaken",
        3: "resourcesRecovery"
    };

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
        <div className="min-details">
            <div className="exit">
                <FontAwesomeIcon
                    icon={faXmark}
                    onClick={() => setItemCount(minCount)}
                />
            </div>
            <div className="title">
                <div>{name}</div>
            </div>
            <ul>
                {sortedPlayerData?.slice(0, itemCount).map((player: CombatPlayerType) => (
                    <li key={player.id}>
                        <div className="min-details__title">
                            <div className="username">{player.username.split('-')[0]}</div>
                            <div className="min-details__values">
                                <div className="actual-value">{getDetailsValue(player)}</div>
                                <div className="value-per-time">{calculationValuePerTime(player, dashboardDetailsType[detailsType])} {getValuePerTimeType()}</div>
                            </div>
                        </div>
                        <div className="player-progress" onClick={() => goToCombatGeneralDetails(player.id)}>
                            <div className="progress">
                                <div className="progress-bar" role="progressbar" style={{ width: calculation(player, dashboardDetailsType[detailsType]) + '%' }}></div>
                            </div>
                        </div>
                        <div className="procentage">
                            <div className="player-contribution">{calculation(player, dashboardDetailsType[detailsType])}%</div>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default memo(DashboardMinDetails);