using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    private static bool applicationIsQuitting = false;

    public static GameManager Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                return null;
            }

            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
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
        
        DontDestroyOnLoad(this);
    }

    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }
    #endregion

    #region Observables
    public event Action<int> ItemConsumedAction;
    #endregion

    #region Buttons
    public void StartGame()
    {
        UpdateGameState(GameState.LoadGame);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region GameState

    public enum GameState
    {
        Menu,
        LoadGame,
        InitGameObjs,
        Level1,
        Level2,
        Level3
    }

    private GameState state;
    public GameState State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
        }
    }
    AsyncOperation asyncLoadLevel;

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Menu:
                break;
            case GameState.LoadGame:
                asyncLoadLevel = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
                StartCoroutine(WaitForSceneToLoad());
                break;
            case GameState.InitGameObjs:
                // Get obj references here!
                CanvasManager.Instance.Initialize();
                UpdateGameState(GameState.Level1);
                break;
            case GameState.Level1:
                StartCoroutine(SpawnItems());
                break;
            case GameState.Level2:
                // Make sure to init Canvas Manager Face 1
                break;
            case GameState.Level3:
                // Make sure to init Canvas Manager Face 2
                break;
            default:
                break;
        }
    }

    private IEnumerator WaitForSceneToLoad()
    {
        while (!asyncLoadLevel.isDone){
            yield return null;
        }

        UpdateGameState(GameState.InitGameObjs);
    }
    #endregion

    #region Audio
    private AudioClip HoverClip;
    private AudioSource audioSourcer;
    public AudioSource AudioSourcer 
    {
        get
        {
            return audioSourcer;
        }
        set 
        {
            audioSourcer = value;
        }
    }

    public void HoverMouseNoise()
    {
        AudioSourcer.PlayOneShot(HoverClip);
    }
    #endregion

    #region Built In Functions
    private void Start()
    {
        AudioSourcer = gameObject.AddComponent<AudioSource>();
        HoverClip = Resources.Load("Click") as AudioClip;

        // Remove this before deploying (hack to develop using game scene)
        UpdateGameState(GameState.InitGameObjs);
    }
    #endregion

    #region Items
    private IEnumerator SpawnItems()
    {
        List<UnityEngine.Object> healthyItems = Resources.LoadAll("HealthyPics", typeof(Texture)).ToList();
        List<UnityEngine.Object> unhealthyItems = Resources.LoadAll("UnhealthyPics", typeof(Texture)).ToList();
        GameObject itemPrefab;

        for (int i = 0; i < 100; i++)
        {
            var randomSelector = UnityEngine.Random.Range(0, 100);

            if (randomSelector < 45)
            {
                itemPrefab = Resources.Load("HealthyItemPrefab") as GameObject;
                var foodIndex = UnityEngine.Random.Range(0, healthyItems.Count);
                var newTexture = healthyItems[foodIndex] as Texture2D;
                var newSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), Vector2.zero);
                itemPrefab.GetComponent<SpriteRenderer>().sprite = newSprite as Sprite;
            }
            else if (randomSelector < 90)
            {
                itemPrefab = Resources.Load("UnhealthyItemPrefab") as GameObject;
                var foodIndex = UnityEngine.Random.Range(0, unhealthyItems.Count);
                var newTexture = unhealthyItems[foodIndex] as Texture2D;
                var newSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), Vector2.zero);
                itemPrefab.GetComponent<SpriteRenderer>().sprite = newSprite as Sprite;
            }
            else
            {
                itemPrefab = Resources.Load("HealthPackItemPrefab") as GameObject;
            }

            Instantiate(itemPrefab);

            yield return new WaitForSeconds(2f);
        }
        yield return null;
    }

    public void ItemConsumed(GameObject item, int index)
    {
        // Eventually add to points based on item's value
        ItemConsumedAction?.Invoke(index);
    }
    #endregion
}