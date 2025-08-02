import { CombatDetailsType } from "../CombatDetailsType";
import { CombatTargetType } from "./CombatTargetType";

export interface DashboardTargetItemProps {
    combatTarget: CombatTargetType[];
    details: CombatDetailsType;
    duration: number;
    detailsType: number;
    getValueShortName(value: number): string;
}