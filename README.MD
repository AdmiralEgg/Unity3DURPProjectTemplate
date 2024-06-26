# Unity Standard URP Template
This repo contains a Unity Project Template (`com.unity.template.aeg-3d-urp-0.1.8.tgz`) which can be used to quickly initialise a baseline Unity project with URP rendering. The intention of this template is to reduce the initial work required in setting up Unity projects and publishing builds.

## Core Unity Setup Features
- Instantly deploy a new Packages manifest, which automatically installs a number of key packages; URP, VFX Graph, Shader Graph, the new Input System, and more.
- Add new default scene, with a standardised GameObject hierarchy.
- Add a standard directory hierarchy to the Assets directory.
- Setup basic URP rendering and, and configure lighting to realtime by default.
- Add a basic spinning cube to check your build works.

## Helper Features
- Update the standard editor c# template.
- Revert back to the default package manifest
- Basic .gitignore file.

## GitHub Actions
- Trigger a 'Build Unity Project' action to build your project for multiple different platforms.
- Overnight scheduling which runs builds overnight if there are changes in your repo.
- Trigger a 'Publish to Itch' action to use Butler to push your builds to an Itch page.
- Trigger a 'Notify Discord' action to notify a Discord channel that that a new Itch build is available.

# Compatibility
Tested with 2022.2 and 2022.3. 
Recommend 2022.3.

Note: Not tested with Unity 6. Likely to need rework, as Unity 6 project will be created with URP as standard.

# Installation & Use
1. Copy the template file (`com.unity.template.aeg-3d-urp-0.1.8.tgz`) to to your editor path,
`<EDITORPATH>\Editor\2022_3_14\Editor\Data\Resources\PackageManager\ProjectTemplate`
2. Open Unity Hub, and create a project with the new template (`aeg-3d-urp-0.1.8`).
3. When the project opens, double click the `AEGStandardProjectSetup.unitypackage` to extract it. 
4. 'Tools' will be added to the top menu. Click Tools -> Initial Setup Tools and follow the steps.

# Known Issues
Issue: After updating the Package Manifest, the project will restart to update the Input backend. On restart, the project will hang on the startup screen.
Workaround: Force close the project and open it again.

Issue: Spinning cube setup requires manual work.
Workaround: Read the popup when job is ran and follow the instructions.