using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float health = 100f;
    public float Health 
    { 
        get 
        { 
            return health; 
        } 

        private set
        {
            value = Mathf.Clamp(value, 0f, 100f);
            health = value;
            if (health <= 0f)
                Die();
        }
    }

    public InputActionReference moveAction;

    public float steerSpeed = 1f;
    float input = 0f;

    public UnityEvent OnDeath;

    void OnEnable()
    {
        moveAction.action.Enable();
        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMove;
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        moveAction.action.performed -= OnMove;
        moveAction.action.canceled -= OnMove;
    }

    void Update()
    {
        if (health <= 0f)
            Die();
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

    public void AddHealth(float health)
    {
        Health += health;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<float>();
    }

    void Die()
    {
        OnDeath?.Invoke();
    }
}
