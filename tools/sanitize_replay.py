#!/usr/bin/env python3
"""Prepare a MINIMIZED Replay candidate for human review. Never auto-publish.

This is a strict whitelist and common-identifier scrubber, NOT a guarantee of
de-identification. Unknown metadata is discarded, suspicious strings fail closed.
Raw traces and credentials must NEVER be passed to a public GitHub Action.
"""
from __future__ import annotations

import argparse
import hashlib
import json
import re
from pathlib import Path

SCHEMA = "merlinops.replay.v1"
RUN_FIELDS = {"id", "name", "status", "events"}
EVENT_FIELDS = {"sequence", "kind", "agent", "message", "gate", "metric", "traceId"}
PATTERNS = (
    (re.compile(r"(?i)(?:bearer\s+)[A-Za-z0-9._~-]{10,}"), "[REDACTED_AUTH]"),
    (re.compile(r"(?i)(?:sk-[A-Za-z0-9_-]{16,}|gh[psu]_[A-Za-z0-9]{20,}|eyJ[A-Za-z0-9._-]{20,})"), "[REDACTED_TOKEN]"),
    (re.compile(r"(?i)\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}\b"), "[REDACTED_EMAIL]"),
    (re.compile(r"\b(?:\d{1,3}\.){3}\d{1,3}\b"), "[REDACTED_IP]"),
)
CRITICAL = re.compile(
    r"(?is)-----BEGIN (?:RSA |EC |OPENSSH )?PRIVATE KEY-----|"
    r"(?:password|client_secret|connectionstring|access_token)\s*[:=]\s*\S+"
)
SAFE_NAME = re.compile(r"^[a-zA-Z0-9_.-]{1,64}$")
SAFE_KIND = re.compile(r"^[a-z][a-z0-9_.-]{1,64}$")
ALLOWED_GATES = {"NONE", "PASS", "DENY", "PENDING", "SIMULATED"}
ALLOWED_STATES = {"COMPLETED", "BLOCKED", "ROLLED_BACK", "FAILED"}


class SanitizationError(ValueError):
    """Candidate is unsafe; refuse to release."""


def _safe_text(text: object, *, max_length: int = 500) -> str:
    if not isinstance(text, str) or len(text) > 10000:
        raise SanitizationError("Non-text or oversized message")
    if CRITICAL.search(text):
        raise SanitizationError("Possible secret material; manual handling required")
    result = text
    for pattern, replacement in PATTERNS:
        result = pattern.sub(replacement, result)
    if len(result) > max_length:
        raise SanitizationError("Message too long; manually minimize")
    return result


def sanitized_replay(payload: object) -> dict:
    if not isinstance(payload, dict) or payload.get("schema") != SCHEMA:
        raise SanitizationError("Invalid Replay schema")
    scenes = payload.get("scenarios")
    if not isinstance(scenes, list) or not 1 <= len(scenes) <= 25:
        raise SanitizationError("Invalid scenario count")

    output_scenes = []
    for scene in scenes:
        if not isinstance(scene, dict) or not RUN_FIELDS.issubset(scene.keys()):
            raise SanitizationError("Missing scenario fields")
        scene_id = scene["id"]
        if not isinstance(scene_id, str) or not SAFE_NAME.fullmatch(scene_id):
            raise SanitizationError("Unsafe scenario identifier")
        if scene["status"] not in ALLOWED_STATES:
            raise SanitizationError("Unknown scenario state")
        events = scene["events"]
        if not isinstance(events, list) or not 1 <= len(events) <= 1000:
            raise SanitizationError("Invalid event count")

        output_events = []
        for index, event in enumerate(events, 1):
            if not isinstance(event, dict) or not EVENT_FIELDS.difference({"metric"}).issubset(event):
                raise SanitizationError("Missing event fields")
            if type(event["sequence"]) is not int or event["sequence"] != index:
                raise SanitizationError("Events not ordered consecutively")
            if event["gate"] not in ALLOWED_GATES:
                raise SanitizationError("Unknown gate")
            if not isinstance(event["kind"], str) or not SAFE_KIND.fullmatch(event["kind"]):
                raise SanitizationError("Unsafe event kind")
            if not isinstance(event["agent"], str) or not SAFE_NAME.fullmatch(event["agent"]):
                raise SanitizationError("Unsafe agent name")
            result = {
                "sequence": index,
                "kind": event["kind"],
                "agent": event["agent"],
                "message": _safe_text(event["message"]),
                "gate": event["gate"],
                "traceId": "public-" + hashlib.sha256(
                    (scene_id + ":" + str(event["traceId"])).encode()
                ).hexdigest()[:16],
            }
            if "metric" in event:
                metric = event["metric"]
                if isinstance(metric, bool) or not isinstance(metric, (int, float)):
                    raise SanitizationError("Metric must be numeric")
                if abs(metric) > 1e12:
                    raise SanitizationError("Metric outside safe range")
                result["metric"] = metric
            output_events.append(result)
        output_scenes.append({
            "id": scene_id,
            "name": _safe_text(scene["name"], max_length=100),
            "status": scene["status"],
            "events": output_events,
        })

    return {
        "schema": SCHEMA,
        "provenance": "REVIEW_REQUIRED",
        "disclosure": (
            "Sanitized candidate only. Not approved for public publication. "
            "Manual human review and provenance confirmation required."
        ),
        "scenarios": output_scenes,
    }


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("input", type=Path, help="Local trusted input path (never GitHub Actions secrets)")
    parser.add_argument(
        "--output",
        type=Path,
        default=Path("replay-review/candidate.json"),
        help="Unpublished candidate for HUMAN review; do not commit",
    )
    args = parser.parse_args()
    payload = json.loads(args.input.read_text(encoding="utf-8"))
    candidate = sanitized_replay(payload)
    args.output.parent.mkdir(parents=True, exist_ok=True)
    args.output.write_text(json.dumps(candidate, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print("Candidate written for HUMAN REVIEW. Do NOT automatically publish it.")


if __name__ == "__main__":
    main()
