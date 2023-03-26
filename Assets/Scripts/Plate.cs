using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve curve;

    private Vector3 startPos;
    private Vector3 endPos = new Vector3(0, -1.5f, 0);
    private float desiredDuration = 1f;
    private float elapsedTime;
    private bool moving;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moving = true;
        }

        if (moving)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / desiredDuration;
            if (percentageComplete >= 0.98f)
            {
                transform.position = startPos;
                elapsedTime = 0;
                moving = false;
                return;
            }

            transform.position = Vector3.Lerp(startPos, endPos, curve.Evaluate(percentageComplete));
        }
    }
}
