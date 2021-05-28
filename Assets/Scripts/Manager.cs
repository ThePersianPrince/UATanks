using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    
    public static Manager instance;
    public int playerOneLives = 2;
    public int playerTwoLives = 2;
    public int livePlayers;
    public int liveEnemies;
    public int numOfEnemies;
    public int numOfPlayers;
    public List<GameObject> players;
    public List<GameObject> enemies;
    public List<GameObject> enemyTanks;
    public List<GameObject> playerTanks;
    public bool newGame = false;
    public GameObject game;
    public GameObject options;
    public GameObject gameOver;
    public GameObject playerOneScoreText;
    public GameObject playerOneHealthText;
    public GameObject playerOneLivesText;
    public GameObject playerOnePowerupText;
    public GameObject playerOneOptionsButton;
    public GameObject playerOneGameOverImage;
    public GameObject playerTwoScoreText;
    public GameObject playerTwoHealthText;
    public GameObject playerTwoLivesText;
    public GameObject playerTwoPowerupText;
    public GameObject playerTwoOptionsButton;
    public GameObject playerTwoGameOverImage;
    public float defaultMusicVolume = 0.5f;
    public AudioSource battleMusic;

    private readonly string musicVolumeKey = "MusicVolume";
    private int player1Score;
    private int player1Lives;
    private int player2Score;
    private int player2Lives;
    private new AudioListener audio; 
    private List<ScoreData> scores;
    private bool playerOneExists = false;
    private bool playerTwoExists = false;
    private GameObject playerOne;
    private TankData playerOneData;
    private PowerupController playerOnePowerupController;
    private GameObject playerTwo;
    private TankData playerTwoData;
    private PowerupController playerTwoPowerupController;

    
    void Awake()
    {
        if (newGame)
        {
            ResetSettings();
        }

        
        if (instance == null)
        {
            
            instance = this;
        }
        else
        {
            
            Debug.LogError("ERROR: There can only be one GameManager.");
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        if (newGame)
        {
            ResetSettings();
        }

       
        player1Score = 0;
        player2Score = 0;

        audio = GameObject.FindGameObjectWithTag("AudioListener").GetComponent<AudioListener>();

        scores = new List<ScoreData>();

        if (PlayerPrefs.HasKey("NumPlayers"))
        {
            numOfPlayers = PlayerPrefs.GetInt("NumPlayers");
        }
    }
	
	
	void Update ()
    {
        if (!PlayerPrefs.HasKey(musicVolumeKey))
        {
            PlayerPrefs.SetFloat(musicVolumeKey, defaultMusicVolume);
        }
        battleMusic.volume = PlayerPrefs.GetFloat(musicVolumeKey);

        if (newGame)
        {
            ResetSettings();
        }

        
        enemies = GetAllEnemies();

        
        players = GetAllPlayers();

       
        livePlayers = 0;

       
        for (int player = 0; player < players.Count; player++)
        {
            
            if (players[player] != null)
            {
                
                livePlayers++;
            }
        }

       
        switch (numOfPlayers)
        {
            case 1:
                if (livePlayers == 0 && player1Lives == 0)
                {
                    PlayerPrefs.SetInt("PlayerOneScore", player1Score);
                    UpdateScores();
                    DisplayGameOver();
                }
                break;
            case 2:
                if (livePlayers == 0 && player1Lives == 0 && player2Lives == 0)
                {
                    PlayerPrefs.SetInt("PlayerOneScore", player1Score);
                    PlayerPrefs.SetInt("PlayerTwoScore", player2Score);
                    UpdateScores();
                    DisplayGameOver();
                }
                break;
            default:
                break;
        }


        
        liveEnemies = 0;

        
        for (int enemy = 0; enemy < enemies.Count; enemy++)
        {
            
            if (enemies[enemy] != null)
            {
                
                liveEnemies++;
            }
        }

        SpawnEnemies();

        UpdatePlayerExistsStatus();

        if (players.Count < numOfPlayers)
        {
            switch (numOfPlayers)
            {
                case 1:
                    if (!playerOneExists && player1Lives > 0)
                    {
                        SpawnPlayer(1);
                    }
                    break;
                case 2:
                    if (!playerOneExists && player1Lives > 0)
                    {
                        SpawnPlayer(1);
                    }

                    if (!playerTwoExists && player2Lives > 0)
                    {
                        SpawnPlayer(2);
                    }
                    break;
            }
        }

        AudioSource source = audio.GetComponent<AudioSource>();
        if (source != null)
        {
            if (PlayerPrefs.HasKey("Volume"))
            {
                source.volume = PlayerPrefs.GetFloat("Volume");
            }
        }

        SetupCameras();

        foreach (GameObject player in players)
        {
            TankData playerTankData = player.GetComponent<TankData>();
            PowerupController playerPowerupController = player.GetComponent<PowerupController>();

            if (playerTankData != null)
            {
                if (playerTankData.type == TankData.TankType.Player_1)
                {
                    playerOne = player;
                    playerOneData = playerTankData;

                    if (playerPowerupController != null)
                    {
                        playerOnePowerupController = playerPowerupController;
                    }
                }
                else if (playerTankData.type == TankData.TankType.Player_2)
                {
                    playerTwo = player;
                    playerTwoData = playerTankData;

                    if (playerPowerupController != null)
                    {
                        playerTwoPowerupController = playerPowerupController;
                    }
                }
            }
        }

        UpdateUI();
	}

    public List<GameObject> GetAllEnemies()
    {
        List<GameObject> enemyList = new List<GameObject>();
        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        
        foreach(GameObject enemy in enemyArray)
        {
            enemyList.Add(enemy);
        }

        return enemyList;
    }

    public List<GameObject> GetAllPlayers()
    {
        List<GameObject> playerList = new List<GameObject>();
        GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in playerArray)
        {
            playerList.Add(player);
        }

        return playerList;
    }

    public void IncrementPlayerOneScore(int value)
    {
        
        player1Score += value;
    }

    public void IncrementPlayerTwoScore(int value)
    {
        
        player2Score += value;
    }

    public void SpawnEnemies()
    {
        
        foreach(GameObject enemyTank in enemyTanks)
        {
            bool foundTank = false;

            
            foreach(GameObject enemy in enemies)
            {
                if (enemy.GetComponent<AI_Controller>().personality == enemyTank.GetComponent<AI_Controller>().personality)
                {
                    foundTank = true;
                }
            }

            if (!foundTank)
            {
                SpawnEnemyTank(enemyTank);
            }
        }
    }

    public void SpawnEnemyTank(GameObject enemyTank)
    {
        int enemySpawnsIndex;
        
        SpawnData spawnData;
        GameObject spawn;
        GameObject spawnRoom;
        GameObject spawnedEnemy;


        List<GameObject> enemySpawnList = GetEnemySpawnList();

        enemySpawnsIndex = UnityEngine.Random.Range(0, enemySpawnList.Count - 1);
        spawn = enemySpawnList[enemySpawnsIndex];
        spawnData = spawn.GetComponent<SpawnData>();
        if (spawnData != null)
        {
            spawnRoom = spawnData.room;
            enemySpawnList.RemoveAt(enemySpawnsIndex);

            _ = UnityEngine.Random.Range(0, enemyTanks.Count - 1);
            
            spawnedEnemy = Instantiate(enemyTank, spawn.transform.position, Quaternion.identity) as GameObject;

            spawnedEnemy.transform.parent = spawnRoom.transform;
            spawnedEnemy.GetComponent<AI_Controller>().room = spawnRoom;
            spawnedEnemy.transform.position += new Vector3(0, 1, 0);

            enemies.Add(spawnedEnemy);
        }
    }

    public void SpawnPlayer(int playerNum)
    {
        
        List<GameObject> playerSpawnList = GetPlayerSpawnList();

        if (players.Count < numOfPlayers)
        {
            
            if (playerSpawnList.Count == 0)
            {
                playerSpawnList = GetPlayerSpawnList();
            }

            
            int playerSpawnsIndex = UnityEngine.Random.Range(0, playerSpawnList.Count - 1);
            GameObject spawn = playerSpawnList[playerSpawnsIndex];
            playerSpawnList.RemoveAt(playerSpawnsIndex);

            
            int playerTankIndex = -1;
            if (playerNum == 1)
            {
                if (!playerOneExists && player1Lives > 0)
                {
                    player1Lives--;
                    playerTankIndex = 0;
                }
            }
            else if (playerNum == 2)
            {
                if (!playerTwoExists && player2Lives > 0)
                {
                    player2Lives--;
                    playerTankIndex = 1;
                }
            }

            
            if (playerTankIndex > -1)
            {
                
                GameObject playerTank = playerTanks[playerTankIndex];

                
                GameObject spawnedPlayerTank = Instantiate(playerTank, spawn.transform.position, Quaternion.identity) as GameObject;

                
                spawnedPlayerTank.transform.parent = game.transform;
                spawnedPlayerTank.transform.position += new Vector3(0, 1, 0);

                
                players.Add(spawnedPlayerTank);
            }
        }
    }

    private List<GameObject> GetPlayerSpawnList()
    {
        GameObject[] playerSpawns = GameObject.FindGameObjectsWithTag("PlayerSpawn");
        List<GameObject> playerSpawnList = new List<GameObject>();
        foreach (GameObject playerSpawn in playerSpawns)
        {
            playerSpawnList.Add(playerSpawn);
        }

        return playerSpawnList;
    }

    private List<GameObject> GetEnemySpawnList()
    {
        GameObject[] enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
        List<GameObject> enemySpawnList = new List<GameObject>();
        foreach (GameObject enemySpawn in enemySpawns)
        {
            enemySpawnList.Add(enemySpawn);
        }

        return enemySpawnList;
    }

    public void DisplayOptions()
    {
        options.SetActive(true);
        game.SetActive(false);

        GameObject optionsManager = GameObject.FindGameObjectWithTag("OptionsManager");
        optionsManager.GetComponent<OptionsManager>().whoCalledMe = "Game";
    }

    public void UpdateScores()
    {
        
        scores.Clear();

       
        for (int x = 0; x < 10; x++)
        {
            string scoreKey = "High_Score_" + (x + 1).ToString();
            if (PlayerPrefs.HasKey(scoreKey))
            {
                ScoreData newScore = new ScoreData();
                newScore.score = PlayerPrefs.GetInt(scoreKey);
                scores.Add(newScore);
            }
        }

        
        if (numOfPlayers == 1)
        {
            ScoreData player1ScoreData = new ScoreData();
            player1ScoreData.score = player1Score;
            scores.Add(player1ScoreData);
        }
        else if (numOfPlayers == 2)
        {
            ScoreData player1ScoreData = new ScoreData();
            player1ScoreData.score = player1Score;
            scores.Add(player1ScoreData);

            ScoreData player2ScoreData = new ScoreData();
            player2ScoreData.score = player2Score;
            scores.Add(player2ScoreData);
        }

        
        scores.Sort();
         
        
        scores.Reverse();

        if (scores.Count > 10)
        {
            scores = scores.GetRange(0, 10);
        }

        
        for (int x = 0; x < scores.Count; x++)
        {
            PlayerPrefs.SetInt("High_Score_" + (x + 1).ToString(), scores[x].score);
        }
    }

    private void SetupCameras()
    {
        float playerOneX;
        float playerOneY;
        float playerOneWidth;
        float playerOneHeight;
        float playerTwoX;
        float playerTwoY;
        float playerTwoWidth;
        float playerTwoHeight;

        if (numOfPlayers == 1)
        {
            GameObject playerOneCameraObject = GameObject.FindGameObjectWithTag("PlayerOneCamera");
            if (playerOneCameraObject != null)
            {
                Camera playerOneCamera = playerOneCameraObject.GetComponent<Camera>();
                if (playerOneCamera != null)
                {
                    playerOneX = 0;
                    playerOneY = 0;
                    playerOneWidth = 1;
                    playerOneHeight = 1;

                    playerOneCamera.rect = new Rect(playerOneX, playerOneY, playerOneWidth, playerOneHeight);
                }
            }

            GameObject playerTwoCameraObject = GameObject.FindGameObjectWithTag("PlayerTwoCamera");
            if (playerTwoCameraObject != null)
            {
                Camera playerTwoCamera = playerTwoCameraObject.GetComponent<Camera>();
                if (playerTwoCamera != null)
                {
                    playerTwoX = 0;
                    playerTwoY = 0;
                    playerTwoWidth = 1;
                    playerTwoHeight = 1;

                    playerTwoCamera.rect = new Rect(playerTwoX, playerTwoY, playerTwoWidth, playerTwoHeight);
                }
            }
        }
        else if (numOfPlayers == 2)
        {
            GameObject playerOneCameraObject;
            GameObject[] playerOneCameras = GameObject.FindGameObjectsWithTag("PlayerOneCamera");
            if (playerOneCameras.Length > 1)
            {
                playerOneCameraObject = playerOneCameras[playerOneCameras.Length - 1];
            }
            else
            {
                playerOneCameraObject = GameObject.FindGameObjectWithTag("PlayerOneCamera");
            }
             
            if (playerOneCameraObject != null)
            {
                Camera playerOneCamera = playerOneCameraObject.GetComponent<Camera>();
                if (playerOneCamera != null)
                {
                    playerOneX = 0;
                    playerOneY = 0.5f;
                    playerOneWidth = 1;
                    playerOneHeight = 0.5f;

                    playerOneCamera.rect = new Rect(playerOneX, playerOneY, playerOneWidth, playerOneHeight);
                }
            }

            GameObject playerTwoCameraObject = GameObject.FindGameObjectWithTag("PlayerTwoCamera");
            if (playerTwoCameraObject != null)
            {
                Camera playerTwoCamera = playerTwoCameraObject.GetComponent<Camera>();
                if (playerTwoCamera != null)
                {
                    playerTwoX = 0;
                    playerTwoY = 0;
                    playerTwoWidth = 1;
                    playerTwoHeight = 0.5f;

                    playerTwoCamera.rect = new Rect(playerTwoX, playerTwoY, playerTwoWidth, playerTwoHeight);
                }
            }
        }
    }

    private void UpdatePlayerExistsStatus()
    {
        playerOneExists = false;
        playerTwoExists = false;

        for (int x = 0; x < players.Count; x++)
        {
            TankData playerData = players[x].GetComponent<TankData>();
            if (playerData != null)
            {
                if (playerData.type == TankData.TankType.Player_1)
                {
                    playerOneExists = true;
                }
                else if (playerData.type == TankData.TankType.Player_2)
                {
                    playerTwoExists = true;
                }
            }
        }
    }

    private void DisplayGameOver()
    {
        game.SetActive(false);
        options.SetActive(false);
        gameOver.SetActive(true);
    }

    public void ResetSettings()
    {
        player1Lives = playerOneLives + 1;
        player1Score = 0;
        player2Lives = playerTwoLives + 1;
        player2Score = 0;

        if (PlayerPrefs.HasKey("NumPlayers"))
        {
            numOfPlayers = PlayerPrefs.GetInt("NumPlayers");
        }

        playerOneGameOverImage.SetActive(false);
        playerTwoGameOverImage.SetActive(false);

        newGame = false;
    }

    public void UpdateUI()
    {
        string playerOnePowerups = "";
        string playerTwoPowerups = "";

        if (playerOnePowerupController != null)
        {
            foreach(Powerup powerup in playerOnePowerupController.powerups)
            {
                playerOnePowerups += powerup.powerupName + " ";
            }
        }

        if (playerTwoPowerupController != null)
        {
            foreach (Powerup powerup in playerTwoPowerupController.powerups)
            {
                playerTwoPowerups += powerup.powerupName + " ";
            }
        }

        switch (numOfPlayers)
        {
            case 1:
                if (playerOne != null)
                {
                    playerOneScoreText.GetComponent<Text>().text = "Score - " + player1Score.ToString();
                    playerOneHealthText.GetComponent<Text>().text = "Health - " + playerOneData.health + "/" + playerOneData.maxHealth;
                    playerOneLivesText.GetComponent<Text>().text = "Lives - " + player1Lives.ToString();
                    playerOnePowerupText.GetComponent<Text>().text = playerOnePowerups;
                    playerOneOptionsButton.SetActive(true);
                }
                else
                {
                    playerOneScoreText.GetComponent<Text>().text = "";
                    playerOneHealthText.GetComponent<Text>().text = "";
                    playerOneLivesText.GetComponent<Text>().text = "";
                    playerOnePowerupText.GetComponent<Text>().text = "";
                    playerOneOptionsButton.SetActive(false);
                }

                playerTwoScoreText.GetComponent<Text>().text = "";
                playerTwoHealthText.GetComponent<Text>().text = "";
                playerTwoLivesText.GetComponent<Text>().text = "";
                playerTwoPowerupText.GetComponent<Text>().text = "";
                playerTwoOptionsButton.SetActive(false);

                break;
            case 2:
                if (playerOne != null)
                {
                    playerOneScoreText.GetComponent<Text>().text = "Score - " + player1Score.ToString();
                    playerOneHealthText.GetComponent<Text>().text = "Health - " + playerOneData.health + "/" + playerOneData.maxHealth;
                    playerOneLivesText.GetComponent<Text>().text = "Lives - " + player1Lives.ToString();
                    playerOnePowerupText.GetComponent<Text>().text = playerOnePowerups;
                    playerOneOptionsButton.SetActive(true);
                }
                else
                {
                    playerOneScoreText.GetComponent<Text>().text = "";
                    playerOneHealthText.GetComponent<Text>().text = "";
                    playerOneLivesText.GetComponent<Text>().text = "";
                    playerOnePowerupText.GetComponent<Text>().text = "";
                    playerOneOptionsButton.SetActive(false);
                }

                if (playerTwo != null)
                {
                    playerTwoScoreText.GetComponent<Text>().text = "Score - " + player2Score.ToString();
                    playerTwoHealthText.GetComponent<Text>().text = "Health - " + playerTwoData.health + "/" + playerTwoData.maxHealth;
                    playerTwoLivesText.GetComponent<Text>().text = "Lives - " + player2Lives.ToString();
                    playerTwoPowerupText.GetComponent<Text>().text = playerTwoPowerups;
                    playerTwoOptionsButton.SetActive(true);
                }
                else
                {
                    playerTwoScoreText.GetComponent<Text>().text = "";
                    playerTwoHealthText.GetComponent<Text>().text = "";
                    playerTwoLivesText.GetComponent<Text>().text = "";
                    playerTwoPowerupText.GetComponent<Text>().text = "";
                    playerTwoOptionsButton.SetActive(false);
                }
                break;
        }
    }

    public bool CanPlayerRespawn(int playerNum)
    {
        bool result = false;

        switch (playerNum)
        {
            case 1:
                result = (player1Lives > 0);
                break;
            case 2:
                result = (player2Lives > 0);
                break;
        }

        return result;
    }
}