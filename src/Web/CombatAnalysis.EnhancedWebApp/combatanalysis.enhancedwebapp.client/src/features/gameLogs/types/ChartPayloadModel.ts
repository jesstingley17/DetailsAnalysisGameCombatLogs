import type { CombatPlayerPropertyModel } from './CombatPlayerPropertyModel';

export type ChartPayloadModel = {
    title: string;
    color: string;
    data: Array<CombatPlayerPropertyModel>;
}