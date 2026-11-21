# GitHub repository presentation checklist

[← README](../README.md) · [Media kit](media-kit.md) · [Repository Settings](https://github.com/davidop/netcoreconf-barcelona-2026/settings)

The repository source already includes the hero, diagrams, badges, CI, contributor templates and **an upload-ready PNG social preview**. The **About panel and Social preview selection** are repository settings, not README files, and the connected GitHub tools cannot change them. As checked on 2026-11-21, the repository's description and website are empty and its topics list is empty.

## Recommended About panel

**Description (copy into GitHub):**

> MerlinOps 🪄 · .NET 10 + Microsoft Agent Framework + Slidev Mission Control | FinOps, governance and synthetic Replay · NetCoreConf Barcelona 2026

**Website:** https://davidop.github.io/netcoreconf-barcelona-2026/

**Suggested repository topics:** dotnet, azure, microsoft-agent-framework, agentic-ai, multi-agent-systems, finops, slidev, observability, replay, github-actions, netcoreconf, copilot-studio.

## Social preview image

1. Open [Settings → General](https://github.com/davidop/netcoreconf-barcelona-2026/settings).
2. Scroll to **Social preview**.
3. Download the **already rendered, upload-ready [social-preview.png](../assets/banners/social-preview.png)** (1280 × 640). Its [original editable SVG](../assets/banners/social-preview.svg) is also included.
4. In **Social preview**, click **Edit → Upload an image**, select that PNG and save.
5. Inspect the share preview and make sure the subtitle remains legible on mobile.

The PNG is generated from original MerlinOps SVG artwork through [GitHub Actions](../.github/workflows/social-preview.yml), using [a deterministic conversion script](../tools/render_social_preview.py). Third-party marks are not included in the social preview. Uploading the image as the repository-wide Social preview remains a **manual GitHub Settings control**; the file existing in the repository does not apply it.

## Collaboration and quality gates

- [ ] Consider repository branch protection when the autonomous bootstrap is finished: require CI + browser E2E, reviewed PRs and disallow force pushes.
- [x] CODEOWNERS, contributing guide, issue forms and PR template live in the repository.
- [x] Dependabot proposes dependency updates as PRs; do not enable unattended auto-merge for Preview SDKs.
- [x] Public GitHub Pages hosts static Replay, not privileged LIVE operations.
- [ ] Add authentic screenshots from successful Playwright runs after they have been inspected and checked for safety.
- [ ] Attach official NetCoreConf organizer assets only with verified attribution/permission, never invent endorsements.
