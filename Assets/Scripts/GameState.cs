using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class GameState : MonoBehaviour
{
    [Header("menu Panels")]
    public Button start;
    public Button restart;
    public Button quit;
    public Button resume;

    [Header("main StateMaschine at the Moment")]
    [Tooltip("....")]
    public GameObject player;
    public Canvas menu;
    public Spawner spawner;
    public TerrainHandler terrainHandler;
    public float countdown = 300.0f;
    private float time;
    public bool isRunning = false;
    public bool isPaused = false;
    public bool justStarted = true;
    // Start is called before the first frame update
    private void Awake()
    {
        //on ApplicationStart set the playerObject InActive  
        player.SetActive(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void StartGame()
    {
        justStarted = false;
        isRunning = true;
        menu.enabled = false;

        player.transform.position = terrainHandler.GetRandomPosition();
        player.SetActive(true);

        /*for the time beeing*/
        start.gameObject.SetActive(false);
        restart.gameObject.SetActive(true);
        resume.gameObject.SetActive(true);
        /*mit event caaall ersetzen*/
        spawner.SpawnItems();

        /*TimerStart*/
        float _startTime = Time.time;
    }
    public void QuitApplication()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    public void Restart()
    {
        player.SetActive(false);
        terrainHandler.ResetBoard();
        spawner.ReSpawnItems();

        
        player.transform.position = terrainHandler.GetRandomPosition();
        player.SetActive(true);
        ToggleMenu();
    }
    public void ToggleMenu()
    {
        if (menu.isActiveAndEnabled)
        {
            menu.enabled = false;
            isRunning = true;
            isPaused = false;
        }
        else
        {
            menu.enabled = true;
            isRunning = false;
            isPaused = true;
        }
    }
}
