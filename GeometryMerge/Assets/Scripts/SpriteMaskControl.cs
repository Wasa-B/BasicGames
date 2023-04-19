using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(SpriteMask))]
public class SpriteMaskControl : MonoBehaviour
{
    public SpriteRenderer originSprite;
    SpriteMask mask;
    SpriteRenderer sRenderer;

    float blinkTime;

    private void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        mask = GetComponent<SpriteMask>();
        originSprite.GetComponent<WasabiGame.IDamagedObject>()?.AddEvent(BlinkMask);
    }

    public void BlinkMask()
    {
        blinkTime = .1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(blinkTime > 0)
        {
            blinkTime -= Time.deltaTime;
            mask.enabled = true;
            mask.sprite = originSprite.sprite;
            if (blinkTime <= 0)
                mask.enabled = false;
        }
    }
}
