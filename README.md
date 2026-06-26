# Messenger

A desktop messenger for Windows: real-time chats, group conversations, friends, avatars, in-app video playback and a switchable interface language (English / Russian).

The Windows client connects to a hosted backend (ASP.NET + PostgreSQL), so you only need to install the client to start chatting.

## Download

Grab the latest build from the [Releases](https://github.com/dimas4ek/Messenger/releases) page:

- **`Messenger-Setup.exe`** — installer (recommended). Installs to `Program Files\Messenger` by default; you can pick a custom folder during setup.
- **`Messenger-<version>-win-x64.zip`** — portable build. Unzip anywhere and run `Messenger.exe`, no installation required.

Requirements: **Windows 10/11 (x64)**. No .NET installation needed — the build is self-contained.

## First launch

The backend is hosted on a free tier that sleeps when idle, so the **first connection after a period of inactivity can take ~30–60 seconds** while the server wakes up. The client waits for this automatically (`ServerAvailability.WaitUntilAvailable()` in `Client/Program.cs`) — just give it a moment on startup.

## Features

- Private and group chats with real-time delivery (SignalR)
- Friends list, friend requests (accept / decline)
- User avatars and chat images
- In-app video playback (LibVLC)
- Message editing and deletion
- Profile settings (change username / password)
- Interface language switching: English and Russian

## Tech stack

- **Client:** Windows Forms (.NET 10), [Guna.UI2](https://gunaui.com/) for the UI, SignalR client for real-time updates, LibVLCSharp for video.
- **Server:** ASP.NET Core 10 Web API + SignalR hubs, hosted on Render (Docker).
- **Database:** PostgreSQL (Npgsql + EF Core).

## Build from source

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet publish Client/Client.csproj -c Release -r win-x64 --self-contained true \
  -p:PublishSingleFile=false -p:PublishTrimmed=false -o publish/client
```

The self-contained client is produced in `publish/client/` (`Messenger.exe`).

> Note: single-file publishing is intentionally **not** used — LibVLC loads native plugins from a directory on disk, so the client ships as a folder rather than a single executable.

### Running against a local server

By default the client targets the hosted server (`Api:BaseUrl` in `Client/appsettings.json`). To point it at a local backend (`Api:DevBaseUrl`, `http://localhost:5200`), set the environment variable `DOTNET_ENVIRONMENT=Development` (or `ASPNETCORE_ENVIRONMENT=Development`) before launching.

## Releases

Releases are built automatically by GitHub Actions when a `v*` tag is pushed (see `.github/workflows/release.yml`): the workflow publishes the self-contained client, builds the installer with Inno Setup, and attaches both artifacts to a new GitHub Release.
