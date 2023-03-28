using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    #region Singleton and Basic Func
    public static CanvasManager Instance;

    private GameObject Face0;
    private GameObject Face1;
    private GameObject Face2;
    
    void Awake()
    {
        Instance = this;

        GameManager.Instance.FoodEatenAction += FoodEaten;

        DontDestroyOnLoad(this);
    }

    public void Initialize()
    {
        Face0 = GameObject.Find("Face0");
        UpdateBars(0);
    }

    void OnDestroy()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.FoodEatenAction -= FoodEaten;
        }
    }
    #endregion

    private void FoodEaten(int index)
    {
        // Display visual change
        UpdateBars(index);
    }

    public void UpdateBars(int index)
	{
        GameObject face = IndexToFace(index);
        GameObject foodBar = face.GetComponent<Face>().foodBar;
        GameObject healthBar = face.GetComponent<Face>().healthBar;

		float foodRatio = face.GetComponent<Face>().currentFood / face.GetComponent<Face>().maxFood;
		float healthRatio = face.GetComponent<Face>().currentHealth / face.GetComponent<Face>().maxHealth;

		foodBar.GetComponent<RectTransform>().localPosition = new Vector3(foodBar.GetComponent<RectTransform>().rect.width * foodRatio - foodBar.GetComponent<RectTransform>().rect.width, 0, 0);
		healthBar.GetComponent<RectTransform>().localPosition = new Vector3(healthBar.GetComponent<RectTransform>().rect.width * healthRatio - healthBar.GetComponent<RectTransform>().rect.width, 0, 0);
	}

    private GameObject IndexToFace(int index)
    {
        switch (index)
        {
            case 0:
                return Face0;
            case 1:
                return Face1;
            case 2:
                return Face2;
            default:
                return null;
        }
    }
}
