import type { HealDoneGeneralModel } from '../types/HealDoneGeneralModel';
import type { HealDoneModel } from '../types/HealDoneModel';
import { GameLogsApi } from './GameLogs.api';

export const HealDoneApi = GameLogsApi.injectEndpoints({
    endpoints: builder => ({
        getHealDoneByCombatPlayerId: builder.query<HealDoneModel[], { combatPlayerId: number, page: number, pageSize: number }>({
            query: ({ combatPlayerId, page, pageSize }) => `/HealDone/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'HealDone' as const, id })), 'HealDone']
                    : ['HealDone'],
        }),
        getHealDoneCountByCombatPlayerId: builder.query<number, number>({
            query: combatPlayerId => `/HealDone/count/${combatPlayerId}`,
        }),
        getHealDoneUniqueFilterValues: builder.query<string[], { combatPlayerId: number, filter: number }>({
            query: ({ combatPlayerId, filter }) => `/HealDone/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
        }),
        getHealDoneByFilter: builder.query<HealDoneModel[], { combatPlayerId: number, filter: number, filterValue: number, page: number, pageSize: number }>({
            query: ({ combatPlayerId, filter, filterValue, page, pageSize }) => `/HealDone/getByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'HealDone' as const, id })), 'HealDone']
                    : ['HealDone'],
        }),
        getHealDoneCountByFilter: builder.query<number[], { combatPlayerId: number, filter: number, filterValue: number }>({
            query: ({ combatPlayerId, filter, filterValue }) => `/HealDone/countByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}`,
        }),
        getHealDoneGeneralByCombatPlayerId: builder.query<HealDoneGeneralModel[], number>({
            query: combatPlayerId => `/HealDoneGeneral/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'HealDoneGeneral' as const, id })), 'HealDoneGeneral']
                    : ['HealDoneGeneral'],
        }),
    })
})

export const {
    useGetHealDoneByCombatPlayerIdQuery,
    useLazyGetHealDoneCountByCombatPlayerIdQuery,
    useGetHealDoneUniqueFilterValuesQuery,
    useGetHealDoneByFilterQuery,
    useGetHealDoneCountByFilterQuery,
    useGetHealDoneGeneralByCombatPlayerIdQuery,
    useLazyGetHealDoneGeneralByCombatPlayerIdQuery,
} = HealDoneApi;