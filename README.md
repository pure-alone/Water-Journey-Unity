# Dewy's Water Journey — Unity WebGL

Native Unity UGUI conversion of the supplied **Dewy's Water Journey** HTML prototype.

## Preserved experience

- Home + 6 interactive story scenes + Credits in one native Unity scene.
- Portrait reference resolution: **450 × 900**.
- Original story copy and student credit: **Zhihe Zhang**.
- Scene 1: drag the sun upward to complete **Evaporation**.
- Scene 2: drag all four droplets into the cloud for **Condensation**.
- Scene 3: tap the rain cloud four times for **Precipitation**.
- Scene 4: guide Dewy through the underground / groundwater route.
- Scene 5: guide Dewy through the mountain stream route.
- Scene 6: follow the river back to the ocean and reveal the ending card.
- Next remains locked until the current interaction is complete.
- Sound on/off persists through `PlayerPrefs`.

## Audio assets

All **18 supplied MP3 files** are committed directly under:

`Assets/Resources/Audio/`

They are used as project assets directly. The migration does **not** inspect, validate, decode, transcode, or re-encode their codec, encoding, or bitrate.

## Unity version

The project is pinned to **Unity 6000.3.15f1 (Unity 6.3 LTS)** in `ProjectSettings/ProjectVersion.txt`.

## Open locally

1. Clone this repository.
2. In Unity Hub choose **Add project from disk** and select the repository folder.
3. Open it with Unity **6000.3.15f1** and install Web Build Support if necessary.
4. Open `Assets/Scenes/Main.unity`.
5. Press Play.

The Canvas, page flow and interactive UGUI components are created by the Unity runtime scripts.

## Build WebGL

Use **Dewy > Build WebGL for itch.io** in the Unity Editor. The helper in `Assets/Editor/DewyBuild.cs` applies:

- Web player size: **450 × 900**
- Custom template: `PROJECT:Dewy`
- WebGL target
- Main scene: `Assets/Scenes/Main.unity`
- Output: `Builds/WebGL/`

## Unity Cloud / Build Automation

1. Connect the GitHub repository `pure-alone/Water-Journey-Unity` in Unity Cloud.
2. Create a Build Automation target for **WebGL**.
3. Use branch **main**.
4. Use Unity **6000.3.15f1 / Unity 6.3 LTS**.
5. Trigger the build and download the generated WebGL artifact.

## itch.io deployment

1. Zip the contents of the WebGL output folder so `index.html` is at the ZIP root.
2. Set the itch.io project type to **HTML**.
3. Upload the ZIP and enable browser play.
4. Use a **450 × 900** embed viewport, or allow fullscreen/mobile scaling.
5. Test Home, Scene 1–6, Credits, Restart, sound toggle, and all completion-gated Next buttons.

## Key files

- `Assets/Scenes/Main.unity` — Unity scene.
- `Assets/Scripts/DewyBootstrap.cs` — runtime bootstrap.
- `Assets/Scripts/DewyApp.cs` — page state, navigation, progress and completion gating.
- `Assets/Scripts/DewyPages.cs` — Home/Scene 1–6/Credits visuals and interactions.
- `Assets/Scripts/DewyUI.cs` — reusable UGUI primitives.
- `Assets/Scripts/DewyAudio.cs` — BGM/SFX mapping and persisted sound preference.
- `Assets/Scripts/DewyDragHandler.cs` — pointer drag callbacks.
- `Assets/Scripts/DewyContent.cs` — story and Credits copy.
- `Assets/WebGLTemplates/Dewy/` — WebGL shell.
- `Assets/Editor/DewyBuild.cs` — WebGL build settings.
- `Assets/Resources/Audio/` — 18 supplied MP3 files.

## Verification note

The repository structure and required files have been checked remotely. Actual Unity C# compilation and WebGL player generation require the Unity Editor or Unity Build Automation and are therefore the final build-time verification step.
