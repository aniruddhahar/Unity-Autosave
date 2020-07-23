using UnityEngine;
public class AutosavePrefs : ScriptableObject
{

    public bool enable = true, log = true, onPlay = false;
    public int interval = 10;

    //public AutosavePrefs(bool e, bool l, bool p, int i)
    //{

    //    enable = e; log = l; p = onPlay; interval = i;

    //}
    //public AutosavePrefs()
    //{
    //    enable = true; log = true; onPlay = false; int interval = 2;

    //}


}