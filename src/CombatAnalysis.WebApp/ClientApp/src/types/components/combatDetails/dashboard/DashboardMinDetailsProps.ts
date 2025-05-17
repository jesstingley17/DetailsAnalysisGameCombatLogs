import { SetStateAction } from "react";
import { CombatDetailsType } from "../CombatDetailsType";
import { CombatPlayerType } from "../CombatPlayerType";

export interface DashboardMinDetailsProps {
    name: string;
    calculation: (player: any, typeOfResource: string) => string;
    getDetailsValue: (player: any) => any;
    sortedPlayerData: CombatPlayerType[];
    details: CombatDetailsType;
    detailsType: number;
    itemCount: number,
    setItemCount: (value: SetStateAction<number>) => void,
}