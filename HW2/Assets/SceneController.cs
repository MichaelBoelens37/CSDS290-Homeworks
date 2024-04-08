using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {
	[SerializeField] private GameObject enemyPrefab;
    private GameObject[] _enemies;
    public int EnemyCount = 1;

    void Start()
    {
        SpawnEnemies(EnemyCount);
    }
    void Update() {
        for (int i = 0; i < _enemies.Length; i++)
        {
            if (_enemies[i] == null)
            {
                SpawnEnemies(2);
                break; 
            }
        }

	}
    void SpawnEnemies(int count)
    {
        _enemies = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            _enemies[i] = Instantiate(enemyPrefab);
            _enemies[i].transform.position = new Vector3(0, 1, 0);
            float angle = Random.Range(0, 360);
            _enemies[i].transform.Rotate(0, angle, 0);
        }
    }

}
