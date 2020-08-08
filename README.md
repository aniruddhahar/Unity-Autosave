# Unity-Autosave
## A simple autosave plugin package for Unity using Editor Coroutines
This project utilises Editor Coroutines to implement a simple auto save function for Unity scenes. You can set it to auto save at regular intervals (in minutes), save on Play Mode, and also save all open scenes with a single click. Installation is designed to be simple, and one-time.

## IMPORTANT NOTE
If, while entering playmode, you have any objects selected in the scene window or hierarchy, You may see an error in the console similar to the following:
NullReferenceException: Object reference not set to an instance of an object
UnityEditor.GameObjectInspector.ClearPreviewCache () at [some asset ID will be shown here]
#### This is benign and likely due to a Unity bug. Read more <a href = "https://answers.unity.com/questions/1610979/nullreferenceexception-on-gameobjectinspector-edit.html">here. </a>
 
 
 
### Instructions
1. Clone or download this repository, and extract/copy the "com.aniruddhahar.unity-autosave" folder somewhere.
2. Create a new Unity project or open an existing project. Unity 2019.4 LTS or later is recommended, but it should work with 2018.1+ (Untested)
3. In the project window, right click on the 'Packages' root folder and click 'Show in Explorer' 
    OR Navigate to the project folder manually and open the 'Packages' folder (NOT the Assets folder)
4. Copy the "com.aniruddhahar.unity-autosave" folder extracted/copied from from this repository to the 'Packages' folder.
5. The package will be imported when you switch to Unity. The Editor Coroutines dependency package should also be installed automatically.
6. There should be a new "Autosave > Autosave Settings" Menu Item, which will open the settins window.
7. In the settings winodow, you can set options such as Save Interval (Minutes only), Save on Play, Console Logs and Enable/Disable Autosave.
8. <b>You must click "Update Interval" after changing any settings</b> for the change to take effect.

