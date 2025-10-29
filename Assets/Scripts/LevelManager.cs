using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform enemies;
    public Transform collectables;
    public float speed = 10f;

    void Update()
    {
        foreach (Transform enemy in enemies)
        {
            enemy.Translate(new Vector3(0f, 0f, speed * Time.deltaTime));
        }
        foreach (Transform collectable in collectables)
        {
            collectable.Translate(new Vector3(0f, 0f, speed * Time.deltaTime));
        }
    }
}
