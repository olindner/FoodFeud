using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    public int index;
    public GameObject foodBar;
    public GameObject healthBar;
    public float currentFood = 50f;
    public float currentHealth = 100f;
    public float maxFood = 100f;
    public float maxHealth = 100f;

    private float foodRefillValue = 20f;
    private float hungerTickInSeconds = 5f;
    private float hungerTickValue = 10f;
    private float unhealthyPenalty = 10f;

    void Start()
    {
        foodBar = GameObject.Find($"FoodBar{index}Insides");
        healthBar = GameObject.Find($"HealthBar{index}Insides");
        StartCoroutine(HungerTick());
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Item")
        {
            Destroy(col.gameObject);
            currentFood = Mathf.Min(currentFood + foodRefillValue, maxFood);
            if (!col.gameObject.GetComponent<Item>().healthy)
            {
                currentHealth = Mathf.Max(0, currentHealth - unhealthyPenalty);
            }
            GameManager.Instance.FoodEaten(col.gameObject, index);
        }
    }

    private IEnumerator HungerTick()
    {
        yield return new WaitForSeconds(hungerTickInSeconds);
        currentFood = Mathf.Max(currentFood - hungerTickValue, 0);
        CanvasManager.Instance.UpdateBars(index);
        //Do something if empty (0)
        StartCoroutine(HungerTick());
    }
}
