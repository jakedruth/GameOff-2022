using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchStand : MonoBehaviour
{
    [SerializeField] private SwapMaterial _swapMaterial;
    [SerializeField] private GameObject _light;
    [SerializeField] private FloatingGameObject _orb;

    public void HandleSwitchOn()
    {
        _swapMaterial?.Swap();
        _light?.SetActive(true);
        _orb?.SetIsOn(true);
    }
}
