import { createClient, type SupabaseClient } from '@supabase/supabase-js';

const url = import.meta.env.VITE_SUPABASE_URL;
const anonKey = import.meta.env.VITE_SUPABASE_ANON_KEY;

let client: SupabaseClient | null = null;

/** True when both URL and anon key are set (e.g. in `.env.local` or Vercel env). */
export const isSupabaseConfigured = Boolean(
    url?.trim() && anonKey?.trim()
);

/**
 * Shared Supabase browser client (anon key). Returns `null` if env vars are missing.
 * Configure `VITE_SUPABASE_URL` and `VITE_SUPABASE_ANON_KEY` — never commit the service role key here.
 */
export function getSupabaseClient(): SupabaseClient | null {
    if (!isSupabaseConfigured || !url || !anonKey) {
        return null;
    }
    if (!client) {
        client = createClient(url, anonKey, {
            auth: {
                persistSession: true,
                autoRefreshToken: true,
                detectSessionInUrl: true,
            },
        });
    }
    return client;
}
