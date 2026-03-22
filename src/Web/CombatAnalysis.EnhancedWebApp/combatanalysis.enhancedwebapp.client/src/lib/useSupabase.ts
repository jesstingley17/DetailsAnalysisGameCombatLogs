import { useMemo } from 'react';

import { getSupabaseClient } from '@/lib/supabase';
import type { SupabaseClient } from '@supabase/supabase-js';

/**
 * Returns the singleton Supabase client, or `null` if `VITE_SUPABASE_*` env vars are not set.
 */
export function useSupabase(): SupabaseClient | null {
    return useMemo(() => getSupabaseClient(), []);
}
