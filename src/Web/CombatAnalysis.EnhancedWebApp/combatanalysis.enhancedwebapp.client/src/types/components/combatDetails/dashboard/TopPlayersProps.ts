import { CombatPlayerType } from "../CombatPlayerType";

export interface TopPlayersProps {
    calculation(player: any, typeOfResource: string): string;
    calculationValuePerTime(player: any, typeOfResource: string): string;
    goToCombatGeneralDetails(playerId: number): void;
    getDetailsValue(player: any): string;
    topPlayers: CombatPlayerType[];
    detailsType: number;
}