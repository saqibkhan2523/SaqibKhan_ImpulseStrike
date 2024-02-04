using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public GameObject cagePrefab;
    public GameObject[] enemyPrefabs;
    public GameObject friendlyPrefab;

    private int maxRangeZX = 80;
    private int minRangeZX = -80;
    private int positionChangeRate = 20;

    private List<GameObject> cages = new List<GameObject>();

    public int totalFriends = 0;
    [SerializeField] TextMeshProUGUI totalFriendsText;

    public bool gamePause = false;
    public bool gameOver = false;
    public bool gameWin = false;
    public bool gameLose = false;

    public List<GameObject> activeFriends = new List<GameObject>();

    private GameObject player;

    public GameObject gameLosePanel;
    public GameObject gameWinPanel;
    public GameObject gamePausePanel;



    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("Player");
        totalFriends = 0;
        SpawnCages();
        SpawnEnemiesAndFriendly();
    }


    // Update is called once per frame
    void Update()
    {
        totalFriendsText.text = "Friendly: " + totalFriends;
        GameWinCondition();

        

        if(Input.GetKeyDown(KeyCode.P))
        {
            //PauseGame.
            PauseGame();
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Quit();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResumeGame();
        }
    }

    //Call from start UnPuase Game
    public void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnLockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void SpawnCages()
    {
        for(int z = minRangeZX; z <= maxRangeZX;  z += positionChangeRate)
        {
            for(int x = minRangeZX; x <= maxRangeZX; x += positionChangeRate)
            {
                if (z == 0 && x == 0)
                {
                    x += positionChangeRate;
                }

                GameObject cage = Instantiate(cagePrefab, new Vector3(x, cagePrefab.transform.position.y, z), cagePrefab.transform.rotation);
                cages.Add(cage);
                
            }
        }
    }

    private void SpawnEnemiesAndFriendly()
    {
        foreach(GameObject cage in cages)
        {
            Transform characterSpawnPoint = cage.GetComponent<CageScript>().characterSpwanPoint;

            int enemyOrFriend = UnityEngine.Random.Range(0, 2);
            if(enemyOrFriend == 0)
            {
                //enemy
                int enemyIndex = UnityEngine.Random.Range(0, 2);
                GameObject obj = Instantiate(enemyPrefabs[enemyIndex], characterSpawnPoint.position, enemyPrefabs[enemyIndex].transform.rotation);
                obj.transform.SetParent(cage.transform);
                cage.gameObject.GetComponent<CageScript>().character = obj;
                
            }
            else
            {
                //friend
                GameObject obj = Instantiate(friendlyPrefab, characterSpawnPoint.position, friendlyPrefab.transform.rotation);
                obj.transform.SetParent(cage.transform);
                cage.gameObject.GetComponent<CageScript>().character = obj;
                totalFriends++;
                
            }
        }
    }

    void GameWinCondition()
    {
        if(totalFriends <= 0)
        {
            gameOver = true;
            gameWin = true;
            gameWinPanel.gameObject.SetActive(true);
            UnLockAndHideCursor();
            Invoke("RestartGame", 3);
        }
    }

    public void GameLoseCondition()
    {
        UnLockAndHideCursor();
        gameOver = true;
        gameLose = true;
        gameLosePanel.gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        UnLockAndHideCursor();
        gamePause = true;
        gamePausePanel.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        LockAndHideCursor();
        gamePause = false;
        gamePausePanel.gameObject.SetActive(false);

    }

    public void RestartGame()
    {
        LockAndHideCursor();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        UnLockAndHideCursor();
        SceneManager.LoadScene("Main");
    }
    public Transform ReturnTargetForEnemy()
    {
        if (activeFriends.Count > 0)
            return activeFriends[0].transform;

        return player.transform;
    }
}
