import { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyGetCombatByIdQuery } from '../../../store/api/core/CombatParser.api';
import { CombatType } from "../../../types/components/combatDetails/CombatType";
import { DashboardProps } from "../../../types/components/combatDetails/dashboard/DashboardProps";
import DashboardDeathItem from "./DashboardDeathItem";
import DashboardItem from "./DashboardItem";

import "../../../styles/dashboard.scss";

const Dashboard: React.FC<DashboardProps> = ({ details, combatPlayers, playersDeath, getValueShortName }) => {
    const { t } = useTranslation("combatDetails/dashboard");

    const dashboardDetailsType = [t("Damage"), t("Healing"), t("DamageTaken"), t("ResourcesRecovery")];

    const [combat, setCombat] = useState<CombatType | null>(null);
    const [duration, setDuration] = useState<number>(1);

    const [getCombatById] = useLazyGetCombatByIdQuery();

    useEffect(() => {
        const getCombat = async () => {
            try {
                const combat = await getCombatById(details.id);
                if (combat.data !== undefined) {
                    setCombat(combat.data);
                }
            } catch (error) {
                console.error("Failed to fetch combat:", error);
            }
        }

        if (details.id > 0) {
            getCombat();
        }
    }, [details.id]);

    useEffect(() => {
        if (combat) {
            const duration = getCombatDurationSeconds();
            setDuration(duration);
        }
    }, [combat]);

    const getCombatDurationSeconds = (): number => {
        const duration = combat?.duration.split(':') || [];

        const hours = parseInt(duration[0] || '0');
        const minutes = parseInt(duration[1] || '0');
        const seconds = parseInt(duration[2] || '0');

        const totalSeconds = (hours * 3600) + (minutes * 60) + seconds;

        return totalSeconds;
    }

    if (!combat || combatPlayers.length === 0) {
        return (<></>);
    }

    return (
        <div className="dashboard">
            <ul className="items">
                {dashboardDetailsType.map((name, index) => (
                    <li key={index} className="dashboard__statistics">
                        <DashboardItem
                            name={name}
                            details={details}
                            duration={duration}
                            combatPlayers={combatPlayers}
                            detailsType={index}
                            getValueShortName={getValueShortName}
                        />
                    </li>
                ))}
                {playersDeath.length > 0 &&
                    <li key={dashboardDetailsType.length} className="dashboard__statistics">
                        <DashboardDeathItem
                            playersDeath={playersDeath}
                            combatPlayers={combatPlayers}
                        />
                    </li>
                }
            </ul>
        </div>
    );
}

export default memo(Dashboard);