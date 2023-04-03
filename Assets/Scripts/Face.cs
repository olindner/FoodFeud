using System.Collections;
using System.Collections.Generic;
using Type = Item.Type;
using UnityEngine;

public class Face : MonoBehaviour
{
    public int index;
    public RectTransform foodGlobe;
    public RectTransform healthGlobe;
    public float currentFood = 75f;
    public float currentHealth = 100f;
    public float greenDuration = 0.5f;
    public float multiplier = 8f; // Hack bc idk why the lerping with deltatime doesn't work accurately
    public float maxFood = 100f;
    public float maxHealth = 100f;

    private ParticleSystem particleSystem;
    private float foodRefillValue = 10f;
    private float healthRefillValue = 50f;
    private float hungerTickInSeconds = 1f;
    private float hungerTickValue = 2f;
    private float unhealthyPenalty = 15f;

    void Start()
    {
        foodGlobe = GameObject.Find($"FoodGlobe{index}Insides").GetComponent<RectTransform>();
        healthGlobe = GameObject.Find($"HealthGlobe{index}Insides").GetComponent<RectTransform>();
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();
        StartCoroutine(HungerTick());
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Item")
        {
            Destroy(col.gameObject);

            switch (col.gameObject.GetComponent<Item>().type)
            {
                case Type.Healthy:
                    currentFood = Mathf.Min(currentFood + foodRefillValue, maxFood);
                    StartCoroutine(StarBurst());
                    break;
                case Type.Unhealthy:
                    // Eating unhealthy food replenishes hunger, but only at half effectiveness!
                    currentFood = Mathf.Min(currentFood + foodRefillValue/2, maxFood);
                    currentHealth = Mathf.Max(0, currentHealth - unhealthyPenalty);
                    StartCoroutine(TintGreen(gameObject.GetComponent<SpriteRenderer>()));
                    break;
                case Type.HealthPack:
                    currentHealth = Mathf.Min(currentHealth + healthRefillValue, maxHealth);
                    StartCoroutine(StarBurst());
                    break;
                default:
                    break;
            }

            GameManager.Instance.ItemConsumed(col.gameObject, index);
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

    private IEnumerator TintGreen(SpriteRenderer sprite)
    {
        Color normal = new Color(1f, 1f, 1f, 1f);
        Color green = new Color(0f, 1f, 0f, 1f);
        float elapsedTime = 0;
        // float duration = greenDuration / 2;

        // Fade to green
        while (elapsedTime < greenDuration)
        {
            elapsedTime += Time.deltaTime;
            var percentage = elapsedTime / multiplier;
            sprite.color = Color.Lerp(sprite.color, green, percentage);
            yield return null;
        }
        // Fade back
        elapsedTime = 0;
        while (elapsedTime < greenDuration)
        {
            elapsedTime += Time.deltaTime;
            var percentage = elapsedTime / multiplier;
            sprite.color = Color.Lerp(sprite.color, normal, percentage);
            yield return null;
        }
        yield return null;
    }
}
