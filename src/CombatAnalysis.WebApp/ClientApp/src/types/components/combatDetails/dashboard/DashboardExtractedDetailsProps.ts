import { SetStateAction } from "react";
import { CombatPlayerType } from "../CombatPlayerType";

export interface DashboardExtractedDetailsProps {
    name: string;
    calculation(player: any, typeOfResource: string): string;
    calculationValuePerTime(player: any, typeOfResource: string): string;
    goToCombatGeneralDetails(playerId: number): void;
    getDetailsValue(player: any): any;
    combatPlayers: CombatPlayerType[];
    detailsType: number;
    itemCount: number;
    setItemCount(value: SetStateAction<number>): void;
}