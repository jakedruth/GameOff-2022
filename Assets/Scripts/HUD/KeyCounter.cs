using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCounter : MonoBehaviour
{
    TMPro.TMP_Text _text;

    protected void Awake()
    {
        _text = GetComponentInChildren<TMPro.TMP_Text>();
        SetKeyCount(0);
    }

    public void SetKeyCount(int amount)
    {
        _text.text = $"<sprite=\"Key\" name=\"Key\"> - {amount}";
    }
}
