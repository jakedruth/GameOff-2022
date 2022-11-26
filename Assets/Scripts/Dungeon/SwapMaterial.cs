using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMaterial : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Material newMaterial;

    public void Swap()
    {
        meshRenderer.material = newMaterial;
    }
}
