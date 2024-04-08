using UnityEngine;
using System.Collections;
using UnityEditor;

public class ReactiveTarget : MonoBehaviour {
    [SerializeField] private GameObject tombstonePrefab;
    public void ReactToHit() {
		WanderingAI behavior = GetComponent<WanderingAI>();
		if (behavior != null) {
			behavior.SetAlive(false);
		}
		StartCoroutine(Die());
	}

	private IEnumerator Die() {
        Vector3 enemyPosition = transform.position;
        for(int i = 0;  i < 90; i++) {
            this.transform.Rotate(-1, 0, 0);
            yield return new WaitForSeconds(1.0f * Time.deltaTime);
        }

		yield return new WaitForSeconds(0.5f);
        
		Destroy(this.gameObject);

        InstantiateTombstone(enemyPosition);
    }

    void InstantiateTombstone(Vector3 position)
    {
        position.y = -2.5f;
        // Instantiate tombstone at the hit location
        GameObject Tombstone = Instantiate(tombstonePrefab, position, Quaternion.identity);
        // Make the tombstone slowly rotate
        Tombstone.AddComponent<RotateForever>();
    }
}
