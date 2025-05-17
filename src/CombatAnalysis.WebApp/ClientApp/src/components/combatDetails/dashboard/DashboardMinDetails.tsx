import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo } from "react";
import { useNavigate } from 'react-router-dom';
import { CombatPlayerType } from "../../../types/components/combatDetails/CombatPlayerType";
import { DashboardMinDetailsProps } from '../../../types/components/combatDetails/dashboard/DashboardMinDetailsProps';

const DashboardMinDetails: React.FC<DashboardMinDetailsProps> = ({ name, calculation, getDetailsValue, sortedPlayerData, details, detailsType, itemCount, setItemCount }) => {
    const minCount = 4;

    const navigate = useNavigate();

    const dashboardDetailsType: any = {
        0: "damageDone",
        1: "healDone",
        2: "damageTaken",
        3: "resourcesRecovery"
    };

    const goToCombatGeneralDetails = (playerId: number) => {
        navigate(`/combat-details?id=${details.id}&playerId=${playerId}&detailsType=${detailsType}&combatLogId=${details.combatLogId}&name=${details.name}&tab=${0}&number=${details.number}&isWin=${details.isWin}`);
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
                                <div className="player-contribution">{calculation(player, dashboardDetailsType[detailsType])}%</div>
                            </div>
                        </div>
                        <div className="player-statistics">
                            <div className="progress" onClick={() => goToCombatGeneralDetails(player.id)}>
                                <div className="progress-bar" role="progressbar" style={{ width: calculation(player, dashboardDetailsType[detailsType]) + '%' }}></div>
                            </div>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default memo(DashboardMinDetails);