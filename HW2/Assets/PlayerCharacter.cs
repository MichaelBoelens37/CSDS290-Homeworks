using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour {
	private int _health;
    public Text healthText;
    public Text gameOverText;

    void Start() {
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        _health = 2;
        UpdateHealthText();
    }
    public void Hurt(int damage) {
		_health -= damage;
        UpdateHealthText();
        Debug.Log("Health: " + _health);

        if (_health <= 0)
        {
            GameOver();
        }
    }
    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = _health + ": " + new string('*', _health);
        }
    }

    void GameOver()
    {
        gameOverText.text = "GAME OVER";
    }
}
