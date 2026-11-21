# Visual system and responsible brand use

[← Back to README](../README.md) · [Media kit](media-kit.md)

**Design intent:** modern Fluent-adjacent interface language, high contrast, Microsoft ecosystem familiarity and a distinctive "technological magic" personality. **This is not an official Microsoft design system** and should not be described as Microsoft-authored.

## Palette

| Token | Hex | Usage |
|---|---|---|
| Night | #07162F | Main backgrounds |
| Azure navy | #102D4C | Technical cards |
| Cyber cyan | #5CEEFE | Telemetry and positive traces |
| Agent violet | #8B76EF | MerlinOps and advanced orchestration |
| Success teal | #67E1CD | Quality Gate PASS |
| Alert coral | #F493AA | Policy DENY / rollback alert |
| Ice white | #F2F8FF | Main text |

**Typography:** Segoe UI → Arial → sans-serif. Large 16:9 headlines; readable projector-size labels; avoid relying on color alone to convey failure/success.

## Components

- **Hero:** 1600×620 original SVG; never stretch vertically.
- **Social:** 1280×640 original SVG; exports should preserve native aspect ratio.
- **Mascot:** 640×640. Use static fallback when motion is reduced or WebGL is unavailable.
- **Status:** always show a text label for SIMULATED, REPLAY, or LIVE. Purple/dashed diagrams denote **future/not connected**, not shipping capabilities.
- **Diagrams:** use arrows only for actual implemented flows; dashed arrows for proposed future integrations.

## Logos and legal

The Microsoft ecosystem mark is used only to identify the context. The logos sourced from [Devicon](../assets/brand/README.md) are subject to the owners' trademarks. Avoid implying that the repository is Microsoft-certified, officially supported, or co-branded with the event organizer. Do not invent a Copilot Studio or Foundry product logo; use product names until verified owner-provided assets are available.

## Accessibility

Prefer native SVG text with descriptive alt text. Avoid SVG animation in README assets. Respect prefers-reduced-motion in Slidev; ensure keyboard controls remain functional and decorative graphics do not replace essential textual descriptions.
