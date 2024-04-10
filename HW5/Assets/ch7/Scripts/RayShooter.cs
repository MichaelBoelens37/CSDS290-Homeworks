using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShooter : MonoBehaviour
{
    private Camera cam;
    public Transform firePoint; // The point where shots will be fired from

    void Start()
    {
        // Find the main camera in the scene
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Main camera not found in the scene.");
            return;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            // Cast a ray from the mouse cursor position
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Check if the ray hits something
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
                if (target != null)
                {
                    // If the hit object has a ReactiveTarget component, react to the hit
                    target.ReactToHit();
                    Messenger.Broadcast(GameEvent.ENEMY_HIT);
                }
                else
                {
                    // If not, create a sphere indicator at the hit point
                    StartCoroutine(SphereIndicator(hit.point));
                }
            }
        }
    }

    private IEnumerator SphereIndicator(Vector3 pos)
    {
        // Create a sphere at the hit point
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;

        yield return new WaitForSeconds(1);

        // Destroy the sphere after a delay
        Destroy(sphere);
    }
}
