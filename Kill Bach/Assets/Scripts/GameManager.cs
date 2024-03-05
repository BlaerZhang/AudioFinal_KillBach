using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState
    {
        isPreparing,
        isInGame,
        isGameOver
    }

    public GameState gameState;

    public int numberOfPlayers;

    [Header("Player Score")]
    public int playerQScore;
    public int playerPScore;
    public int playerZScore;
    public int playerMScore;

    [Header("Text")]
    public TextMeshProUGUI endingText;
    public TextMeshProUGUI nightText;

    private BandSpawner _bandSpawner;
    private bool _isMovingRight;
    private Camera mainCam;
    private int currentRound;
    private List<int> playerScores;

    private bool isRepeat = false;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        _bandSpawner = GetComponent<BandSpawner>();
        mainCam = GetComponentInChildren<Camera>();
        gameState = GameState.isPreparing;
        currentRound = 1;

        Prepare();
    }


    void Update()
    {
        PressRReload();
        SelectNumberOfPlayer();
        
        switch (gameState)
        {
            case GameState.isPreparing:
                break;
            case GameState.isInGame:
                if (PlayerManager.isAllPressed) gameState = GameState.isGameOver;
                break;
            case GameState.isGameOver:
                endingText.text = "FINALE";

                if (currentRound < 3)
                {
                    Sequence roundEnding = DOTween.Sequence();
                    roundEnding
                        .Append(endingText.DOText("FINALE", 0))
                        .AppendInterval(2)
                        .Append(endingText.DOText(CalculateLeader() + " Leads", 0f))
                        .AppendInterval(3)
                        .Append(endingText.DOText("",0f))
                        .OnComplete((() =>
                        {
                            ResetRound();
                        }));

                    roundEnding.Play();
                    gameState = GameState.isPreparing;
                }
                else
                {
                    Sequence roundEnding = DOTween.Sequence();
                    roundEnding
                        .Append(endingText.DOText("FINALE", 0))
                        .AppendInterval(2)
                        .Append(endingText.DOText(CalculateLeader() + " Wins", 0f))
                        .AppendInterval(3)
                        .Append(endingText.DOText("",0f))
                        .OnComplete((() =>
                        {
                            ResetGame();
                        }));

                    roundEnding.Play();
                    gameState = GameState.isPreparing;
                }
               
                break;
        }
    }

    void PressRReload()
    {
        if (Input.GetKeyDown(KeyCode.R)) ResetGame();
    }

    void Prepare()
    {
        _bandSpawner.ShuffleBandList();
        _isMovingRight = Random.value > 0.5f;
        _bandSpawner.SpawnBand(3, _isMovingRight);
        StartCoroutine(GoIntoNight());
    }

    IEnumerator GoIntoNight()
    {
        yield return new WaitForSeconds(15f);
        
        //TODO: Play a clock sound
        
        mainCam.DOColor(Color.black, 5f).OnComplete((() =>
        {
            gameState = GameState.isInGame;
            GameObject[] rehersalBand = GameObject.FindGameObjectsWithTag("Musician");
            foreach (var musician in rehersalBand) Destroy(musician);
            _bandSpawner.SpawnBand(1f, _isMovingRight);
        }));
    }
    
    string CalculateLeader()
    {
        playerScores = new List<int>();
        playerScores.Add(playerQScore);
        playerScores.Add(playerPScore);
        playerScores.Add(playerZScore);
        playerScores.Add(playerMScore);
        
        string leaderName = "Nobody";
        int currentHighScore = -5;

        foreach (var score in playerScores)
        {
            if (score >= currentHighScore)
            {
                if (score == currentHighScore)
                {
                    isRepeat = true;
                    continue;
                }
                currentHighScore = score;
                isRepeat = false;
            }
        }


        if (!isRepeat)
        {
            if (currentHighScore == playerQScore) leaderName = "Q";
            
            if (currentHighScore == playerPScore) leaderName = "P";
            
            if (currentHighScore == playerZScore) leaderName = "Z";
            
            if (currentHighScore == playerMScore) leaderName = "M";
        }
      

        return leaderName;
    }

    void ResetRound()
    {
        currentRound++;
        mainCam.backgroundColor = Color.white;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StopAllCoroutines();

        Invoke("Prepare", 0.1f);
    }

    void ResetGame()
    {
        ResetRound();
        currentRound = 1;
        playerMScore = playerPScore = playerQScore = playerZScore = 0;
    }

    void SelectNumberOfPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            numberOfPlayers = 2;
            ResetGame();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            numberOfPlayers = 3;
            ResetGame();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            numberOfPlayers = 4;
            ResetGame();
        }
    }
    
}
