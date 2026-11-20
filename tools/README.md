# Replay minimization: HUMAN REVIEW REQUIRED

The \`tools/sanitize_replay.py\` utility creates a **local candidate only**. It whitelists event fields, hashes trace IDs and removes some common identifiers while refusing suspected secrets or invalid events.

~~~bash
python3 tools/sanitize_replay.py slides/data/replay.json --output replay-review/candidate.json
python3 -m unittest discover -s tools/tests -v
~~~

**Never send raw private traces to public CI.** Detection is not complete de-identification. A human must independently inspect candidates, verify the legal basis, provenance and privacy, and explicitly approve any publication. This utility intentionally provides no direct GitHub upload/push function and stamps \`REVIEW_REQUIRED\`.
