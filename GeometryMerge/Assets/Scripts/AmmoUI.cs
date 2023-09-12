using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AmmoUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textMeshPro;
    public GridLayoutGroup gaugeUI;
    public GameObject bulletGaugePrefabs;
    const float bulletSizeMax = 1000;

    int bulletMax = 1;
    float bulletSize = bulletSizeMax;
    int currentBullet = 0;

    List<Animator> gaugeList = new List<Animator>();
    private void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            gaugeList.Add(Instantiate(bulletGaugePrefabs, gaugeUI.transform).GetComponent<Animator>());
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void UpdateCount(int count, int max)
    {
        currentBullet = count;
        textMeshPro.text = count.ToString() + " / " + max.ToString();

        if (max > gaugeList.Count)
        {
            for (int i = gaugeList.Count; i <= max; ++i)
                gaugeList.Add(Instantiate(bulletGaugePrefabs, gaugeUI.transform).GetComponent<Animator>());
        }
        for (int i = 0; i < gaugeList.Count; i++)
        {
            if (i < currentBullet)
            {
                gaugeList[i].gameObject.SetActive(true);
                gaugeList[i].Play("Idle");
            }
            else if (i >= max)
            {
                if (gaugeList[i].gameObject.activeSelf)
                {
                    gaugeList[i].Play("Idle");
                    gaugeList[i].gameObject.SetActive(false);
                }
            }
            else if(gaugeList[i].gameObject.activeSelf)
                gaugeList[i].Play("AmmoGaugeUse");
            
        }

        if (max != bulletMax)
            UpdateGridSize(max);
    }

    void UpdateGridSize(int max)
    {


        bulletMax = max;

        gaugeUI.cellSize = new Vector2(gaugeUI.cellSize.x, bulletSizeMax / max);
    }
}
