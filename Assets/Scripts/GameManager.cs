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
    
    #endregion

    #region Buttons
    public void StartGame()
    {
        UpdateGameState(GameState.Level1);
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
        Level1
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

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Menu:
                break;
            case GameState.Level1:
                SceneManager.LoadScene("GameScene");

                StartCoroutine(SpawnItems());
                break;
            default:
                break;
        }
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
        UpdateGameState(GameState.Level1);
    }
    #endregion

    #region Items
    private IEnumerator SpawnItems()
    {
        List<UnityEngine.Object> items = Resources.LoadAll("ItemPics", typeof(Texture)).ToList();
        GameObject itemPrefab = Resources.Load("ItemPrefab") as GameObject;
        var spawnPoint = new Vector2(-13, 0);
        for (int i = 0; i < 10; i++)
        {
            var index = Random.Range(0, items.Count);
            var newTexture = items[index] as Texture2D;
            var newSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), Vector2.zero);
            itemPrefab.GetComponent<SpriteRenderer>().sprite = newSprite as Sprite;

            // newItem.SetActive(true);
            Instantiate(itemPrefab, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }
    #endregion
}