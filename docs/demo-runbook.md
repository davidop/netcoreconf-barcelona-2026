# 45-minute conference rehearsal — Érika & David

## Safe local launch

Prerequisites: .NET 10, Node.js 22+, npm.

~~~bash
dotnet run --project src/MerlinOps.Api
# Other terminal, from repository root:
cd slides
npm install
npm run dev
~~~

The presentation includes its own versioned SYNTHETIC Replay fixtures. Navigate to the Mission Control slide (sixth slide), select a scenario and press Reproducir. If the local API is running, use SIMULATED SSE to stream the synthetic event sequence. The buttons do not access Azure, Foundry or Teams.

API smoke:

~~~bash
curl http://localhost:5080/health
curl http://localhost:5080/api/scenarios
curl -X POST http://localhost:5080/api/runs/maf-simulate -H "Content-Type: application/json" -d '{"scenario":"policy-block"}'
~~~

## Presenter narrative

| Scene | Érika | David | Result |
|---|---|---|---|
| Opening | Challenges security and accountability | Challenges slow manual processes | Present MerlinOps |
| Architecture | Explains identity, policies, approvals | Explains .NET orchestration, FinOps and event traces | Agreement on boundaries |
| 1. The optimization | Validates quality and explicit approval | Evaluates synthetic model routing and cost | Success (synthetic) |
| 2. Governance shield | Blocks critical-risk request | Observes DENY and safe alternatives | No deployment |
| 3. Unexpected regression | Requires safe recovery | Explains canary and rollback | Recovered (Replay synthetic) |
| Closing | Governance is not optional | Automation must be measurable | Joint takeaway |

Flexible theatrical transitions, with the total talk capped at 45 minutes. Alternate dynamically; do not enforce arbitrary speaker time slots. Audience does not execute operations.

## Readiness gates

- [ ] All .NET, Slidev, fixture, browser and security checks pass
- [ ] The procedural 3D wizard renders on the venue laptop; static fallback works
- [ ] Reduced-motion and keyboard controls verified
- [ ] Slides available offline, including fallback dataset
- [ ] Quality Gates, Replay source and provenance are readable on projector
- [ ] Network-disconnection rehearsal, restoration and restart tested
- [ ] 45-minute dress rehearsals with both speakers and screen-sharing
- [ ] Authentic LIVE integration separately verified and authorized, or use transparently labelled synthetic demo
- [ ] Dependencies frozen and known-good tag released
- [ ] No secrets or private real traces in GitHub

## Safety

The current approved scope is **GitHub only**. Do not deploy, create or modify Azure resources. All shown costs, approvals and rollbacks in this first version are synthetic; never describe them as observed production results.
