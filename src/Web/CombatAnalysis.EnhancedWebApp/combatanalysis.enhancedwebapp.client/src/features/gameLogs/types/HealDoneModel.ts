export type HealDoneModel = {
    id: number;
    spell: string;
    value: number;
    overheal: number;
    time: string;
    creator: string;
    target: string;
    isCrit: boolean;
    isAbsorbed: boolean;
    combatPlayerId: number;
}