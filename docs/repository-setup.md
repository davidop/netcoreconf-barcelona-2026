# GitHub repository presentation checklist

[← README](../README.md) · [Media kit](media-kit.md) · [Repository Settings](https://github.com/davidop/netcoreconf-barcelona-2026/settings)

The repository source is already upgraded with the hero, diagrams, badges, CI and contributor templates. The **About panel and social preview are GitHub repository settings**, not README files, so they still require an authenticated owner to configure manually.

## Recommended About panel

**Description (copy into GitHub):**

> MerlinOps 🪄 · .NET 10 + Microsoft Agent Framework + Slidev Mission Control | FinOps, governance and synthetic Replay · NetCoreConf Barcelona 2026

**Website:** https://davidop.github.io/netcoreconf-barcelona-2026/

**Suggested repository topics:** dotnet, azure, microsoft-agent-framework, agentic-ai, multi-agent-systems, finops, slidev, observability, replay, github-actions, netcoreconf, copilot-studio.

## Social preview image

1. Open [Settings → General](https://github.com/davidop/netcoreconf-barcelona-2026/settings).
2. Scroll to **Social preview**.
3. Export [original social-preview.svg](../assets/banners/social-preview.svg) at **1280 × 640** into a raster PNG/JPG, retaining aspect ratio.
4. Upload that raster. GitHub may not accept SVG directly as the social-preview image.
5. Inspect the share preview and make sure the subtitle remains legible on mobile.

The SVG is original MerlinOps artwork; third-party marks are not included in the social preview.

## Collaboration and quality gates

- [ ] Consider repository branch protection when the autonomous bootstrap is finished: require CI + browser E2E, reviewed PRs and disallow force pushes.
- [x] CODEOWNERS, contributing guide, issue forms and PR template live in the repository.
- [x] Dependabot proposes dependency updates as PRs; do not enable unattended auto-merge for Preview SDKs.
- [x] Public GitHub Pages hosts static Replay, not privileged LIVE operations.
- [ ] Add authentic screenshots from successful Playwright runs after they have been inspected and checked for safety.
- [ ] Attach official NetCoreConf organizer assets only with verified attribution/permission, never invent endorsements.
