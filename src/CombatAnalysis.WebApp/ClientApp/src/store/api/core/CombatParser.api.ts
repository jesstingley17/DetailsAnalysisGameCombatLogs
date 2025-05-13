import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { CombatAuraType } from '../../../types/components/combatDetails/CombatAuraType';
import { CombatLogType } from '../../../types/components/combatDetails/CombatLogType';
import { CombatPlayerPositionType } from '../../../types/components/combatDetails/CombatPlayerPositionType';
import { CombatPlayerType } from '../../../types/components/combatDetails/CombatPlayerType';
import { CombatType } from '../../../types/components/combatDetails/CombatType';
import { PlayerDeathType } from '../../../types/components/combatDetails/PlayerDeathType';

const apiURL = '/api/v1';

export const CombatParserApi = createApi({
    reducerPath: 'combatParserAPi',
    tagTypes: [
        'CombatLog',
        'PlayerDeath',
        'Combat',
        'CombatPlayer',
        'CombatPlayerPosition',
        'CombatAura',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getCombatLogs: builder.query<CombatLogType[], void>({
            query: () => '/CombatLog',
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CombatLog' as const, id })), 'CombatLog']
                    : ['CombatLog'],
        }),
        getPlayersDeathByPlayerId: builder.query<PlayerDeathType[], number>({
            query: (combatPlayerId: number) => `/PlayerDeath/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'PlayerDeath' as const, id })), 'PlayerDeath']
                    : ['PlayerDeath'],
        }),
        getCombatsByCombatLogId: builder.query<CombatType[], number>({
            query: (combatLogId: number) => `/Combat/getByCombatLogId/${combatLogId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Combat' as const, id })), 'Combat']
                    : ['Combat'],
        }),
        getCombatPlayersByCombatId: builder.query<CombatPlayerType[], number>({
            query: (combatId: number) => `/CombatPlayer/getByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CombatPlayer' as const, id })), 'CombatPlayer']
                    : ['CombatPlayer'],
        }),
        getCombatPlayerById: builder.query<CombatPlayerType, number>({
            query: (id: number) => `/CombatPlayer/${id}`,
            providesTags: result =>
                result
                    ? [{ type: 'CombatPlayer' as const, id: result.id }]
                    : ['CombatPlayer'],
        }),
        getCombatById: builder.query<CombatType, number>({
            query: (id: number) => `/Combat/${id}`,
            providesTags: result =>
                result
                    ? [{ type: 'Combat' as const, id: result.id }]
                    : ['Combat'],
        }),
        getCombatPlayerPositionsByCombatId: builder.query<CombatPlayerPositionType[], number>({
            query: (combatId: number) => `/CombatPlayerPosition/getByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CombatPlayerPosition' as const, id })), 'CombatPlayerPosition']
                    : ['CombatPlayerPosition'],
        }),
        getCombatAurasByCombatId: builder.query<CombatAuraType[], number>({
            query: (combatId: number) => `/CombatAura/getByCombatId/${combatId}`,
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
} = CombatParserApi;