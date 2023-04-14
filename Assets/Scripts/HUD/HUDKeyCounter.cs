using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDKeyCounter : MonoBehaviour
{
    TMPro.TMP_Text _text;

    protected void Awake()
    {
        _text = GetComponentInChildren<TMPro.TMP_Text>();
    }

    public void SetKeyCount(int amount)
    {
        _text.text = $"<sprite=\"Key\" name=\"Key\"> - {amount}";
    }
}
