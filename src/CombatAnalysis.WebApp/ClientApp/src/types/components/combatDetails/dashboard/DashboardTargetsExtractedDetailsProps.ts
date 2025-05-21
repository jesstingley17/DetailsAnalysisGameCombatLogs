import { SetStateAction } from "react";
import { CombatTargetType } from "./CombatTargetType";

export interface DashboardTargetsExtractedDetailsProps {
    name: string;
    calculation: (combatTargetPlayerSum: number) => string;
    calculationDamagePerTimeByTarget: (damage: number) => string;
    goToCombatGeneralDetails: (playerId: number) => void;
    getValueShortName: (value: number) => string;
    targets: CombatTargetType[];
    itemCount: number;
    setItemCount: (value: SetStateAction<number>) => void;
}