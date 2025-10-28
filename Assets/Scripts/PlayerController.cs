using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        pos.z += speed * Time.deltaTime;

        transform.position = pos;
    }
}
