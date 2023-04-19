using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ReloadUI : MonoBehaviour
{
    public GameObject bar;
    public GameObject gauge;

    Bounds barBounds;

    internal void BarUpdate(float rate)
    {
        
        if(rate >= 1||rate == 0) bar.SetActive(false);
        else
        {
            bar.SetActive(true);

            gauge.transform.localPosition = new Vector2(barBounds.min.x + barBounds.size.x * rate,0);
        }
    }

    private void Awake()
    {
        barBounds = bar.GetComponent<BoxCollider2D>().bounds;

        bar.SetActive(false);
    }

}
