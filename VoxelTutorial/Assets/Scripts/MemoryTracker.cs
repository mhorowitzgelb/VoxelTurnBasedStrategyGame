using UnityEngine;
using System.Collections;

public class MemoryTracker : MonoBehaviour {

    void OnGUI()
    {
        GUI.TextArea(new Rect(10, 10, 300, 100), System.GC.GetTotalMemory(true) +"");
    }
}
