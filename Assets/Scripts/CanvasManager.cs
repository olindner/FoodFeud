using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    #region Singleton and Basic Func
    private static CanvasManager _instance;
    private static bool applicationIsQuitting = false;

    public static CanvasManager Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                return null;
            }

            if (_instance == null)
            {
                GameObject go = new GameObject("CanvasManager");
                go.AddComponent<CanvasManager>();
            }
 
            return _instance;
        }
    }
 
    private void Awake()
    {
        if (_instance) 
        {
            Destroy(gameObject);
        }
        else 
        {
            _instance = this;
        }
        GameManager.Instance.ItemConsumedAction += ItemConsumed;
        
        DontDestroyOnLoad(this);
    }

    public void OnDestroy()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.ItemConsumedAction -= ItemConsumed;
        }

        applicationIsQuitting = true;
    }
    #endregion

    private GameObject Face0;
    private GameObject Face1;
    private GameObject Face2;
    private GameObject PointsTextGameObject;

    public void Initialize()
    {
        Face0 = GameObject.Find("Face0");
        PointsTextGameObject = GameObject.Find("PointsValue");
        UpdateBars(0);
    }

    public void InitializeLevel2()
    {
        Face1 = GameObject.Find("Face1");
        UpdateBars(1);
    }

    public void InitializeLevel3()
    {
        Face1 = GameObject.Find("Face2");
        UpdateBars(2);
    }

    private void ItemConsumed(int totalPoints, int index)
    {
        PointsTextGameObject.GetComponent<Text>().text = totalPoints.ToString();
        UpdateBars(index);
    }

    public void UpdateBars(int index)
	{
        GameObject face = IndexToFace(index);
        Face faceComponent = face.GetComponent<Face>();
        RectTransform foodGlobe = faceComponent.foodGlobe;
        RectTransform healthGlobe = faceComponent.healthGlobe;

		float foodRatio = faceComponent.currentFood / faceComponent.maxFood;
		float healthRatio = faceComponent.currentHealth / faceComponent.maxHealth;

		foodGlobe.localPosition = new Vector3(0, foodGlobe.rect.height * foodRatio - foodGlobe.rect.height, 0);
		healthGlobe.localPosition = new Vector3(0, healthGlobe.rect.height * healthRatio - healthGlobe.rect.height, 0);
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
