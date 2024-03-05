using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private int numberOfPlayer;
    private List<PlayerControl> playerList;
    
    public PlayerControl playerQ;
    public TextMeshProUGUI playerQScoreText;
    // public int playerQScore;
    
    public PlayerControl playerP;
    public TextMeshProUGUI playerPScoreText;
    // public int playerPScore;
    
    public PlayerControl playerZ;
    public TextMeshProUGUI playerZScoreText;
    // public int playerZScore;
    
    public PlayerControl playerM;
    public TextMeshProUGUI playerMScoreText;
    // public int playerMScore;

    [SerializeField]
    private List<GameObject> band;

    public static bool isAllPressed;

    // Start is called before the first frame update
    void Start()
    {
        numberOfPlayer = GameManager.instance.numberOfPlayers;
        SetPlayerNumber();
        isAllPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreDisplay();
        band = GameObject.FindGameObjectsWithTag("Musician").ToList();
        if(playerQ.enabled) playerQ.TryKill(band);
        if(playerP.enabled) playerP.TryKill(band);
        if(playerZ.enabled) playerZ.TryKill(band);
        if(playerM.enabled) playerM.TryKill(band);

        if (GameManager.instance.gameState == GameManager.GameState.isInGame)
        {
            foreach (var player in playerList)
            {
                if (!player.isPressed) return;
            }

            if(!isAllPressed) isAllPressed = true;
        }
    }

    void UpdateScoreDisplay()
    {
        playerQScoreText.text = GameManager.instance.playerQScore + " pt";
        playerPScoreText.text = GameManager.instance.playerPScore + " pt";
        playerZScoreText.text = GameManager.instance.playerZScore + " pt";
        playerMScoreText.text = GameManager.instance.playerMScore + " pt";
    }

    void SetPlayerNumber()
    {
        switch (numberOfPlayer)
        {
            case 2:
                playerZ.gameObject.SetActive(false);
                playerZ.enabled = false;
                playerM.gameObject.SetActive(false);
                playerM.enabled = false;
                
                playerList = new List<PlayerControl>();
                playerList.Add(playerQ);
                playerList.Add(playerP);
                break;
            case 3:
                playerZ.gameObject.SetActive(false);
                playerM.enabled = false;
                
                playerList = new List<PlayerControl>();
                playerList.Add(playerQ);
                playerList.Add(playerP);
                playerList.Add(playerZ);
                break;
            case 4:
                playerList = new List<PlayerControl>();
                playerList.Add(playerQ);
                playerList.Add(playerP);
                playerList.Add(playerZ);
                playerList.Add(playerM);
                break;
            default:
                break;
        }
    }

}
