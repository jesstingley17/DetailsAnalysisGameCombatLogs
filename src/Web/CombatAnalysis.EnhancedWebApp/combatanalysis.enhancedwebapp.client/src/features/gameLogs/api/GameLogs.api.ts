import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { CombatAuraModel } from '../types/CombatAuraModel';
import type { CombatLogModel } from '../types/CombatLogModel';
import type { CombatModel } from '../types/CombatModel';
import type { CombatPlayerModel } from '../types/CombatPlayerModel';
import type { CombatPlayerDeathModel } from '../types/CombatPlayerDeathModel';

const apiURL = '/api/v1';

export const GameLogsApi = createApi({
    reducerPath: 'combatParserAPi',
    tagTypes: [
        'CombatLog',
        'Combat',
        'CombatPlayer',
        'CombatAura',
        'DamageDone',
        'DamageDoneGeneral',
        'DamageTaken',
        'DamageTakenGeneral',
        'HealDone',
        'HealDoneGeneral',
        'ResourceRecovery',
        'ResourceRecoveryGeneral',
        'PlayerDeath',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getCombatLogs: builder.query<CombatLogModel[], void>({
            query: () => '/CombatLog',
            providesTags: result =>
                result
                    ? [
                        ...result.map(combatLog => ({ type: 'CombatLog' as const, id: combatLog.id })),
                        { type: 'CombatLog', id: 'LIST' },
                    ]
                    : [{ type: 'CombatLog', id: 'LIST' }]
        }),
        getPlayersDeathByPlayerId: builder.query<CombatPlayerDeathModel[], number>({
            query: combatPlayerId => `/PlayerDeath/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(playerDeath => ({ type: 'PlayerDeath' as const, id: playerDeath.id })),
                        { type: 'PlayerDeath', id: 'LIST' },
                    ]
                    : [{ type: 'PlayerDeath', id: 'LIST' }]
        }),
        getCombatsByCombatLogId: builder.query<CombatModel[], number>({
            query: combatLogId => `/Combat/getByCombatLogId/${combatLogId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(combat => ({ type: 'Combat' as const, id: combat.id })),
                        { type: 'Combat', id: 'LIST' },
                    ]
                    : [{ type: 'Combat', id: 'LIST' }]
        }),
        getCombatPlayersByCombatId: builder.query<CombatPlayerModel[], number>({
            query: combatId => `/CombatPlayer/getByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(combatPlayer => ({ type: 'CombatPlayer' as const, id: combatPlayer.id })),
                        { type: 'CombatPlayer', id: 'LIST' },
                    ]
                    : [{ type: 'CombatPlayer', id: 'LIST' }]
        }),
        getCombatPlayerById: builder.query<CombatPlayerModel, number>({
            query: id => `/CombatPlayer/${id}`,
            providesTags: result => result ? [{ type: 'CombatPlayer', id: result.id }] : [],
        }),
        getCombatById: builder.query<CombatModel, number>({
            query: id => `/Combat/${id}`,
            providesTags: result => result ? [{ type: 'Combat', id: result.id }] : [],
        }),
        getCombatAurasByCombatId: builder.query<CombatAuraModel[], number>({
            query: combatId => `/CombatAura/getByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(combatAura => ({ type: 'CombatAura' as const, id: combatAura.id })),
                        { type: 'CombatAura', id: 'LIST' },
                    ]
                    : [{ type: 'CombatAura', id: 'LIST' }]
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
    useLazyGetCombatAurasByCombatIdQuery,
} = GameLogsApi;