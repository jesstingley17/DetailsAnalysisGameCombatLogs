import { DamageDoneGeneralType } from "../../../types/components/combatDetails/DamageDoneGeneralType";
import { DamageDoneType } from "../../../types/components/combatDetails/DamageDoneType";
import { CombatTargetType } from "../../../types/components/combatDetails/dashboard/CombatTargetType";
import { CombatParserApi } from "../core/CombatParser.api";

export const DamageDoneApi = CombatParserApi.injectEndpoints({
    endpoints: builder => ({
        getDamageDoneByCombatPlayerId: builder.query<DamageDoneType[], { combatPlayerId: number, page: number, pageSize: number }>({
            query: ({ combatPlayerId, page, pageSize }) => ({
                url: `/DamageDone/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageDone' as const, id })), 'DamageDone']
                    : ['DamageDone'],
        }),
        getDamageDoneCountByCombatPlayerId: builder.query<number, number>({
            query: (combatPlayerId) => `/DamageDone/count/${combatPlayerId}`,
        }),
        getDamageDoneUniqueFilterValues: builder.query<string[], { combatPlayerId: number, filter: number }>({
            query: ({ combatPlayerId, filter }) => ({
                url: `/DamageDone/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
            }),
        }),
        getDamageDoneByFilter: builder.query<DamageDoneType[], { combatPlayerId: number, filter: number, filterValue: number, page: number, pageSize: number }>({
            query: ({ combatPlayerId, filter, filterValue, page, pageSize }) => ({
                url: `/DamageDone/getByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageDone' as const, id })), 'DamageDone']
                    : ['DamageDone'],
        }),
        getDamageDoneValueByTarget: builder.query<number, { combatPlayerId: number, target: string }>({
            query: ({ combatPlayerId, target }) => ({
                url: `/DamageDone/getValueByTarget?combatPlayerId=${combatPlayerId}&target=${target}`,
            }),
        }),
        getDamageDoneDamageByEachTarget: builder.query<Array<CombatTargetType[]>, number>({
            query: (combatId) => ({
                url: `/DamageDone/getDamageByEachTarget/${combatId}`,
            }),
        }),
        getDamageDoneCountByFilter: builder.query<number[], { combatPlayerId: number, filter: number, filterValue: number }>({
            query: ({ combatPlayerId, filter, filterValue }) => `/DamageDone/countByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}`,
        }),
        getDamageDoneGeneralByCombatPlayerId: builder.query<DamageDoneGeneralType[], number>({
            query: (combatPlayerId) => `/DamageDoneGeneral/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageDoneGeneral' as const, id })), 'DamageDoneGeneral']
                    : ['DamageDoneGeneral'],
        }),
    })
})

export const {
    useGetDamageDoneByCombatPlayerIdQuery,
    useLazyGetDamageDoneCountByCombatPlayerIdQuery,
    useLazyGetDamageDoneUniqueFilterValuesQuery,
    useGetDamageDoneUniqueFilterValuesQuery,
    useGetDamageDoneValueByTargetQuery,
    useLazyGetDamageDoneValueByTargetQuery,
    useGetDamageDoneByFilterQuery,
    useLazyGetDamageDoneDamageByEachTargetQuery,
    useGetDamageDoneCountByFilterQuery,
    useGetDamageDoneGeneralByCombatPlayerIdQuery,
    useLazyGetDamageDoneGeneralByCombatPlayerIdQuery,
} = DamageDoneApi;