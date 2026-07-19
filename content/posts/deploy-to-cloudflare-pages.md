+++
title = "Deploying a Zest Site to Cloudflare Pages"
date = 2026-07-19
tags = ["deployment", "cloudflare", "guide"]
description = "Step-by-step guide to deploy a Zest static site on Cloudflare Pages, with detailed notes on installing .NET SDK in the build environment."
layout = "single"
+++

[Cloudflare Pages](https://pages.cloudflare.com/) is a great fit for Zest sites — it's free, fast, and supports custom build commands. This guide walks you through setting up continuous deployment from a Git repository.

---

## Prerequisites

- A Zest site in a Git repository (GitHub, GitLab, or Bitbucket)
- A Cloudflare account

---

## Important: .NET SDK is not pre-installed

Cloudflare Pages build images are minimal Linux environments. They do **not** come with the .NET SDK pre-installed, which means you cannot run `dotnet tool install --global zest` directly.

**Every build must first install the .NET SDK** before it can run Zest. This is done via the official [dotnet-install.sh](https://dot.net/v1/dotnet-install.sh) script at the start of the build process.

Both approaches below (build script and one-liner) handle this automatically.

---

## Step 1: Choose your build method

### Option A: Using a build script (recommended)

Create a `build.sh` at the root of your repository:

```bash
#!/usr/bin/env bash
# dotnet-install.sh downloads the .NET SDK at build time because
# Cloudflare Pages images do not include it.
curl -sSL https://dot.net/v1/dotnet-install.sh > dn.sh
chmod +x dn.sh
./dn.sh --channel 10.0 --install-dir $HOME/.dotnet
export PATH="$HOME/.dotnet:$HOME/.dotnet/tools:$PATH"
export DOTNET_ROOT="$HOME/.dotnet"
export DOTNET_ROOT_X64="$HOME/.dotnet"
hash -r
dotnet tool install --global zest
zest build
```

Commit and push `build.sh` to your repository, then set the build command in Cloudflare to:

```
bash build.sh
```

### Option B: One-liner build command

If you prefer not to have a separate script file, paste the entire process as a single command directly into the Cloudflare Dashboard build field:

```
curl -sSL https://dot.net/v1/dotnet-install.sh > dn.sh && chmod +x dn.sh && ./dn.sh --channel 10.0 --install-dir $HOME/.dotnet && export PATH="$HOME/.dotnet:$HOME/.dotnet/tools:$PATH" && export DOTNET_ROOT="$HOME/.dotnet" && export DOTNET_ROOT_X64="$HOME/.dotnet" && hash -r && dotnet tool install --global zest && zest build
```

---

## Step 2: Connect your repository

1. Log in to the [Cloudflare Dashboard](https://dash.cloudflare.com/).
2. Go to **Workers & Pages** → **Pages**.
3. Click **Create a project** → **Connect to Git**.
4. Select your Zest site repository and click **Begin setup**.

---

## Step 3: Configure build settings

Under **Build settings**, enter:

| Setting | Value |
|---|---|
| **Build command** | `bash build.sh` (Option A) or paste the one-liner (Option B) |
| **Build output directory** | `_site` |
| **Root directory (optional)** | leave blank |

### Understanding the build process

When Cloudflare Pages triggers a build, here is what happens step by step:

1. Cloudflare spins up a **fresh ephemeral Linux container** with Node.js, Python, and other common runtimes — but **no .NET SDK**
2. The `dotnet-install.sh` script downloads the .NET 10.0 SDK and places it in `$HOME/.dotnet`
3. Environment variables (`PATH`, `DOTNET_ROOT`, `DOTNET_ROOT_X64`) are set so the .NET CLI is discoverable
4. `dotnet tool install --global zest` installs the Zest global tool
5. `zest build` runs the Zest build, which processes your content and outputs the static site to the `_site/` directory
6. Cloudflare Pages publishes `_site/` to its global edge network

The entire .NET installation adds roughly 30–60 seconds to each build. This overhead is per-build since Cloudflare does not cache the SDK across builds.

---

## Step 4: Deploy

Click **Save and Deploy**. Cloudflare Pages will execute your build command, and within a minute or two your site will be live at your `<project>.pages.dev` domain.

---

## Automatic redeploys

Every time you push to your repository, Cloudflare Pages automatically rebuilds and redeploys your site. You can also configure:

- **Custom domain** — add your own domain under the Pages project settings
- **Branch deploys** — preview changes from feature branches before merging
- **Environment variables** — useful for configuring different outputs per environment

---

## Summary

Deploying a Zest site to Cloudflare Pages is straightforward once you account for the missing .NET SDK. With a `build.sh` script (or one-liner) that installs .NET at the start of every build, your site compiles to plain HTML and is served globally on Cloudflare's edge network — no server, no database, no maintenance.

For the repository template, you can also include a `.pages.toml` or configure everything through the dashboard. The key takeaway: **always install .NET first**, and point the output directory to `_site`.
