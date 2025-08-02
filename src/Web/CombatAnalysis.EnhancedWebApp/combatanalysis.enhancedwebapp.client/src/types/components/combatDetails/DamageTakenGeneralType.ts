export interface DamageTakenGeneralType {
    id: number;
    spell: string;
    value: number;
    actualValue: number;
    damageTakenPerSecond: number;
    critNumber: number;
    missNumber: number;
    castNumber: number;
    minValue: number;
    maxValue: number;
    averageValue: number;
    combatPlayerId: number;
}