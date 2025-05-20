import { memo, useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { CombatTargetType } from "../../../types/components/combatDetails/dashboard/CombatTargetType";
import { DashboardTargetItemProps } from "../../../types/components/combatDetails/dashboard/DashboardTargetItemProps";
import TopPlayersByTarget from "./TopPlayersByTarget";

const DashboardTargetItem: React.FC<DashboardTargetItemProps> = ({ combatTarget, details, duration, detailsType, getValueShortName }) => {
    const minCount = 4;
    const topPlayersCount = 3;

    const { t } = useTranslation("combatDetails/dashboard");

    const navigate = useNavigate();

    const [sum, setSum] = useState(0);
    const [itemCount, setItemCount] = useState(minCount);

    useEffect(() => {
        const sum = calculateSum(combatTarget);
        setSum(sum);
    }, []);

    const calculation = (combatTargetPlayerSum: number): string => {
        const playerContribution: number = combatTargetPlayerSum / sum;
        const playerContributionFixed: string = (playerContribution * 100).toFixed(2);

        return playerContributionFixed;
    }

    const calculationDamagePerTimeByTarget = (damage: number): string => {
        const valuePerTime = damage / duration;
        const shortValue = getValueShortName(parseInt(valuePerTime.toFixed(2)));

        return shortValue;
    }

    const calculateSum = (targetPlayers: CombatTargetType[]): number => {
        const sum = targetPlayers.reduce((acc, player: any) => acc + player.sum, 0);

        return sum;
    }

    const goToCombatGeneralDetails = (playerId: number): void => {
        navigate(`/combat-details?id=${details.id}&playerId=${playerId}&detailsType=${detailsType}&combatLogId=${details.combatLogId}&name=${details.name}&tab=${0}&number=${details.number}&isWin=${details.isWin}`);
    }

    return (
        <>
            <div className="title">
                <div>{combatTarget[0].target}</div>
            </div>
            <TopPlayersByTarget
                calculation={calculation}
                calculationDamagePerTimeByTarget={calculationDamagePerTimeByTarget}
                goToCombatGeneralDetails={goToCombatGeneralDetails}
                getValueShortName={getValueShortName}
                targetTopPlayers={combatTarget.slice(0, topPlayersCount)}
            />
            {itemCount === minCount
                ? <div className="extend" onClick={() => setItemCount(combatTarget.length)}>{t("More")}</div>
                : <div className="extend" onClick={() => setItemCount(minCount)}>{t("Less")}</div>
            }
        </>
    );
}

export default memo(DashboardTargetItem);