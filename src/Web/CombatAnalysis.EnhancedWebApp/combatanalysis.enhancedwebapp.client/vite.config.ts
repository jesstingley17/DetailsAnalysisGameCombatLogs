import { fileURLToPath, URL } from 'node:url';

import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import { env } from 'process';
import { defineConfig } from 'vite';

const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
}

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7296';

const apiVersion = env.API_VERSION ? env.API_VERSION : 'v1';

const logsProxy = {
    [`^/api/${apiVersion}/Logs`]: { target, secure: false },
};

const gameLogsProxy = {
    [`^/api/${apiVersion}/CombatLog`]: { target, secure: false },
    [`^/api/${apiVersion}/Combat`]: { target, secure: false },
    [`^/api/${apiVersion}/CombatPlayer`]: { target, secure: false },
    [`^/api/${apiVersion}/CombatAura`]: { target, secure: false },
    [`^/api/${apiVersion}/DamageDone`]: { target, secure: false },
    [`^/api/${apiVersion}/DamageDoneGeneral`]: { target, secure: false },
    [`^/api/${apiVersion}/DamageTaken`]: { target, secure: false },
    [`^/api/${apiVersion}/DamageTakenGeneral`]: { target, secure: false },
    [`^/api/${apiVersion}/HealDone`]: { target, secure: false },
    [`^/api/${apiVersion}/HealDoneGeneral`]: { target, secure: false },
    [`^/api/${apiVersion}/ResourceRecovery`]: { target, secure: false },
    [`^/api/${apiVersion}/ResourceRecoveryGeneral`]: { target, secure: false },
    [`^/api/${apiVersion}/PlayerDeath`]: { target, secure: false },
};

const communityProxy = {
    [`^/api/${apiVersion}/Community`]: { target, secure: false },
    [`^/api/${apiVersion}/CommunityDiscussion`]: { target, secure: false },
    [`^/api/${apiVersion}/CommunityDiscussionComment`]: { target, secure: false },
    [`^/api/${apiVersion}/CommunityUser`]: { target, secure: false },
    [`^/api/${apiVersion}/InviteToCommunity`]: { target, secure: false },
};

const userProxy = {
    [`^/api/${apiVersion}/Account`]: { target, secure: false },
    [`^/api/${apiVersion}/Authentication`]: { target, secure: false },
    [`^/api/${apiVersion}/Customer`]: { target, secure: false },
    [`^/api/${apiVersion}/Friend`]: { target, secure: false },
    [`^/api/${apiVersion}/RequestToConnect`]: { target, secure: false },
    [`^/api/${apiVersion}/Identity`]: { target, secure: false },
};

const feedProxy = {
    [`^/api/${apiVersion}/UserPost`]: { target, secure: false },
    [`^/api/${apiVersion}/UserPostLike`]: { target, secure: false },
    [`^/api/${apiVersion}/UserPostDislike`]: { target, secure: false },
    [`^/api/${apiVersion}/UserPostComment`]: { target, secure: false },
    [`^/api/${apiVersion}/CommunityPost`]: { target, secure: false },
    [`^/api/${apiVersion}/CommunityPostLike`]: { target, secure: false },
    [`^/api/${apiVersion}/CommunityPostDislike`]: { target, secure: false },
    [`^/api/${apiVersion}/CommunityPostComment`]: { target, secure: false },
};

const chatProxy = {
    [`^/api/${apiVersion}/PersonalChat`]: { target, secure: false },
    [`^/api/${apiVersion}/PersonalChatMessage`]: { target, secure: false },
    [`^/api/${apiVersion}/GroupChat`]: { target, secure: false },
    [`^/api/${apiVersion}/GroupChatMessage`]: { target, secure: false },
    [`^/api/${apiVersion}/GroupChatUser`]: { target, secure: false },
    [`^/api/${apiVersion}/VoiceChat`]: { target, secure: false },
};

const notificationProxy = {
    [`^/api/${apiVersion}/Notification`]: { target, secure: false },
};

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        proxy: {
            ...logsProxy,
            ...gameLogsProxy,
            ...communityProxy,
            ...userProxy,
            ...feedProxy,
            ...chatProxy,
            ...notificationProxy,
        },
        port: parseInt(env.DEV_SERVER_PORT || '5173')
    }
})
