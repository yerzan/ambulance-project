# CLAUDE.md — LAST CALL: Emergency Transfer

Long-lived rules for AI coding agents (Claude Code, Antigravity, etc.) working in this repository.
Read this file fully before every task. If a request conflicts with these rules, **stop and ask**.

## 1. Product rules (non-negotiable)

- **First-person only.** Never add third-person, orbit, cinematic or external vehicle cameras — including when driving the ambulance.
- **First release = 2–4 medic players cooperating against NPC patients** (1 medic allowed only for solo testing/debug). No player-controlled patient, no "Patient Online" code in v1 (only leave extension points if explicitly asked).
- Patient archetypes are fictional personalities, **never medical diagnoses**. No realistic restraint, medication or medical procedure instructions — all such actions are abstract, stylized mini-games (GDD §2.1).
- Physics should create funny situations but must never softlock a mission (always provide unstuck / reset / assisted carry).

## 2. Tech stack (see `Docs/Architecture/ADR-001-engine.md`)

- Unity **6000.6.10f1** (Unity 6.6), URP, C#.
- Netcode for GameObjects 2.x + Unity Multiplayer Services SDK (Sessions/Lobby/Relay).
- Input System (no legacy `UnityEngine.Input`), AI Navigation (NavMesh), Unity Test Framework.

## 3. Changes that REQUIRE explicit human approval

Do **not** do any of the following unless the task explicitly says so:

- Change the Unity editor version (`ProjectSettings/ProjectVersion.txt`).
- Add, remove or upgrade packages (`Packages/manifest.json`, `packages-lock.json`) or import Asset Store / third-party plugins.
- Edit anything in `ProjectSettings/` (incl. physics layers, tags, quality, player settings).
- Change input bindings (`*.inputactions`).
- Delete or rename scenes, prefabs or ScriptableObject assets outside the task scope.
- Force-push, rewrite history, or push directly to `main`.

## 4. Networking rules (GDD §13.4, §14.2)

- **Host-authoritative.** The host decides all critical state: mission result, patient state, stretcher/ambulance physics, item ownership.
- Clients send **intent/input** ("grab stretcher handle #2"), never final positions, physics results or scores.
- Networked physics runs only on the authoritative side (`NetworkRigidbody` + `NetworkTransform` per NGO physics docs).
- **Every RPC validates:** who is calling, whether they are allowed, whether the match/object state allows it, and whether it is a duplicate/spam request. Reject silently-but-logged otherwise.
- UI never calls RPCs directly: UI → gameplay service → validated network call.
- Never replicate a full ragdoll skeleton per frame; use animation mode + short ragdoll state with a few synced parameters.
- Gameplay must not depend on local FPS: use `FixedUpdate`/network tick for physics and timers.

## 5. Architecture & code rules

- Layers (top → bottom, no upward dependencies): **Presentation** (UI/HUD/Camera/Audio/VFX) → **Gameplay** (Mission/Patient/Stretcher/Ambulance/Scoring/Progression) → **Networking** (Session/Roles/RPC validation/Replication) → **Data** (ScriptableObjects/Save/Configs).
- Systems talk through interfaces and events, not by reaching into each other's MonoBehaviours.
- One class = one responsibility. **No god-object `GameManager`.** Keep MonoBehaviours small.
- Content and balance values live in **ScriptableObjects** or `[SerializeField]`, never hard-coded duplicates.
- Avoid empty/unnecessary `Update()`; cache components; no `Find*` calls at runtime hot paths.
- Namespaces: `LastCall.<Area>` (e.g. `LastCall.Stretcher`, `LastCall.Networking`).
- Style: see `.editorconfig` — PascalCase public members, `_camelCase` private fields, braces always.

### Folder layout (GDD §14)

