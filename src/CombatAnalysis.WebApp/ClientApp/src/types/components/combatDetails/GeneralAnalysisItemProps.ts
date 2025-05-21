import { CombatType } from "./CombatType";

export interface GeneralAnalysisItemProps {
    uniqueCombats: CombatType[];
    combatLogId: number;
    getValueShortName: (value: number) => string;
}