using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //To set the number of enemy and players and in the game which would increase as player completes each level.
        PlayerPrefs.SetInt("enemy", 2);
        PlayerPrefs.SetInt("friends", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GamePlay()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
