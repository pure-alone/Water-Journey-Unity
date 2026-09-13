# Water Journey Unity Migration Design

## Goal
Convert the supplied Dewy's Water Journey HTML/CSS/JavaScript prototype into a native Unity WebGL project while preserving the prototype's 450x900 portrait layout, visual hierarchy, component positions, copy, navigation, interaction rules, sound behavior, and 18 supplied MP3 files.

## Source of truth
The uploaded `Dewy_HTML_Prototype_Assessment2_Zhihe_Zhang(5).zip` is the source of truth. It contains `index.html`, `scene1.html` through `scene6.html`, `credits.html`, `styles.css`, `app.js`, reference images, and 18 MP3 files under `audio/`.

## Architecture
Use one Unity scene named `Main.unity` and one root Canvas configured with `CanvasScaler` reference resolution 450x900. Home, Scene 1-6, and Credits are represented as page panels under the same Canvas and switched by a central story controller. This avoids scene-loading transitions and keeps state, audio, and navigation behavior consistent with the original JavaScript prototype.

The project is built with native Unity UGUI rather than a WebView or embedded HTML. Visual objects such as Dewy, clouds, sun, hills, soil, rocks, trees, stream, river, lake, waterfall, labels, progress dots, and buttons are recreated with Unity UI primitives and generated sprites where practical. The target platform is Unity WebGL for Unity Cloud Build and itch.io deployment.

## Project layout
- `Assets/Scenes/Main.unity` - single runtime scene.
- `Assets/Scripts/StoryController.cs` - page state, navigation, completion state, toast messaging, and common UI wiring.
- `Assets/Scripts/AudioController.cs` - background music and SFX routing, sound toggle, and `PlayerPrefs` persistence.
- `Assets/Scripts/DragHandler.cs` - reusable pointer drag behavior.
- `Assets/Scripts/Scene1Evaporation.cs` - sun drag threshold and completion.
- `Assets/Scripts/Scene2Condensation.cs` - four water-drop drag targets and cloud snapping.
- `Assets/Scripts/Scene3Precipitation.cs` - four cloud taps, rain feedback, and forest completion.
- `Assets/Scripts/Scene4Groundwater.cs` - Dewy drag and sequential waypoint detection.
- `Assets/Scripts/Scene5MountainStream.cs` - Dewy drag and four sequential stream waypoints.
- `Assets/Scripts/Scene6RiverReturn.cs` - Dewy drag and four sequential river waypoints plus ending card.
- `Assets/Editor/WaterJourneyProjectBuilder.cs` - editor-time project/scene construction so the committed project opens with a complete generated hierarchy.
- `Assets/Resources/Audio/` - all 18 MP3 files copied byte-for-byte from the supplied prototype.
- `ProjectSettings/` and `Packages/` - Unity project metadata suitable for Unity Cloud Build.

## Global UI rules
- Design reference size is exactly 450x900 portrait.
- Canvas uses `Scale With Screen Size` with reference resolution 450x900 so positions and proportions match the supplied mobile prototype.
- Each page contains the same structural regions as the HTML: top bar, story copy, stage, bottom navigation, and eight progress dots.
- The active progress dot maps to Home=1, Scene1=2, Scene2=3, Scene3=4, Scene4=5, Scene5=6, Scene6=7, Credits=8.
- Back and Next/Finish/Home controls occupy the same bottom-navigation roles as the source prototype.
- Next remains disabled until the current scene's required interaction is complete.
- Toast messages reproduce the JavaScript messages and disappear automatically.
- Credits retain `Created by: Zhihe Zhang`, Restart Journey, Back, and Home behavior.

## Page behavior
### Home
Show the ocean home scene, Dewy, title, tagline, Start Journey, Credits, sound toggle, and progress state. Start Journey moves to Scene 1 and plays the same start SFX behavior.

### Scene 1 - Sunny Ocean
Allow the sun to be dragged vertically. When it crosses the source threshold equivalent, complete the scene, play evaporation SFX, show `Evaporation unlocked`, and enable Next. If released too low, show `Try dragging the sun higher`.

