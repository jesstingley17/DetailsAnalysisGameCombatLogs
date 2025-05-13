import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { CombatPlayerType } from "../../../types/components/combatDetails/CombatPlayerType";
import { DashboardItemProps } from "../../../types/components/combatDetails/dashboard/DashboardItemProps";
import DashboardMinDetails from "../DashboardMinDetails";

const DashboardProjectItem: React.FC<DashboardItemProps> = ({ name, details, combatPlayers, detailsType, getValueShortName }) => {
    const minCount = 4;

    const { t } = useTranslation("combatDetails/dashboard");

    const navigate = useNavigate();

    const [sum, setSum] = useState(0);
    const [itemCount, setItemCount] = useState(minCount);
    const [selectedPlayerIndex, setSelectedPlayerIndex] = useState(-1);
    const [sortedPlayerData, setSortedPlayerData] = useState<CombatPlayerType[]>([]);

    const dashboardDetailsType: any = {
        0: "damageDone",
        1: "healDone",
        2: "damageTaken",
        3: "resourcesRecovery"
    };

    useEffect(() => {
        const sum = calculateSum(dashboardDetailsType[detailsType]);
        setSum(sum);

        const data = sortByKey(combatPlayers, dashboardDetailsType[detailsType]);
        setSortedPlayerData(data);
    }, []);

    const closeMinDetailsHandle = (e: any) => {
        setSelectedPlayerIndex(-1);
    }

    const calculation = (player: any, typeOfResource: string): number => {
        const typeOfResourceValue = player[typeOfResource];

        if (!typeOfResourceValue) {
            return 0;
        }

        const playerContribution: number = typeOfResourceValue / sum;
        const playerContributionFixed: string = (playerContribution * 100).toFixed(2);

        return parseFloat(playerContributionFixed);
    }

    const calculateSum = (key: string) => {
        const reducedPlayers = combatPlayers.reduce((acc, player: any) => acc + player[key], 0);

        return reducedPlayers;
    }

    const sortByKey = (players: CombatPlayerType[], key: string): CombatPlayerType[] => {
        const sortedPlayers = [...players].sort((playerA: any, playerB: any) => playerB[key] - playerA[key]);

        return sortedPlayers;
    }

    const getDetailsValue = (player: any) => {
        const detailsMapping = ["damageDone", "healDone", "damageTaken", "resourcesRecovery"];
        const shortValue = getValueShortName(player[detailsMapping[detailsType]]) || 0;

        return shortValue;
    }

    const goToCombatGeneralDetails = (playerId: number) => {
        navigate(`/combat-details?id=${details.id}&playerId=${playerId}&detailsType=${detailsType}&combatLogId=${details.combatLogId}&name=${details.name}&tab=${0}&number=${details.number}&isWin=${details.isWin}`);
    }

    return (
        <>
            <div className="title">
                <div>{name}</div>
            </div>
            <ul className="players-progress">
                {sortedPlayerData?.slice(0, itemCount).map((player: CombatPlayerType, index: number) => (
                    <li key={player.id}>
                        {(selectedPlayerIndex === index) &&
                            <DashboardMinDetails
                                combatPlayerId={player.id}
                                closeHandle={closeMinDetailsHandle}
                                detailsType={detailsType}
                                getValueShortName={getValueShortName}
                            />
                        }
                        <div className="title">
                            <div className="username">{player.username}</div>
                            <div className="value">{getDetailsValue(player)}</div>
                        </div>
                        <div className="player-statistics">
                            <div className="progress" onClick={() => goToCombatGeneralDetails(player.id)}>
                                <div className="progress-bar" role="progressbar" style={{ width: calculation(player, dashboardDetailsType[detailsType]) + '%' }}></div>
                            </div>
                            <div className="player-contribution">{calculation(player, dashboardDetailsType[detailsType])}%</div>
                        </div>
                    </li>
                ))}
            </ul>
            {itemCount === minCount
                ? <div className="extend" onClick={() => setItemCount(sortedPlayerData.length)}>{t("More")}</div>
                : <div className="extend" onClick={() => setItemCount(minCount)}>{t("Less")}</div>
            }
        </>
    );
}

export default DashboardProjectItem;