import { memo } from "react";
import { useTranslation } from 'react-i18next';
import { DashboardProps } from "../../../types/components/combatDetails/dashboard/DashboardProps";
import DashboardProjectItem from "./DashboardProjectItem";

import "../../../styles/dashboard.scss";
import DashboardDeathItem from "../DashboardDeathItem";

const DashboardProject: React.FC<DashboardProps> = ({ details, combatPlayers, playersDeath, getValueShortName }) => {
    const { t } = useTranslation("combatDetails/dashboard");

    const dashboardDetailsType = [t("Damage"), t("Healing"), t("DamageTaken"), t("ResourcesRecovery")];

    if (combatPlayers.length === 0) {
        return (<></>);
    }

    return (
        <div className="dashboard">
            <ul className="items">
                {dashboardDetailsType.map((name, index) => (
                    <li key={index} className="dashboard__statistics">
                        <DashboardProjectItem
                            name={name}
                            details={details}
                            combatPlayers={combatPlayers}
                            detailsType={index}
                            getValueShortName={getValueShortName}
                        />
                    </li>
                ))}
                <li key={dashboardDetailsType.length} className="dashboard__statistics">
                    <DashboardDeathItem
                        playersDeath={playersDeath}
                        players={combatPlayers}
                    />
                </li>
            </ul>
        </div>
    );
}

export default memo(DashboardProject);