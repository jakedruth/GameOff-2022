using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Device Display Settings", menuName = "GameOff-2022/Device Display Settings")]
public class DeviceDisplaySettings_SO : ScriptableObject
{
    [Header("Control Scheme name")]
    public string controlSchemeName;
    public string deviceDisplayName;

    [Header("Rich Text Tags")]
    public string moveTag;
    public string interactTag;
    public string swordTag;
    public string boomerangTag;
}
