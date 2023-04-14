using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: change to include half hearts. Maybe quarter heart too?
public class HUDHealthBar : MonoBehaviour
{
    [SerializeField] private Sprite heartFull;
    [SerializeField] private Sprite heartEmpty;

    public void SetMaxHeatCount(int amount)
    {
        int count = transform.childCount;
        for (int i = count - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < amount; i++)
        {
            CreateHeartGameObject();
        }
    }

    public void SetHealth(int value)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Image image = transform.GetChild(i).GetComponent<Image>();
            image.sprite = (i < value) ? heartFull : heartEmpty;
        }
    }

    private GameObject CreateHeartGameObject()
    {
        GameObject heart = new("Heart", typeof(RectTransform));
        heart.transform.SetParent(transform);

        RectTransform rect = heart.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(50, 50);
        rect.localScale = Vector3.one;

        Image image = heart.AddComponent<Image>();
        image.sprite = heartFull;

        return heart;
    }
}
