import { DamageTakenGeneralType } from "../../../types/components/combatDetails/DamageTakenGeneralType";
import { DamageTakenType } from "../../../types/components/combatDetails/DamageTakenType";
import { CombatParserApi } from "../core/CombatParser.api";

export const DamageTakenApi = CombatParserApi.injectEndpoints({
    endpoints: builder => ({
        getDamageTakenByCombatPlayerId: builder.query<DamageTakenType[], { combatPlayerId: number, page: number, pageSize: number }>({
            query: ({ combatPlayerId, page, pageSize }) => ({
                url: `/DamageTaken/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageTaken' as const, id })), 'DamageTaken']
                    : ['DamageTaken'],
        }),
        getDamageTakenCountByCombatPlayerId: builder.query<number, number>({
            query: (combatPlayerId) => `/DamageTaken/count/${combatPlayerId}`,
        }),
        getDamageTakenUniqueFilterValues: builder.query<string[], { combatPlayerId: number, filter: number }>({
            query: ({ combatPlayerId, filter }) => ({
                url: `/DamageTaken/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
            }),
        }),
        getDamageTakenByFilter: builder.query<DamageTakenType[], { combatPlayerId: number, filter: number, filterValue: number, page: number, pageSize: number }>({
            query: ({ combatPlayerId, filter, filterValue, page, pageSize }) => ({
                url: `/DamageTaken/getByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageTaken' as const, id })), 'DamageTaken']
                    : ['DamageTaken'],
        }),
        getDamageTakenCountByFilter: builder.query<number[], { combatPlayerId: number, filter: number, filterValue: number }>({
            query: ({ combatPlayerId, filter, filterValue }) => `/DamageTaken/countByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}`,
        }),
        getDamageTakenGeneralByCombatPlayerId: builder.query<DamageTakenGeneralType[], number>({
            query: (combatPlayerId) => `/DamageTakenGeneral/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageTakenGeneral' as const, id })), 'DamageTakenGeneral']
                    : ['DamageTakenGeneral'],
        }),
    })
})

export const {
    useGetDamageTakenByCombatPlayerIdQuery,
    useLazyGetDamageTakenByCombatPlayerIdQuery,
    useLazyGetDamageTakenCountByCombatPlayerIdQuery,
    useGetDamageTakenUniqueFilterValuesQuery,
    useGetDamageTakenByFilterQuery,
    useGetDamageTakenCountByFilterQuery,
    useGetDamageTakenGeneralByCombatPlayerIdQuery,
    useLazyGetDamageTakenGeneralByCombatPlayerIdQuery,
} = DamageTakenApi;