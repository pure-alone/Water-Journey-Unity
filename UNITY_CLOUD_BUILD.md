# Unity Build Automation configuration

Target project: Dewy's Water Journey
Recommended editor: Unity 2022.3.62f2 LTS
Target platform: WebGL
Source repository: pure-alone/Water-Journey-Unity
Branch: unity-2022.3.62f2
Project subfolder path: leave blank (Assets and ProjectSettings are at repository root)

## Basic configuration

- Target name: dewy-water-journey-webgl-2022
- Platform: WebGL
- Branch: unity-2022.3.62f2
- Auto detect Unity version: ON
- Detected Unity version: 2022.3.62f2
- Builder OS: choose the default/supported WebGL builder offered by Unity
- Project subfolder path: blank
- Auto-build: optional; OFF is simpler for assessment submission

## Advanced configuration

- Scene List: leave blank to use EditorBuildSettings (recommended)
- If you explicitly enter a scene, use: Scenes/Main.unity
- Development build: OFF
- Scripting define symbols: leave blank
- Pre-build method/script: blank
- Post-build method/script: blank
- Environment variables: none required

The project already implements IPreprocessBuildWithReport in Assets/Editor/DewyBuild.cs.
For WebGL builds it automatically applies:
- 450 × 900 web player size
- PROJECT:Dewy WebGL template
- Gzip compression
- decompression fallback enabled
- Assets/Scenes/Main.unity as the enabled scene

## GitHub connection

Unity Build Automation needs read access to the repository.
For a public repository with a classic GitHub PAT, public_repo is sufficient for repository access.
If Auto-build is enabled, also grant write:repo_hook.
For a fine-grained PAT, grant Contents: Read-only; add Webhooks: Read and write only when Auto-build is used.

## Output

Run a build from Build Automation. After it succeeds, download the WebGL artifact.
For itch.io, zip the contents of the generated WebGL output so index.html is at the ZIP root, upload it as an HTML project, and enable browser play.
