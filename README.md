# Details Analysis Game Combat Logs

**The clearest way to turn a WoW combat log into performance you can act on** — upload a log, explore damage/healing/timelines, see player contribution, and drill into specialization scoring. A **social layer** (feed, chat, communities) is there when you want to coordinate around those insights, but the spine of the product is **raid analytics**.

Upstream project: [Solinx97/DetailsAnalysisGameCombatLogs](https://github.com/Solinx97/DetailsAnalysisGameCombatLogs) · [Releases](https://github.com/Solinx97/DetailsAnalysisGameCombatLogs/releases)

---

## Why this exists

- **Performance-first**: Parsing, dashboards, timelines, deaths, bursts, and **specialization scoring** (e.g. damage/heal scores vs expectations) — see the Combat Parser API and web combat-log UI.
- **Optional social**: Posts, communities, chat, notifications — useful when tied to guild/team workflows; not required to get value from the analyzer.
- **Web for reach, desktop for power users**: The **Enhanced Web App** (React/Vite + ASP.NET host) is the natural default entry; the **WPF desktop** app remains for users who want a native client.

---

## How this differs from “logs in a browser”

| Angle | Here |
|--------|------|
| Focus | Deep **encounter breakdown** (players, targets, timelines, contribution) plus **scoring** hooks in the parser pipeline — not only aggregate rankings. |
| Ecosystem | **Your stack**: Identity Server, multiple APIs, SQL databases, SignalR — built for a full product, not a single static viewer. |
| Social | **Integrated** only where it supports communication; the README and UX can keep the **analyzer loop** as the hero path. |

*Warcraft Logs and similar tools are excellent at global benchmarks; this project targets a self-hosted / full-stack workflow with rich parsing and extensibility.*

---

## Repository map (the “ambitious spine”)

| Area | Location |
|------|----------|
| Solution | `CombatAnalysis.sln` |
| Combat parsing & scores | `src/API/CombatAnalysis.CombatParserAPI/`, `src/Parser/`, `src/Common/CombatAnalysis.*` |
| Identity (Duende) | `src/Identity/CombatAnalysisIdentity/` |
| Web UI (Vite + React) | `src/Web/CombatAnalysis.EnhancedWebApp/combatanalysis.enhancedwebapp.client/` |
| Web BFF / server | `src/Web/CombatAnalysis.EnhancedWebApp/CombatAnalysis.EnhancedWebApp.Server/` |
| Chat, user, comms, hubs, notifications | `src/API/CombatAnalysis.*` |
| Desktop | `src/DesktopApp/` |
| SQL projects | `databases/` |
| Tests | `tests/` |

---

## Features (combat first)

- Upload and parse **combat logs**; readable fights, **damage / healing / taken / resources**, deaths, actions, **timelines**, dashboards, **bursts**.
- **Player contribution** views and **specialization score** data (parser + API + UI surfaces).
- **Details** for your logs and (where enabled) others’ logs.
- **Social (optional)**: profile, posts, chat, communities, discovery.
- **Identity Server** for sign-in and protected APIs.

---

## Fastest path for developers (today)

There is **no** single-command Docker bootstrap in this repo yet; local setup is still **.NET + SQL Server + multiple processes**. The shortest honest path:

1. **Prerequisites**: [.NET SDK](https://dotnet.microsoft.com/download) (match solution version), **SQL Server** (or LocalDB for light dev), **Node.js** for the web client.
2. **Clone** the repository.
3. **Database**: Deploy or publish the `databases/*.sqlproj` (or your own scripts) and set **connection strings** via `appsettings.Development.json`, **environment variables**, or **user secrets** (each API/Identity project has a `UserSecretsId`). There is no committed production connection string.
4. **Configure URLs**: Point Identity, APIs, and the **Enhanced Web App** server at each other (see `appsettings.Development.json` under each host). The desktop app uses `src/DesktopApp/CombatAnalysis.App/appsettings.Development.json` for API URLs (not `App.config` in this tree).
5. **Build**: `dotnet build CombatAnalysis.sln`
6. **Run** (typical dev order): Identity → APIs you need (Combat Parser, User, Chat, …) → Hubs if using chat → **Enhanced Web App Server** (serves API proxy + static SPA in dev) → optional **Desktop** app.
7. **Web client** (from `combatanalysis.enhancedwebapp.client`): `npm ci && npm run dev` (or rely on the .NET host’s dev workflow).

**Nice-to-haves (not yet in-repo)** — good contributions: `docker-compose` for SQL + services, **seed data**, a **sample `.txt` combat log**, and a **GIF** of upload → score → timeline in the README.

---

## Tests

- .NET: projects under `tests/` (e.g. integration/smoke tests; configure connection strings for your environment).
- Frontend: `npm run lint` in `combatanalysis.enhancedwebapp.client` (see `package.json` for available scripts).

---

## Trust & data

- **Identity** uses Duende IdentityServer patterns; treat **secrets** (DB passwords, signing certificates, SMTP) as production secrets — use user secrets or a host vault, never commit them.
- **Uploaded logs** contain character names and combat data; for any hosted deployment, document **retention**, **who can access logs**, and **deletion** in your own privacy policy.

---

## Product direction (concise)

The **killer loop** to emphasize in UX and docs:

**Upload log → see score & breakdown → understand what drove the result → compare to spec/encounter context → share or discuss with the team.**

“So what?” insights (cooldown timing, movement, phase downtime) are the next leap — much of the **raw signal** exists in parsing; product work is **interpretation & UX** on top.

**Primary audience (suggested wedge)**: serious raiders and **guild leads** who need team-level clarity, not only a global leaderboard.

---

## Contributing

Issues and PRs welcome. Keep changes focused; match existing patterns. Upstream: [Solinx97](https://github.com/Solinx97).

---

## License

See [LICENSE](LICENSE).
