using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DisplayManager : MonoBehaviour
{
    public static DisplayManager instance;
    public DeviceDisplaySettings_SO[] displaySettings;

    void Awake()
    {
        instance = this;
    }

    public DeviceDisplaySettings_SO FindDisplaySetting(string controlSchemeName)
    {
        DeviceDisplaySettings_SO value = displaySettings.FirstOrDefault(ds => ds.controlSchemeName == controlSchemeName);
        return value ?? displaySettings[0];
    }
}
