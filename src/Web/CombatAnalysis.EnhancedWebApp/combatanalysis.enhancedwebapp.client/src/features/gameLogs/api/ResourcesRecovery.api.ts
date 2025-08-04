import type { ResourceRecoveryGeneralModel } from '../types/ResourceRecoveryGeneralModel';
import type { ResourceRecoveryModel } from '../types/ResourceRecoveryModel';
import { GameLogsApi } from './GameLogs.api';

export const ResourcesRecoveryApi = GameLogsApi.injectEndpoints({
    endpoints: builder => ({
        getResourceRecoveryByCombatPlayerId: builder.query<ResourceRecoveryModel[], { combatPlayerId: number, page: number, pageSize: number }>({
            query: ({ combatPlayerId, page, pageSize }) => `/ResourceRecovery/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'ResourceRecovery' as const, id })), 'ResourceRecovery']
                    : ['ResourceRecovery'],
        }),
        getResourceRecoveryCountByCombatPlayerId: builder.query<number, number>({
            query: combatPlayerId => `/ResourceRecovery/count/${combatPlayerId}`,
        }),
        getResourceRecoveryUniqueFilterValues: builder.query<string[], { combatPlayerId: number, filter: number }>({
            query: ({ combatPlayerId, filter }) => `/ResourceRecovery/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
        }),
        getResourceRecoveryByFilter: builder.query<ResourceRecoveryModel[], { combatPlayerId: number, filter: number, filterValue: number, page: number, pageSize: number }>({
            query: ({ combatPlayerId, filter, filterValue, page, pageSize }) => `/ResourceRecovery/getByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'ResourceRecovery' as const, id })), 'ResourceRecovery']
                    : ['ResourceRecovery'],
        }),
        getResourceRecoveryCountByFilter: builder.query<number[], { combatPlayerId: number, filter: number, filterValue: number }>({
            query: ({ combatPlayerId, filter, filterValue }) => `/ResourceRecovery/countByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}`,
        }),
        getResourceRecoveryGeneralByCombatPlayerId: builder.query<ResourceRecoveryGeneralModel[], number>({
            query: combatPlayerId => `/ResourceRecoveryGeneral/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'ResourceRecoveryGeneral' as const, id })), 'ResourceRecoveryGeneral']
                    : ['ResourceRecoveryGeneral'],
        }),
    })
})

export const {
    useGetResourceRecoveryByCombatPlayerIdQuery,
    useLazyGetResourceRecoveryCountByCombatPlayerIdQuery,
    useGetResourceRecoveryUniqueFilterValuesQuery,
    useGetResourceRecoveryByFilterQuery,
    useGetResourceRecoveryCountByFilterQuery,
    useGetResourceRecoveryGeneralByCombatPlayerIdQuery,
    useLazyGetResourceRecoveryGeneralByCombatPlayerIdQuery,
} = ResourcesRecoveryApi;