```
Assets/_Game/
  Art/       Characters Vehicles Environments Props UI
  Audio/     Music SFX VO
  Prefabs/   Characters Patients Stretchers Ambulances Items
  Scenes/    Boot MainMenu Lobby TestYard Mission_01 Hospital
  Scripts/   Core Gameplay Patient Stretcher Vehicle Mission Networking UI Audio AI Progression Tools
  Data/      Patients Archetypes Missions Objectives Items Vehicles
  Settings/  Input URP
  Tests/     EditMode PlayMode
Docs/        GDD Architecture Networking TestPlans Tasks
```

Assemblies: `LastCall.Runtime` (Scripts/), `LastCall.Editor` (Scripts/Tools/Editor), `LastCall.Tests.EditMode`, `LastCall.Tests.PlayMode`. Put new code in the correct assembly; create a new `.asmdef` only with approval.

## 6. Workflow

1. **Plan first.** For any non-trivial task: inspect relevant files, list assumptions, risks and a short step plan. Do not edit until the plan is approved.
2. **One task = one subsystem.** Never rewrite netcode, stretcher physics, NPC and UI in the same change.
3. Implement only the approved scope (explicit folders/files).
4. Add/update EditMode or PlayMode tests where appropriate.
5. Run checks (below) and report **exact** results.
6. Report: changed files, known limitations, manual editor steps needed.
7. Commit on a `feature/LC-xxx-short-name` branch, message `LC-xxx: <imperative summary>`; open PR into `develop`.

### Branches

- `main` — stable, only via PR from `develop` (milestone/gate reached).
- `develop` — integration branch.
- `feature/LC-xxx-*`, `fix/LC-xxx-*`, `chore/*` — work branches from `develop`.

### Commands (PowerShell)

```powershell
$UNITY = "C:\Program Files\Unity\Hub\Editor\6000.6.10f1\Editor\Unity.exe"

# EditMode tests
& $UNITY -batchmode -projectPath . -runTests -testPlatform EditMode -testResults TestResults\editmode.xml -logFile Logs\tests-editmode.log

# PlayMode tests
& $UNITY -batchmode -projectPath . -runTests -testPlatform PlayMode -testResults TestResults\playmode.xml -logFile Logs\tests-playmode.log

# Windows build (output: Builds\Windows\LastCall.exe)
& $UNITY -batchmode -quit -projectPath . -executeMethod LastCall.Editor.BuildScript.BuildWindows -logFile Logs\build.log
```

Close the Unity Editor before running batchmode commands on the same project.

## 7. Definition of Done (GDD §20.4)

- [ ] Code compiles with no new errors and no task-related critical warnings.
- [ ] Main scenario + at least one edge case verified (test or documented manual check).
- [ ] Networked feature: a client cannot change authoritative state without host validation.
- [ ] New content/balance data is configurable without code edits.
- [ ] Scenes/prefabs checked in the editor, git diff reviewed (no stray ProjectSettings/package changes), known limitations written down.
- [ ] Issue acceptance criteria ticked; `Docs/Tasks/BACKLOG.md` status updated.

## 8. Task template

```
TASK: Implement [one small feature] (LC-xxx).
CONTEXT: Read CLAUDE.md, the GDD section(s) [..], and Docs/Architecture.
SCOPE: Only modify [explicit folders/files]. Do not refactor unrelated systems.
ACCEPTANCE CRITERIA:
1. [observable result]
2. [network/edge case]
3. [test or validation]
WORKFLOW: inspect → plan → (approval) → implement → tests → report changed files & limitations.
```

## 9. Where things live

- Long-lived rules: `CLAUDE.md` (this file).
- Architecture decisions: `Docs/Architecture/ADR-xxx-*.md`.
- Networking notes: `Docs/Networking/`. Test plans: `Docs/TestPlans/`.
- Progress / backlog: GitHub Issues (source of truth) mirrored in `Docs/Tasks/BACKLOG.md`.
- Design source: `Docs/GDD/LAST_CALL_GDD_UA_v1.1.docx` (+ `GDD_Changelog.md` for deviations).
