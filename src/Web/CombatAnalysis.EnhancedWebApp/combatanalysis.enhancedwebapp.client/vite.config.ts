import { fileURLToPath, URL } from 'node:url';

import plugin from '@vitejs/plugin-react';
import child_process from 'child_process';
import fs from 'fs';
import path from 'path';
import { env } from 'process';
import { defineConfig } from 'vite';

const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

const certificateName = "combatanalysis.enhancedwebapp.client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
}

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    if (0 !== child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit', }).status) {
        throw new Error("Could not create certificate.");
    }
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
    [`^/api/${apiVersion}/UnreadGroupChatMessage`]: { target, secure: false },
    [`^/api/${apiVersion}/GroupChatUser`]: { target, secure: false },
    [`^/api/${apiVersion}/GroupChatRules`]: { target, secure: false },
    [`^/api/${apiVersion}/VoiceChat`]: { target, secure: false },
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
            ...userProxy,
            ...feedProxy,
            ...chatProxy,
        },
        port: parseInt(env.DEV_SERVER_PORT || '65471'),
        https: {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        }
    }
})
