import { memo, useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { CombatPlayerType } from "../../../types/components/combatDetails/CombatPlayerType";
import { DashboardItemProps } from "../../../types/components/combatDetails/dashboard/DashboardItemProps";
import DashboardMinDetails from './DashboardMinDetails';
import TopPlayers from './TopPlayers';

const DashboardItem: React.FC<DashboardItemProps> = ({ name, details, duration, combatPlayers, detailsType, getValueShortName }) => {
    const minCount = 4;

    const { t } = useTranslation("combatDetails/dashboard");

    const navigate = useNavigate();

    const [sum, setSum] = useState(0);
    const [itemCount, setItemCount] = useState(minCount);
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

    const calculation = (player: any, typeOfResource: string): string => {
        const typeOfResourceValue = player[typeOfResource];

        if (!typeOfResourceValue) {
            return '0';
        }

        const playerContribution: number = typeOfResourceValue / sum;
        const playerContributionFixed: string = (playerContribution * 100).toFixed(2);

        return playerContributionFixed;
    }

    const calculationValuePerTime = (player: any, typeOfResource: string): string => {
        const valuePerTime = +player[typeOfResource] / duration;
        const shortValue = getValueShortName(parseInt(valuePerTime.toFixed(2)));

        return shortValue;
    }

    const calculateSum = (key: string): number => {
        const reducedPlayers = combatPlayers.reduce((acc, player: any) => acc + player[key], 0);

        return reducedPlayers;
    }

    const sortByKey = (players: CombatPlayerType[], key: string): CombatPlayerType[] => {
        const sortedPlayers = [...players].sort((playerA: any, playerB: any) => playerB[key] - playerA[key]);

        return sortedPlayers;
    }

    const getDetailsValue = (player: any): string => {
        const detailsMapping = ["damageDone", "healDone", "damageTaken", "resourcesRecovery"];
        const shortValue = getValueShortName(player[detailsMapping[detailsType]]) || '0';

        return shortValue;
    }

    const goToCombatGeneralDetails = (playerId: number): void => {
        navigate(`/combat-details?id=${details.id}&playerId=${playerId}&detailsType=${detailsType}&combatLogId=${details.combatLogId}&name=${details.name}&tab=${0}&number=${details.number}&isWin=${details.isWin}`);
    }

    return (
        <>
            <div className="title">
                <div>{name}</div>
            </div>
            <TopPlayers
                calculation={calculation}
                calculationValuePerTime={calculationValuePerTime}
                goToCombatGeneralDetails={goToCombatGeneralDetails}
                getDetailsValue={getDetailsValue}
                sortedPlayerData={sortedPlayerData}
                detailsType={detailsType}
            />
            {itemCount !== minCount &&
                <DashboardMinDetails
                    name={name}
                    calculation={calculation}
                    calculationValuePerTime={calculationValuePerTime}
                    goToCombatGeneralDetails={goToCombatGeneralDetails}
                    getDetailsValue={getDetailsValue}
                    sortedPlayerData={sortedPlayerData}
                    detailsType={detailsType}
                    itemCount={itemCount}
                    setItemCount={setItemCount}
                />
            }
            {itemCount === minCount
                ? <div className="extend" onClick={() => setItemCount(sortedPlayerData.length)}>{t("More")}</div>
                : <div className="extend" onClick={() => setItemCount(minCount)}>{t("Less")}</div>
            }
        </>
    );
}

export default memo(DashboardItem);