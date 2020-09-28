using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<Player>().OnDeath += OnGameOver;
    }

    public void OnGameOver()
    {
        transform.Find("GameOverPanel").gameObject.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("ShootingGame");
    }
}
