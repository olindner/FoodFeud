using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    public int index;
    public GameObject foodBar;
    public GameObject healthBar;
    public float currentFood = 25f;
    public float currentHealth = 100f;
    public float maxFood = 100f;
    public float maxHealth = 100f;

    private ParticleSystem particleSystem;
    private float foodRefillValue = 10f;
    private float hungerTickInSeconds = 1f;
    private float hungerTickValue = 2f;
    private float unhealthyPenalty = 15f;

    void Start()
    {
        foodBar = GameObject.Find($"FoodBar{index}Insides");
        healthBar = GameObject.Find($"HealthBar{index}Insides");
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();
        StartCoroutine(HungerTick());
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Item")
        {
            Destroy(col.gameObject);

            if (col.gameObject.GetComponent<Item>().healthy)
            {
                currentFood = Mathf.Min(currentFood + foodRefillValue, maxFood);
                StartCoroutine(StarBurst());
            }
            else
            {
                // Eating unhealthy food replenishes hunger, but only at half effectiveness!
                currentFood = Mathf.Min(currentFood + foodRefillValue/2, maxFood);
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

    private IEnumerator StarBurst()
    {
        particleSystem.Play();

        yield return new WaitForSeconds(0.1f);

        particleSystem.Stop();
    }
}
