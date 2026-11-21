# Publish Slidev on GitHub Pages (owner action required)

The **Slidev build and Playwright browser tests are green**, but GitHub Pages was not enabled for this repository. The GitHub Actions token does **not** have permission to create the Pages site (API: `Resource not accessible by integration`). No public Pages URL has been verified.

## One-time setup in GitHub

1. Open [Repository Settings → Pages](https://github.com/davidop/netcoreconf-barcelona-2026/settings/pages) while signed in as an administrator.
2. Under **Build and deployment → Source**, select **GitHub Actions** and save.
3. Go to [Actions → Publish static Slidev to GitHub Pages](https://github.com/davidop/netcoreconf-barcelona-2026/actions/workflows/pages.yml), choose **Run workflow** on `main`.
4. After a successful deploy, verify the expected site address: `https://davidop.github.io/netcoreconf-barcelona-2026/` (it is NOT confirmed live as of this note).

The published site is a **public visualization-only Slidev deck** backed by synthetic Replay JSON. It must not expose private traces, model API keys, identity tokens or privileged REST commands. LIVE Azure is not enabled.

## Export

CI validates the static Slidev build. The editable PowerPoint and PDF visual companion delivered through the ChatGPT conversation are **not identical interactive exports** of this Slidev app. Slides requiring Vue/Three.js interactivity need the web version.
