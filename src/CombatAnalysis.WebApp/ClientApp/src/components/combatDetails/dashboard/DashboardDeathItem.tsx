import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useLazyGetDamageTakenByCombatPlayerIdQuery } from '../../../store/api/combatParser/DamageTaken.api';
import { DashboardDeathItemProps } from "../../../types/components/combatDetails/dashboard/DashboardDeathItemProps";

const minCount = 3;

const DashboardDeathItem: React.FC<DashboardDeathItemProps> = ({ playersDeath, combatPlayers }) => {
    const { t } = useTranslation("combatDetails/dashboard");

    const [getDamageTakenByCombatPlayerId] = useLazyGetDamageTakenByCombatPlayerIdQuery();

    const [itemCount, setItemCount] = useState(minCount);

    useEffect(() => {
        if (!playersDeath || playersDeath.length === 0) {
            return;
        }
    }, [playersDeath, combatPlayers, getDamageTakenByCombatPlayerId]);

    if (playersDeath === undefined) {
        return (<div>Loading...</div>);
    }

    return (
        <>
            <div>{t("PlayersDied")}</div>
            <ul className="death-info">
                {playersDeath?.slice(0, itemCount).map((death, index) => (
                    <li key={index} className="death-info__details">
                        <div>{death?.username}</div>
                        <div>{death?.lastHitSpellOrItem}</div>
                        <div>{death?.lastHitValue}</div>
                    </li>
                ))}
            </ul>
            <div className="extend" onClick={() => setItemCount(itemCount === minCount ? playersDeath.length : minCount)}>
                {itemCount === minCount ? t("More") : t("Less")}
            </div>
        </>
    );
}

export default DashboardDeathItem;