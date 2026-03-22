/// <reference types="vite/client" />

interface ImportMetaEnv {
    readonly VITE_SUPABASE_URL?: string;
    readonly VITE_SUPABASE_ANON_KEY?: string;
    /** e.g. https://discord.gg/your-invite */
    readonly VITE_DISCORD_INVITE_URL?: string;
    readonly VITE_DISCORD_CLIENT_ID?: string;
    /** Must match Discord app OAuth2 redirect(s) exactly */
    readonly VITE_DISCORD_REDIRECT_URI?: string;
}
