using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float speed = 2.5f;
    public int value;
    public enum Type
    {
        Healthy,
        Unhealthy,
        HealthPack
    }
    public Type type;
    public bool moving = true;

    void Start()
    {
        switch (type)
        {
            case Type.Healthy:
                value = 50;
                break;
            case Type.Unhealthy:
                value = 10;
                break;
            case Type.HealthPack:
                value = 40;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (moving)
        {
            transform.position = new Vector2(transform.position.x + (speed * Time.deltaTime), transform.position.y);
        }
    }
}
