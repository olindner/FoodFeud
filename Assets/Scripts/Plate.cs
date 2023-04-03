using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve curve;

    public float multiplier = 1f;

    private GameObject nearbyItem;
    private Vector3 startPos;
    private Vector3 endPos;
    private float desiredDuration = 1f;
    private float elapsedTime;
    private bool moving;

    void Start()
    {
        startPos = transform.position;
        endPos = transform.position + Vector3.up * multiplier;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!moving) moving = true;

            if (nearbyItem != null)
            {
                nearbyItem.GetComponent<Item>().moving = false;
                nearbyItem.transform.SetParent(this.transform);
            }
        }

        if (moving)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / desiredDuration;
            if (percentageComplete >= 0.97f)
            {
                transform.position = startPos;
                elapsedTime = 0;
                moving = false;
                return;
            }

            transform.position = Vector3.Lerp(startPos, endPos, curve.Evaluate(percentageComplete));
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Item")
        {
            nearbyItem = col.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Item")
        {
            nearbyItem = null;
        }
    }
}
