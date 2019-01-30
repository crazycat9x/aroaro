# Aroaro
This project is a collection of modules that together form a VR collaborative learning environment

# Notes
The project used some blender models, Blender need to be installed to run and build the project via Unity and you may need to reimport models in the Models folder

# Prefabs
- ViewLoader: provide an easy way to develop and debug scene, will load all needed component to the scene this prefab attached to
- Avatar: representing the remote player and utility menu for that player (mute, make invisible, etc)
- Canvas: a whiteboard-like networked drawing canvas
- Pen: a grabbable pen that draws on drawable elements
- ControlPanel: attached to the user left controller, provide all customizable options

# Scripts

- PUNNetworkManager.cs - Handle user interaction and synchronization
- AvatarSetupManager.cs - Hold all information and setups related to the avatar
- AvatarTransformFollow.cs - Map avatar components to controllers, headset and synchronize it across the network
- ControllableObject.cs - Inherit from VRTK_InteractableObject, add ability for object interaction to be sync accross the network
- ControlPanelManager.cs - Management script for controlling local customizable options
- RemotePlayerMenu.cs - Management script for controlling the remote player options
- Drawable.cs - Add ability for an object to be drawn on and have that drawing synchronized between the user
- Pen.cs - For use with the pen prefab


