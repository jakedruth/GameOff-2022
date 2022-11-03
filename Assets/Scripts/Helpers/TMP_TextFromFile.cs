using UnityEngine;

[RequireComponent(typeof(TMPro.TMP_Text))]
public class TMP_TextFromFile : MonoBehaviour
{
    private TMPro.TMP_Text _text;
    public string path;

    public void Awake()
    {
        _text = GetComponent<TMPro.TMP_Text>();
        LoadTextAssetAndApply();
    }

    [ContextMenu("Load Text Asset")]
    public void LoadTextAssetAndApply()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        if (textAsset == null)
        {
            throw new System.Exception($"Text Asset could not be loaded from path: {path}");
        }

        _text.text = textAsset.text;
    }
}