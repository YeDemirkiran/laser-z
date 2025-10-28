using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;

    void Update()
    {
        Vector3 pos = transform.position;

        pos.z += speed * Time.deltaTime;

        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }
}
