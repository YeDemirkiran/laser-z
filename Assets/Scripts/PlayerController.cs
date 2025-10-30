using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Health & Death")]
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
    public UnityEvent OnDeath;

    [Header("Movement")]
    [SerializeField] InputActionReference moveAction;
    [SerializeField] float steerSpeed = 1f;
    float input = 0f;

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
        Move();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<float>();
    }

    void Move()
    {
        float horizontal = Mathf.Clamp(input, -1f, 1f) * steerSpeed * Time.deltaTime;
        transform.Translate(Vector3.right * horizontal);
    }
    public void AddHealth(float health)
    {
        Health += health;
    }

    void Die()
    {
        OnDeath?.Invoke();
    }
}
