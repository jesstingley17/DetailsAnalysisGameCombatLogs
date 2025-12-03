import { memo, useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import type { CombatDetailsModel } from '../../types/dashboard/CombatDetailsModel';
import DashboardExtractedDetails from './DashboardExtractedDetails';
import TopPlayers from './TopPlayers';

export interface DashboardGeneralItemProps {
    name: string;
    details: CombatDetailsModel;
    duration: number;
    combatPlayers: CombatPlayerModel[];
    detailsType: number;
    getValueShortName(value: number): string;
}

const DashboardGeneralItem: React.FC<DashboardGeneralItemProps> = ({ name, details, duration, combatPlayers, detailsType, getValueShortName }) => {
    const minCount = 4;
    const topPlayersCount = 3;

    const { t } = useTranslation('combatDetails/dashboard');

    const navigate = useNavigate();

    const [sum, setSum] = useState(0);
    const [itemCount, setItemCount] = useState(minCount);
    const [sortedPlayerData, setSortedPlayerData] = useState<CombatPlayerModel[]>([]);

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
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

    const calculation = (player: CombatPlayerModel, typeOfResource: keyof CombatPlayerModel): string => {
        const typeOfResourceValue: number = +player[typeOfResource];

        if (!typeOfResourceValue) {
            return '0';
        }

        const playerContribution: number = typeOfResourceValue / sum;
        const playerContributionFixed: string = (playerContribution * 100).toFixed(2);

        return playerContributionFixed;
    }

    const calculationValuePerTime = (player: CombatPlayerModel, typeOfResource: keyof CombatPlayerModel): string => {
        const valuePerTime = +player[typeOfResource] / duration;
        const shortValue = getValueShortName(parseInt(valuePerTime.toFixed(2)));

        return shortValue;
    }

    const calculateSum = (key: keyof CombatPlayerModel): number => {
        const sum = combatPlayers.reduce((acc, player: CombatPlayerModel) => acc + +player[key], 0);

        return sum;
    }

    const sortByKey = (players: CombatPlayerModel[], key: keyof CombatPlayerModel): CombatPlayerModel[] => {
        const sortedPlayers = [...players].sort((playerA: CombatPlayerModel, playerB: CombatPlayerModel) => +playerB[key] - +playerA[key]);

        return sortedPlayers;
    }

    const getDetailsValue = (player: CombatPlayerModel): string => {
        const keys: (keyof CombatPlayerModel)[] = ['damageDone', 'healDone', 'damageTaken', 'resourcesRecovery',];
        const key = keys[detailsType];

        const shortValue = getValueShortName(+player[key]) || '0';

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
                topPlayers={sortedPlayerData.slice(0, topPlayersCount)}
                detailsType={detailsType}
            />
            {itemCount !== minCount &&
                <DashboardExtractedDetails
                    name={name}
                    calculation={calculation}
                    calculationValuePerTime={calculationValuePerTime}
                    goToCombatGeneralDetails={goToCombatGeneralDetails}
                    getDetailsValue={getDetailsValue}
                    combatPlayers={sortedPlayerData}
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

export default memo(DashboardGeneralItem);