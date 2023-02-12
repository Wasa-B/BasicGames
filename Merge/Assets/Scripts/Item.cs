using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DragManager))]
public class Item : MonoBehaviour
{
    public int number;
    [SerializeField] Color[] colors;

    public void SetItem(int newValue, Transform newParent)
    {
        number = newValue;
        GetComponent<Image>().color = SetColor(number);
        GetComponentInChildren<Text>().text = number.ToString();
        transform.SetParent(newParent);

    }

    public Color SetColor(int colorValue)
    {
        if (colorValue < 10)
            return colors[colorValue - 1];
        else
            return Color.black;
    }
}
