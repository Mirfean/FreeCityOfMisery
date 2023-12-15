using PixelCrushers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInput: MonoBehaviour
{
#if USE_NEW_INPUT
    protected static bool isRegistered = false;
    private bool didIRegister = false;
    private Triangles controls;
    void Awake()
    {
        controls = new Triangles();
    }
    void OnEnable()
    {
        if (!isRegistered)
        {
            isRegistered = true;
            didIRegister = true;
            controls.Enable();
            InputDeviceManager.RegisterInputAction("Back", controls.UI.Cancel);
            InputDeviceManager.RegisterInputAction("Interact", controls.UI.Submit);
        }
    }
    void OnDisable()
    {
        if (didIRegister)
        {
            isRegistered = false;
            didIRegister = false;
            controls.Disable();
            InputDeviceManager.UnregisterInputAction("Back");
            InputDeviceManager.UnregisterInputAction("Interact");
        }
    }
#endif
}
