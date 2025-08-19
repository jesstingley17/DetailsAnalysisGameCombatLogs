import type { DamageTakenGeneralModel } from '../types/DamageTakenGeneralModel';
import type { DamageTakenModel } from '../types/DamageTakenModel';
import { GameLogsApi } from './GameLogs.api';

export const DamageTakenApi = GameLogsApi.injectEndpoints({
    endpoints: builder => ({
        getDamageTakenByCombatPlayerId: builder.query<DamageTakenModel[], { combatPlayerId: number, page: number, pageSize: number }>({
            query: ({ combatPlayerId, page, pageSize }) => `/DamageTaken/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageTaken' as const, id })), 'DamageTaken']
                    : ['DamageTaken'],
        }),
        getDamageTakenCountByCombatPlayerId: builder.query<number, number>({
            query: combatPlayerId => `/DamageTaken/count/${combatPlayerId}`,
        }),
        getDamageTakenUniqueFilterValues: builder.query<string[], { combatPlayerId: number, filter: string }>({
            query: ({ combatPlayerId, filter }) => `/DamageTaken/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
        }),
        getDamageTakenByFilter: builder.query<DamageTakenModel[], { combatPlayerId: number, filter: string, filterValue: number, page: number, pageSize: number }>({
            query: ({ combatPlayerId, filter, filterValue, page, pageSize }) => `/DamageTaken/getByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageTaken' as const, id })), 'DamageTaken']
                    : ['DamageTaken'],
        }),
        getDamageTakenCountByFilter: builder.query<number, { combatPlayerId: number, filter: string, filterValue: number }>({
            query: ({ combatPlayerId, filter, filterValue }) => `/DamageTaken/countByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}`,
        }),
        getDamageTakenGeneralByCombatPlayerId: builder.query<DamageTakenGeneralModel[], number>({
            query: combatPlayerId => `/DamageTakenGeneral/getByCombatPlayerId/${combatPlayerId}`,
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