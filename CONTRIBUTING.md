# Contributing to MerlinOps

Thanks for making this community sample more reproducible and safe.

## Before you open a PR

1. Read [AGENTS.md](AGENTS.md), [the 78 decisions](docs/decisions.md), [security boundaries](docs/security.md) and the [roadmap](docs/roadmap.md).
2. Open an Issue for significant architectural or presenter-story changes; reference a reproducible scenario.
3. Keep the local demonstration cloud-free. **Never add Terraform apply, Azure deployment, secret keys, private traces or unauthenticated LIVE actions.**
4. Prefer smallest meaningful vertical slices with a clear **Definition of Done**.
5. Run the relevant validation steps and record the results in your PR.
6. Do not present synthetic cost/latency/quality/approval as real measurements.
7. Use English in code/comments and engineering docs; Spanish in Slidev copy and on-stage scripts.

## Local verification

~~~bash
dotnet test tests/MerlinOps.Tests/MerlinOps.Tests.csproj
python3 -m unittest discover -s tools/tests -v
python3 tools/check_docs.py
cd slides && npm install && npm test && npm run build
~~~

Run browser E2E when changing Slidev or visual assets. The public presenter story must remain usable offline in Replay mode.

## PR requirements

- [ ] Scope, linked Issue and risk assessment
- [ ] Relevant tests added or updated
- [ ] Simulated versus Live capabilities explicitly documented
- [ ] No private data / credentials or auto-deploy code
- [ ] Visual screenshots or evidence if UI changes
- [ ] Documentation / ADR and status updated where affected

The initial project bootstrap used authorized direct-to-main commits. Ongoing collaborators should use reviewed GitHub Flow PRs and green CI checks. See [Security policy](SECURITY.md).
