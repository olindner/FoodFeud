using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Sprite pressed;
    public Sprite unpressed;

    public void OnMouseOver()
    {
        gameObject.GetComponent<Image>().sprite = pressed;
    }

    public void OnMouseExit()
    {
        gameObject.GetComponent<Image>().sprite = unpressed;
    }
}
