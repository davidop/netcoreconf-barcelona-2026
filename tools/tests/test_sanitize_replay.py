import importlib.util
from pathlib import Path
import unittest

module_path = Path(__file__).resolve().parents[1] / "sanitize_replay.py"
spec = importlib.util.spec_from_file_location("sanitize_replay", module_path)
assert spec and spec.loader
module = importlib.util.module_from_spec(spec)
spec.loader.exec_module(module)


def fixture(message):
    return {
        "schema": "merlinops.replay.v1",
        "scenarios": [
            {
                "id": "finops-success",
                "name": "Synthetic mission",
                "status": "COMPLETED",
                "events": [
                    {
                        "sequence": 1,
                        "kind": "gate.evaluated",
                        "agent": "GovernanceGuardian",
                        "message": message,
                        "gate": "PASS",
                        "traceId": "potential-private-id",
                    }
                ],
            }
        ],
    }


class SanitizerTests(unittest.TestCase):
    def test_redacts_email_ip_and_rekeys_trace(self):
        safe = module.sanitized_replay(fixture("hello alice@example.org from 192.168.1.2"))
        evt = safe["scenarios"][0]["events"][0]
        self.assertNotIn("alice@example.org", evt["message"])
        self.assertNotIn("192.168.1.2", evt["message"])
        self.assertNotIn("potential-private-id", evt["traceId"])
        self.assertEqual(safe["provenance"], "REVIEW_REQUIRED")

    def test_secret_in_message_fails_closed(self):
        with self.assertRaises(module.SanitizationError):
            module.sanitized_replay(fixture("client_secret=do-not-share"))

    def test_bad_sequence_fails_closed(self):
        data = fixture("ok")
        data["scenarios"][0]["events"][0]["sequence"] = 2
        with self.assertRaises(module.SanitizationError):
            module.sanitized_replay(data)

    def test_unknown_fields_never_persist(self):
        data = fixture("safe")
        data["scenarios"][0]["events"][0]["rawPrompt"] = "private prompt"
        safe = module.sanitized_replay(data)
        self.assertNotIn("rawPrompt", str(safe))
        self.assertNotIn("private prompt", str(safe))

    def test_no_implicit_publication(self):
        safe = module.sanitized_replay(fixture("safe"))
        self.assertEqual(safe["provenance"], "REVIEW_REQUIRED")
        self.assertIn("Manual human review", safe["disclosure"])


if __name__ == "__main__":
    unittest.main()