### Scene 2 - Cloud Workshop
Provide four independently draggable water drops. A drop only counts when released inside the cloud target. Accepted drops snap into four cloud positions. After all four are accepted, play sparkle SFX, show `Condensation complete`, mark complete, and enable Next. Invalid drops show `Drop it inside the cloud`.

### Scene 3 - Rainy Forest
The rain cloud requires four taps. Each tap produces rain feedback and decrements the visible counter. The fourth tap restores/completes the forest state, plays the forest SFX, shows `Precipitation complete`, and enables Next.

### Scene 4 - Underground Adventure
Dewy is dragged through the soil stage and must touch three glowing waypoints in sequence. Status text follows the original labels: `Move down through the topsoil`, `Travel around the rocks`, `Reach the groundwater layer`, then `Groundwater reached!`. Each reached waypoint plays underground drip SFX. Reaching all three plays sparkle SFX, shows `Infiltration complete`, and enables Next.

### Scene 5 - The Mountain Stream
Dewy must touch four stream waypoints in sequence. Status text follows the original labels: `Leave the hillside spring`, `Follow the downhill stream`, `Collect the joining runoff`, `Reach the valley river`, then `Runoff collected into a larger stream!`. Each waypoint plays stream splash SFX and advances visible flow state. Completing all four plays sparkle SFX, shows `Runoff & Collection unlocked`, and enables Next.

### Scene 6 - River to the Ocean
Dewy must touch four waypoints in order through river bend, waterfall, lake, and ocean. Status text follows the original labels: `Follow the river bend`, `Pass the waterfall`, `Cross the lake`, `Return to the ocean`, then `Journey complete!`. Each waypoint plays splash SFX. The last waypoint plays journey-complete SFX, reveals the ending card, and enables Finish.

### Credits
Show the completion copy, Dewy, the final quote/message, `Created by: Zhihe Zhang`, Restart Journey, Back, Home, sound toggle, and the eighth progress dot.

## Audio
The following files are copied directly without transcoding, codec validation, bitrate checks, re-encoding, or ffmpeg processing:

Background music:
- `home_theme.mp3`
- `sunny_ocean.mp3`
- `cloud_workshop.mp3`
- `rainy_forest.mp3`
- `underground.mp3`
- `mountain_stream.mp3`
- `river_ocean.mp3`
- `credits_theme.mp3`

SFX:
- `ui_click.mp3`
- `water_drop.mp3`
- `sparkle.mp3`
- `evaporation.mp3`
- `bubble.mp3`
- `rain.mp3`
- `forest_complete.mp3`
- `underground_drip.mp3`
- `stream_splash.mp3`
- `journey_complete.mp3`

`AudioController` maps one background track per page and the same event categories used by `app.js`. Sound state is persisted with `PlayerPrefs` under a stable key and defaults to enabled.

## Build configuration
- Unity project targets WebGL.
- The committed scene list contains `Assets/Scenes/Main.unity` as the first scene.
- The project uses no external runtime packages beyond Unity's standard UGUI/TextMeshPro-compatible packages required by the selected Unity version.
- Repository root contains `Assets`, `Packages`, and `ProjectSettings`, allowing Unity Cloud Build to treat the repository root as the Unity project root.
- WebGL output is intended to be downloaded as a zip from Unity Cloud Build and uploaded to itch.io.

## Validation
Validation focuses on project correctness, not audio encoding. Checks include:
- all eight page states exist;
- 450x900 Canvas reference resolution is configured;
- all six scene interactions gate Next/Finish correctly;
- Back, Home, Credits, Restart, and Start Journey navigation works;
- eight progress dots reflect the current page;
- all 18 expected MP3 filenames are present under `Assets/Resources/Audio/`;
- no audio transcoding or codec-inspection step is introduced;
- C# scripts compile against the selected Unity project version;
- the project contains the files Unity Cloud Build needs for a WebGL build.

## Non-goals
- Do not redesign the story, copy, visual hierarchy, or scene order.
- Do not replace the prototype with a WebView.
- Do not add analytics, login, networking, ads, or backend services.
- Do not encode, transcode, inspect, or validate the supplied MP3 files.
