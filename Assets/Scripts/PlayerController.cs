using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float steerSpeed = 1f;
    public float health = 100f;
    public InputActionReference moveAction;

    float input = 0f;

    private void OnEnable()
    {
        moveAction.action.Enable();
        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMove;
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        moveAction.action.performed -= OnMove;
        moveAction.action.canceled -= OnMove;
    }

    void Update()
    {
        if (health <= 0f)
            Death();
        else if (health > 100f)
            health = 100f;

        Vector3 pos = transform.position;
        if (input > 1f)
            input = 1f;
        else if (input < -1f)
            input = -1f;
        pos.x += input * steerSpeed * Time.deltaTime;
        transform.position = pos;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<float>();
    }

    void Death()
    {
        Destroy(gameObject);
        GameManager.Instance.ReloadCurrentScene(1f);
    }
}
