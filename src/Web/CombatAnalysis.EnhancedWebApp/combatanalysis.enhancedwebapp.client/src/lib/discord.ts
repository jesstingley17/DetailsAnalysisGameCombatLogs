/**
 * Optional Discord integration via Vite env (public values only).
 * - Invite URL: open your community server (no OAuth).
 * - OAuth URL: starts Discord's authorize flow; you still need a backend to exchange `code` for tokens and link accounts.
 */

const invite = () => import.meta.env.VITE_DISCORD_INVITE_URL?.trim() ?? '';
const clientId = () => import.meta.env.VITE_DISCORD_CLIENT_ID?.trim() ?? '';
const redirectUri = () => import.meta.env.VITE_DISCORD_REDIRECT_URI?.trim() ?? '';

export function isDiscordInviteConfigured(): boolean {
    return Boolean(invite());
}

export function getDiscordInviteUrl(): string {
    return invite();
}

/** True when both client id and redirect URI are set (OAuth authorize URL can be built). */
export function isDiscordOAuthConfigured(): boolean {
    return Boolean(clientId() && redirectUri());
}

/**
 * Discord OAuth2 authorization URL (user signs in with Discord).
 * Register the same redirect URI in the Discord Developer Portal.
 * Your server must handle the callback and exchange `code` — not included in this repo.
 */
export function buildDiscordOAuthAuthorizeUrl(): string | null {
    if (!isDiscordOAuthConfigured()) {
        return null;
    }
    const params = new URLSearchParams({
        client_id: clientId(),
        redirect_uri: redirectUri(),
        response_type: 'code',
        scope: 'identify email',
    });
    return `https://discord.com/api/oauth2/authorize?${params.toString()}`;
}
