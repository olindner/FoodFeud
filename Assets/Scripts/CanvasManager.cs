using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    #region Singleton and Basic Func
    public static CanvasManager Instance;
    private float maxFood = 100;
    private float maxHealth = 100;
    
    private float currentFood = 50f;
    private float currentHealth = 100f;


    void Awake()
    {
        Instance = this;

        GameManager.Instance.FoodEatenAction += FoodEaten;

        DontDestroyOnLoad(this);
    }

    public void Initialize()
    {
        FoodBar = GameObject.Find("FoodBarInsides");
        FoodText = GameObject.Find("FoodBarText");
        UpdateFoodBar();
    }

    void OnDestroy()
    {
        GameManager.Instance.FoodEatenAction -= FoodEaten;
    }
    #endregion

    private void FoodEaten(int amount)
    {
        currentFood = (int)Mathf.Min(currentFood + amount, maxFood);
        UpdateFoodBar();
    }

    private GameObject FoodBar;
    private GameObject FoodText;
    private GameObject HealthBar;

    private void UpdateFoodBar()
	{
		float ratio = currentFood / maxFood;
		FoodBar.GetComponent<RectTransform>().localPosition = new Vector3(FoodBar.GetComponent<RectTransform>().rect.width * ratio - FoodBar.GetComponent<RectTransform>().rect.width, 0, 0);
		FoodText.GetComponent<Text>().text = currentFood.ToString ("0") + "/" + maxFood.ToString ("0");
	}
}
