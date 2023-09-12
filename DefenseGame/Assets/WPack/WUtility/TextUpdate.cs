using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdate : MonoBehaviour
{
    protected TMPro.TMP_Text text;
    private void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    public virtual void SetText(int _value)
    {
        text.SetText(_value.ToString());
    }
    public virtual void SetText(string _value)
    {
        text.SetText(_value);
    }
}
