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
- Builder OS: Windows 11 24H2 or another WebGL-supported builder OS
- Project subfolder path: blank

## Advanced configuration

- Scenes: Assets/Scenes/Main.unity
- Development build: OFF
- Scripting define symbols: leave blank
- Pre-build script: blank
- Post-build script: blank
- Pre-export method: blank
- Post-export method: blank

The project already implements IPreprocessBuildWithReport in Assets/Editor/DewyBuild.cs.
For WebGL builds it automatically applies:
- 450 × 900 web player size
- PROJECT:Dewy WebGL template
- Gzip compression
- decompression fallback enabled
- Assets/Scenes/Main.unity as the enabled scene

## GitHub connection

Unity Build Automation needs read access to the repository. For a public GitHub repository, a classic PAT can use public_repo. If auto-build is enabled, also grant write:repo_hook.

## Output

Build Automation creates the WebGL build artifact. Download the artifact, then zip the contents so index.html is at the ZIP root before uploading to itch.io.
