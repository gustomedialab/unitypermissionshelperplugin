# Unity Permissions Helper Plugin #

We’ve been working on mobile AR applications for a while, and the flow for asking for permissions on IOS always bugged us. We wanted to be able to control the flow better for users, rather than having a lot of dialogs pop up willy nilly. There are a lot of great unity plugins out there, but we didn’t see anything simple that did what we want. Thus: PR Permissions Helper.

PR Permissions Helper is a Unity IOS plugin that lets the developer control how and when you ask your users for permissions. It contains a core plugin class that supports a few permissions common permissions and it can be easily extended to add others.

The package also contains an example usage flow in the Examples folder. In this case, a bootstrap scene contains code that determines whether or not permissions are needed, and forwards to either a main scene or a permissions scene based on this. Feel free to just skin these scenes / components or use them as is. 


## Getting Started ##

If you just want to use the flow as defined in the examples - grab the unity package/include in your code and skin the UI as you see fit. If you want to create your own flow - you can interact with the PermissionsHelperPlugin class directly. Simply:
 
1. Call PermissionsHelperPlugin.Instance.SetRequiredPermissions early in your app lifecycle. Then
2. Use PermissionsHelperPlugin methods to either check if permissions are granted, or ask for a given permission.

## What if I need other permissions? ##
If your app uses the microphone & apple speech recognition, just grab the with_speech_reco branch (that’s what we use for Babble Rabbit). If you need to ask for other permissions, you can modify the objective C/C# plugin files as follows:

1. Add additional const int fields to the UnityIOSPermissionsHelper.mm file as needed (follow examples there)
2. Add a method that requests the permission you need (will vary based on permission, but the model is usually similar to what’s in the class already). 
3. Update the switch statement in ::requestPermission to call your new method.
4. Add a GetXXXPermission method that gets the permission status you need from ios and returns the appropriate code (following existing examples)
5. Update the switch statement in ::getPermissionStatus to use your method.
6. In the C# plugin file (PermissionHelperPlugin.cs) add a new enum to PermissionType enums that matches the const int you added in the .mm file.
7. Add the appropriate plist entry to the dictionary in PermissionsHelperPostBuild.cs file - this will automatically add the plist entry you need when the project is built. 
8. If your permission requires additional frameworks, include them either using unity settings on the .mm file, or if they are not available there, you can add them to the list of frameworks you want added in the PermissionsHelperPostBuild.cs file.
9. Update the example scenes to include your permission: 
    1. Include in the boot strap scenes list of required permissions (on the BootstrapSceneSelector object) and
    2. Add another row to the vertical list of permissions in the RequestPermissions scene.

## License ##

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details


