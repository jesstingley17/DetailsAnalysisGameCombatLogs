/* eslint-disable @typescript-eslint/no-unused-vars */
import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { type CombatAuraType } from '../../../types/components/combatDetails/CombatAuraType';
import { type CombatLogType } from '../../../types/components/combatDetails/CombatLogType';
import { type CombatPlayerPositionType } from '../../../types/components/combatDetails/CombatPlayerPositionType';
import { type CombatPlayerType } from '../../../types/components/combatDetails/CombatPlayerType';
import { type CombatType } from '../../../types/components/combatDetails/CombatType';
import { type PlayerDeathType } from '../../../types/components/combatDetails/PlayerDeathType';

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
        'DamageDone',
        'DamageDoneGeneral',
        'DamageTaken',
        'DamageTakenGeneral',
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
            query: combatPlayerId => `/PlayerDeath/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'PlayerDeath' as const, id })), 'PlayerDeath']
                    : ['PlayerDeath'],
        }),
        getCombatsByCombatLogId: builder.query<CombatType[], number>({
            query: combatLogId => `/Combat/getByCombatLogId/${combatLogId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Combat' as const, id })), 'Combat']
                    : ['Combat'],
        }),
        getCombatPlayersByCombatId: builder.query<CombatPlayerType[], number>({
            query: combatId => `/CombatPlayer/getByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CombatPlayer' as const, id })), 'CombatPlayer']
                    : ['CombatPlayer'],
        }),
        getCombatPlayerById: builder.query<CombatPlayerType, number>({
            query: id => `/CombatPlayer/${id}`,
            providesTags: result =>
                result
                    ? [{ type: 'CombatPlayer' as const, id: result.id }]
                    : ['CombatPlayer'],
        }),
        getCombatById: builder.query<CombatType, number>({
            query: id => `/Combat/${id}`,
            providesTags: result =>
                result
                    ? [{ type: 'Combat' as const, id: result.id }]
                    : ['Combat'],
        }),
        getCombatPlayerPositionsByCombatId: builder.query<CombatPlayerPositionType[], number>({
            query: combatId => `/CombatPlayerPosition/getByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CombatPlayerPosition' as const, id })), 'CombatPlayerPosition']
                    : ['CombatPlayerPosition'],
        }),
        getCombatAurasByCombatId: builder.query<CombatAuraType[], number>({
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
} = CombatParserApi;