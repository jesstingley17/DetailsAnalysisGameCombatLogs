import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { CombatAuraModel } from '../types/CombatAuraModel';
import type { CombatLogModel } from '../types/CombatLogModel';
import type { CombatModel } from '../types/CombatModel';
import type { CombatPlayerModel } from '../types/CombatPlayerModel';
import type { CombatPlayerPositionModel } from '../types/CombatPlayerPositionModel';
import type { PlayerDeathModel } from '../types/PlayerDeathModel';

const apiURL = '/api/v1';

export const GameLogsApi = createApi({
    reducerPath: 'combatParserAPi',
    tagTypes: [
        'CombatLog',
        'PlayerDeath',
        'Combat',
        'CombatPlayer',
        'CombatPlayerPosition',
        'CombatAura',
        'DamageDone',
        'DamageDoneGeneral',
        'DamageTaken',
        'DamageTakenGeneral',
        'HealDone',
        'HealDoneGeneral',
        'ResourceRecovery',
        'ResourceRecoveryGeneral',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getCombatLogs: builder.query<CombatLogModel[], void>({
            query: () => '/CombatLog',
            providesTags: (result) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CombatLog' as const, id })), 'CombatLog']
                    : ['CombatLog'],
        }),
        getPlayersDeathByPlayerId: builder.query<PlayerDeathModel[], number>({
            query: combatPlayerId => `/PlayerDeath/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'PlayerDeath' as const, id })), 'PlayerDeath']
                    : ['PlayerDeath'],
        }),
        getCombatsByCombatLogId: builder.query<CombatModel[], number>({
            query: combatLogId => `/Combat/getByCombatLogId/${combatLogId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Combat' as const, id })), 'Combat']
                    : ['Combat'],
        }),
        getCombatPlayersByCombatId: builder.query<CombatPlayerModel[], number>({
            query: combatId => `/CombatPlayer/getByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CombatPlayer' as const, id })), 'CombatPlayer']
                    : ['CombatPlayer'],
        }),
        getCombatPlayerById: builder.query<CombatPlayerModel, number>({
            query: id => `/CombatPlayer/${id}`,
            providesTags: result =>
                result
                    ? [{ type: 'CombatPlayer' as const, id: result.id }]
                    : ['CombatPlayer'],
        }),
        getCombatById: builder.query<CombatModel, number>({
            query: id => `/Combat/${id}`,
            providesTags: result =>
                result
                    ? [{ type: 'Combat' as const, id: result.id }]
                    : ['Combat'],
        }),
        getCombatPlayerPositionsByCombatId: builder.query<CombatPlayerPositionModel[], number>({
            query: combatId => `/CombatPlayerPosition/getByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CombatPlayerPosition' as const, id })), 'CombatPlayerPosition']
                    : ['CombatPlayerPosition'],
        }),
        getCombatAurasByCombatId: builder.query<CombatAuraModel[], number>({
            query: combatId => `/CombatAura/getByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CombatAura' as const, id })), 'CombatAura']
                    : ['CombatAura'],
        }),
    })
})

export const {
    useGetCombatLogsQuery,
    useLazyGetPlayersDeathByPlayerIdQuery,
    useLazyGetCombatsByCombatLogIdQuery,
    useLazyGetCombatPlayersByCombatIdQuery,
    useLazyGetCombatPlayerByIdQuery,
    useLazyGetCombatByIdQuery,
    useLazyGetCombatPlayerPositionsByCombatIdQuery,
    useLazyGetCombatAurasByCombatIdQuery,
} = GameLogsApi;