# Water Journey Unity Migration Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Rebuild the supplied Dewy HTML storybook as a native Unity 6.3 LTS uGUI WebGL project preserving the 450×900 layout, eight page states, interactions, navigation, copy, and all 18 MP3 assets.

**Architecture:** One Unity scene is bootstrapped at runtime. `DewyApp` owns page navigation and scene state; focused helpers create uGUI elements, procedural sprites, drag handling, audio playback, and reusable controls. The HTML/CSS prototype remains the canonical mapping reference but is not embedded in the runtime.

**Tech Stack:** Unity 6000.3.15f1, C#, UnityEngine.UI/uGUI, EventSystem, WebGL.

**Spec:** `docs/superpowers/specs/2026-09-13-water-journey-unity-design.md`

## Global Constraints

- Reference resolution is exactly 450×900 portrait.
- Pages: Home, Scene 1–6, Credits.
- Scene interactions and Next-lock behavior must match the supplied `app.js`.
- All 18 MP3 files are copied directly into `Assets/Resources/Audio/`; do not inspect, validate, decode, transcode, or re-encode their codec/encoding/bitrate.
- Background sound preference persists using `PlayerPrefs`.
- Target is Unity WebGL suitable for Unity Build Automation and itch.io.
- Main branch is explicitly authorized by the user.

---

### Task 1: Unity project skeleton

**Files:**
- Create: `Packages/manifest.json`
- Create: `ProjectSettings/ProjectVersion.txt`
- Create: `ProjectSettings/EditorBuildSettings.asset`
- Create: `Assets/Scenes/Main.unity`

**Produces:** A Unity project that has a single build scene and declares uGUI.

- [x] Add project/package/scene files for Unity 6000.3.15f1.
- [x] Register `Assets/Scenes/Main.unity` as the enabled build scene.

### Task 2: Runtime UI foundation

**Files:**
- Create: `Assets/Scripts/DewyBootstrap.cs`
- Create: runtime UI/audio/drag helpers under `Assets/Scripts/`

**Produces:** Runtime Canvas/EventSystem creation, 450×900 CanvasScaler, reusable uGUI factories, drag events, and PlayerPrefs-backed audio.

- [x] Implement helpers without external UI plugins.
- [x] Configure the runtime for the 450×900 reference layout.

### Task 3: Pages and story interactions

**Files:**
- Create: `Assets/Scripts/DewyApp.cs`
- Create: `Assets/Scripts/DewyPages.cs`
- Create: `Assets/Scripts/DewyContent.cs`

**Produces:** Home, Scene 1–6, Credits, navigation/progress, sound toggle, toasts, completion labels, and the prototype interactions.

- [x] Implement Home, Scene 1–6, and Credits.
- [x] Preserve completion-gated Next navigation and Restart/Home flow.
- [x] Preserve the scene BGM/SFX mapping and persisted sound preference.

### Task 4: Original audio assets and WebGL presentation

**Files:**
- Copy: the supplied 18 `*.mp3` files → `Assets/Resources/Audio/*.mp3`
- Create: `Assets/WebGLTemplates/Dewy/`
- Create: `Assets/Editor/DewyBuild.cs`
- Create: `README.md`

**Produces:** Directly copied audio files and a WebGL configuration for the 450×900 itch.io deployment.

- [x] Copy all 18 MP3 files directly without inspecting, validating, decoding, transcoding, or re-encoding audio content.
- [x] Add the 450×900 WebGL template.
- [x] Add editor/pre-build WebGL settings for Unity Build Automation.

### Task 5: GitHub delivery and verification

**Files:** all project files above.

- [x] Commit the complete Unity project to `main` in `pure-alone/Water-Journey-Unity`.
- [x] Confirm the expected 18 MP3 filenames are present in `Assets/Resources/Audio/` without opening or validating their audio encoding.
- [x] Confirm `ProjectSettings/ProjectVersion.txt`, the main scene, runtime scripts, WebGL template, and `DewyBuild.cs` exist in the target repository.
- [ ] Actual C# compilation and WebGL player generation must be confirmed by Unity Editor or Unity Build Automation; this execution environment does not contain the Unity Editor.
