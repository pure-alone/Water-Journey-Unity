# Water Journey Unity Migration Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Rebuild the supplied Dewy HTML storybook as a native Unity 6.3 LTS uGUI WebGL project preserving the 450×900 layout, eight page states, interactions, navigation, copy, and all 18 MP3 assets.

**Architecture:** One Unity scene is bootstrapped at runtime. `DewyApp` owns page navigation and scene state; focused helpers create uGUI elements, procedural sprites, drag handling, audio playback, and reusable controls. The HTML/CSS prototype remains the canonical mapping reference but is not embedded in the runtime.

**Tech Stack:** Unity 6000.3.15f1, C#, UnityEngine.UI/uGUI, EventSystem, WebGL, Python static contract tests.

**Spec:** `docs/superpowers/specs/2026-09-13-water-journey-unity-design.md`

## Global Constraints

- Reference resolution is exactly 450×900 portrait.
- Pages: Home, Scene 1–6, Credits.
- Scene interactions and Next-lock behavior must match the supplied `app.js`.
- All 18 MP3 files are copied verbatim into `Assets/Resources/Audio/`; no codec/encoding/bitrate validation or transcoding is performed.
- Background sound preference persists using `PlayerPrefs`.
- Target is Unity WebGL suitable for Unity Build Automation and itch.io.
- Main branch is explicitly authorized by the user.

---

### Task 1: Project contract tests and Unity skeleton

**Files:**
- Create: `tests/test_unity_project.py`
- Create: `Packages/manifest.json`
- Create: `ProjectSettings/ProjectVersion.txt`
- Create: `ProjectSettings/EditorBuildSettings.asset`
- Create: `Assets/Scenes/Main.unity`

**Produces:** A Unity project that has a single build scene and declares uGUI.

- [ ] Write failing Python tests for required project files, reference resolution markers, eight page identifiers, and exactly 18 expected MP3 paths.
- [ ] Run `python -m unittest tests/test_unity_project.py -v` and confirm failure because Unity files are absent.
- [ ] Add minimal project/package/scene files.
- [ ] Re-run tests for the skeleton expectations.

### Task 2: Runtime UI foundation

**Files:**
- Create: `Assets/Scripts/DewyBootstrap.cs`
- Create: `Assets/Scripts/UIFactory.cs`
- Create: `Assets/Scripts/PointerDrag.cs`
- Create: `Assets/Scripts/AudioController.cs`

**Produces:** Runtime Canvas/EventSystem creation, 450×900 CanvasScaler, reusable uGUI factories, drag events, and PlayerPrefs-backed audio.

- [ ] Extend contract tests to require exact runtime API/type markers and resource paths; verify red.
- [ ] Implement helpers without external plugins.
- [ ] Re-run tests and verify green.

### Task 3: Pages and story interactions

**Files:**
- Create: `Assets/Scripts/DewyApp.cs`

**Produces:** Home, Scene 1–6, Credits, navigation/progress, sound toggle, toasts, completion labels, and all prototype interactions.

- [ ] Extend tests for all page copy, interaction thresholds/counts, completion labels, navigation targets, and audio keys; verify red.
- [ ] Implement page builders and scene-specific interaction handlers.
- [ ] Re-run all tests and verify green.

### Task 4: Original audio assets and WebGL presentation

**Files:**
- Copy: `audio/*.mp3` → `Assets/Resources/Audio/*.mp3`
- Create: `Assets/WebGLTemplates/Dewy/index.html`
- Create: `README.md`

**Produces:** Original audio files committed unchanged and a WebGL template/documentation for 450×900 itch.io deployment.

- [ ] Extend tests to compare source/destination MP3 bytes with SHA-256 as a copy-integrity check only (not codec validation); verify red before copying.
- [ ] Copy all 18 files byte-for-byte without inspecting or transcoding audio content.
- [ ] Add responsive 1:2 WebGL shell/template and build/deployment instructions.
- [ ] Run all tests.

### Task 5: Verification and GitHub commit

**Files:** all project files above.

- [ ] Run `python -m unittest discover -s tests -v`.
- [ ] Confirm 18 MP3 files exist and source/destination byte hashes match.
- [ ] Inspect generated tree for excluded temporary/source prototype files.
- [ ] Commit the complete project to `main` in `pure-alone/Water-Journey-Unity`.
- [ ] Fetch committed key files from GitHub and verify the final commit contains the project skeleton, scripts, and audio paths.
