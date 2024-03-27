# Unity Standard URP Template
This repo contains,
1. A Project Template (`com.unity.template.aeg-3d-urp-0.1.7.tgz`) to start your Unity projects with.
2. An examples of a project set up with this template. 

The intention of this work is to allow new prototype projects to be created fast.

# Compatibility
Tested with 2022.2 and 2022.3. Recommend 2022.3.

Note: Not tested with Unity 6. Likely to need rework, as Unity 6 project will be created with URP as standard.

# Included Setup Steps
1. Update your default packages with URP, VFX Graph, Shader Graph, the new Input System and more.
2. Add a new default scene, with a gameobject hierarchy.
3. Add a standard hierarchy to the Assets directory.
4. Basic URP setup and configuration, with realtime lighting by default.
5. Add a basic spinning cube to check your build.

Helper features also included,
- Update the standard editor c# template.
- Revert back to the default package manifest
- Deploy a .gitignore file and basic GitHub workflow files.

# Installation & Use
1. Copy the template file (`com.unity.template.aeg-3d-urp-0.1.7.tgz`) to to your editor path,
`<EDITORPATH>\Editor\2022_3_14\Editor\Data\Resources\PackageManager\ProjectTemplate`
2. Open Unity Hub, and create a project with the new template (`aeg-3d-urp-0.1.7`).
3. When the project opens, double click the `AEGStandardProjectSetup.unitypackage` to extract it. 
4. 'Tools' will be added to the top menu. Click Tools -> Initial Setup Tools and follow the steps.

# Known Issues
Issue: After updating the Package Manifest, the project will restart to update the Input backend. On restart, the project will hang on the startup screen.
Workaround: Force close the project and open it again.

Issue: Spinning cube setup requires manual work.
Workaround: Read the popup when job is ran and follow the instructions.