using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxControl : MonoBehaviour
{
    public float maxY;
    public float minY;

    float speed = 0;
    public float maxSpeed = 20;
    public float minSpeed = 0;
    public float acc = 1;
    public bool up = true;

    public int rotateSpd = 1;
    // Start is called before the first frame update

    float size;

    void Start()
    {
        speed = minSpeed;
        size = maxY - minY;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKeyDown)
        {
            up = !up;
            speed = minSpeed;
        }


        transform.localEulerAngles = Vector3.forward * ((transform.position.y - minY) / size) * 360 * rotateSpd;
    }

    private void FixedUpdate()
    {

        if (up && transform.position.y < maxY)
        {
            speed += acc;
            if (speed > maxSpeed) speed = maxSpeed;
            transform.Translate(Vector2.up * speed * Time.deltaTime,Space.World);
        }
        else if (!up && transform.position.y > minY)
        {
            speed += acc;
            if (speed > maxSpeed) speed = maxSpeed;

            transform.Translate(Vector2.down * speed * Time.deltaTime, Space.World);
        }

        if (transform.position.y > maxY)
        {
            transform.position = new Vector2(0, maxY);
            speed = minSpeed;

        }
        if (transform.position.y < minY)
        {
            transform.position = new Vector2(0, minY);
            speed = minSpeed;

        }

    }
}
