using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.UI;


public class SceneController : MonoBehaviour {
	[SerializeField] GameObject enemyPrefab;
	private GameObject enemy;
	private List <GameObject> enemies = new List<GameObject> ();
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject winText;
    [SerializeField] TMP_Text scoreLabel;
    int score = 0; 

    void Start() {
        startGameButton.GetComponent<Button>().onClick.AddListener(OnStartGameButtonClick);
        exitButton.GetComponent<Button>().onClick.AddListener(OnExitButtonClick);
        startGameButton.SetActive (false);
        exitButton.SetActive (false);
        winText.SetActive (false);
        for(int i = 0; i < 10; i++)
		{
			GameObject _enemy = Instantiate(enemyPrefab) as GameObject;
			float xP = Random.Range(-10f, 10f);
            float zP = Random.Range(-10f, 10f);
			_enemy.transform.position = new Vector3(xP, 0, zP);
			float angle = Random.Range(0, 360);
			_enemy.transform.Rotate(0, angle, 0);
			enemies.Add( _enemy );

        }
	}

    void Update()
    {
        score = int.Parse(scoreLabel.text);
        if (score >= 10)
        {
            startGameButton.SetActive(true);
            exitButton.SetActive(true);
            winText.SetActive(true);
        }
    }


    public void OnExitButtonClick()
    {
        ExitGame();
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnStartGameButtonClick()
    {
        Restart();
    }
    public void Restart()
    { 
        SceneManager.LoadScene("Scene");
    }
}
