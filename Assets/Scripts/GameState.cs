using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GameState : MonoBehaviour
{
    [Header("menu Panels")]
    public Button start;
    public Button restart;
    public Button quit;
    public Button resume;
    public TMP_Text UI_Countdown;

    [Header("main StateMaschine at the Moment")]
    [Tooltip("....")]
    public GameObject player;
    public Canvas menu;
    public Canvas playerUI;
    public Spawner spawner;
    public TerrainHandler terrainHandler;
    public float countdown = 300.0f;
    private float time;
    public bool isRunning = false;
    public bool isPaused = false;

    // Start is called before the first frame update
    private void Awake()
    {
        //on ApplicationStart set the playerObject InActive  
        player.SetActive(false);
        playerUI.enabled = false;
    }
    void Start()
    {
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
        /*MainGame Loop*/
        if(isRunning&&!isPaused)
        {
            time += Time.deltaTime;
            UI_Countdown.text = Mathf.RoundToInt(countdown-time) + "seconds left";
            if (time >= countdown)
            { GameOver(); }
        }
        
    }
    private void GameOver()
    {
        isRunning = false;
        playerUI.enabled = false;
        menu.enabled = true;
        restart.gameObject.SetActive(true);
        resume.gameObject.SetActive(false);
       
    }
    public void StartGame()
    {
        
        isRunning = true;
        menu.enabled = false;
        playerUI.enabled = true;

        player.transform.position = terrainHandler.GetRandomPosition();
        player.SetActive(true);

       
        start.gameObject.SetActive(false);
        restart.gameObject.SetActive(true);
        resume.gameObject.SetActive(true);
        /*mit event caaall ersetzen*/
        spawner.SpawnItems();

        
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
        time = 0;
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
            playerUI.enabled = true;
            isRunning = true;
            isPaused = false;
        }
        else
        {
            menu.enabled = true;
            playerUI.enabled = false;
            isRunning = false;
            isPaused = true;
        }
    }
    private void OnEnable()
    {
        EventManager.StartListening("ItemFound", AddItem);
    }
    private void OnDisable()
    {
        EventManager.StopListening("ItemFound", AddItem);

    }
    void AddItem()
    {
        Debug.Log("Event");
    }
}
