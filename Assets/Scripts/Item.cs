using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float speed = 2.5f;
    public enum Type
    {
        Healthy,
        Unhealthy,
        HealthPack
    }
    public Type type;
    public bool moving = true;

    void Update()
    {
        if (moving)
        {
            transform.position = new Vector2(transform.position.x + (speed * Time.deltaTime), transform.position.y);
        }
    }
}
