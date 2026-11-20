import test from 'node:test'
import assert from 'node:assert/strict'
import { readFileSync } from 'node:fs'
const replay=JSON.parse(readFileSync(new URL('../data/replay.json',import.meta.url),'utf8'))

test('Replay is always explicitly synthetic until authentic traces are approved',()=>{
  assert.equal(replay.provenance,'SYNTHETIC_FIXTURE')
  assert.equal(replay.scenarios.length,3)
  for(const scenario of replay.scenarios){
    assert.equal(scenario.events.length>4,true)
    assert.deepEqual(scenario.events.map(e=>e.sequence),Array.from({length:scenario.events.length},(_,i)=>i+1))
    assert.equal(new Set(scenario.events.map(e=>e.traceId)).size,1)
  }
})
test('Fail-closed demo cannot proceed to approval/deployment',()=>{
  const events=replay.scenarios.find(x=>x.id==='policy-block').events
  assert.equal(events.find(x=>x.kind==='gate.evaluated').gate,'DENY')
  assert.equal(events.some(x=>x.kind.startsWith('deployment.')||x.kind.startsWith('approval.')),false)
})
test('Rollback follows observed regression',()=>{
  const events=replay.scenarios.find(x=>x.id==='quality-rollback').events
  assert.ok(events.find(x=>x.kind==='rollback.simulated').sequence>events.find(x=>x.kind==='telemetry.regression').sequence)
})
test('Public fixtures contain no apparent secrets, email addresses or real access tokens',()=>{
  const text=JSON.stringify(replay)
  assert.equal(/[\w.-]+@[\w.-]+\.[A-Za-z]{2,}/.test(text),false)
  assert.equal(/(BEGIN PRIVATE KEY|eyJhbGci|sk-[A-Za-z0-9]{20,})/.test(text),false)
})
