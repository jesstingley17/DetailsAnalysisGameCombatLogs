export type ResourceRecoveryGeneralModel = {
    id: number;
    spell: string;
    resourcePerSecond: number;
    value: number;
    castNumber: number;
    minValue: number;
    maxValue: number;
    averageValue: number;
    combatPlayerId: number;
}