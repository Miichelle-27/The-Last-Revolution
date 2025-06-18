using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.5f; // Velocidad de movimiento
    private Rigidbody2D _rb; // Componente Rigidbody2D
    private PlayerControls _controls; // Input System controls
    private Vector2 _moveInput; // Entrada de movimiento
    private Vector2 _lastMoveInput; // Última dirección de movimiento no nula
    private Animator _animator; // Componente para animaciones

    private void Awake()
    {
        // Inicializa componentes y controles
        _controls = new PlayerControls(); // Inicializa los controles
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _lastMoveInput = Vector2.down; // Dirección inicial (hacia abajo)
        
        if (_rb == null) Debug.LogError("Rigidbody2D no encontrado en Player!");
    }

    private void OnEnable()
    {
        // Activa controles y asigna eventos
        _controls.Player.Enable(); // Activa el mapa Player
        _controls.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>(); // Lee movimiento
        _controls.Player.Move.canceled += ctx => _moveInput = Vector2.zero; // Para al soltar
    }

    private void OnDisable()
    {
        _controls.Player.Disable(); // Desactiva el mapa Player
    }

    private void Update()
    {
        // Almacena la última dirección de movimiento no nula
        if (_moveInput != Vector2.zero)
        {
            _lastMoveInput = _moveInput.normalized;
        }
        
        /*if (_moveInput != Vector2.zero)
        {
            // Prioriza dirección cardinal
            if (Mathf.Abs(_moveInput.x) > Mathf.Abs(_moveInput.y))
                _lastMoveInput = new Vector2(Mathf.Sign(_moveInput.x), 0);
            else
                _lastMoveInput = new Vector2(0, Mathf.Sign(_moveInput.y));
        }*/
        
        // Actualiza parámetros del Animator para Blend Trees
        _animator.SetFloat("MoveX", _lastMoveInput.x);
        _animator.SetFloat("MoveY", _lastMoveInput.y);
        _animator.SetFloat("Speed", _moveInput.magnitude); // 0 si quieto, ~1 si mueve
        
        //Muestra última posición
        Debug.Log($"MoveX={_lastMoveInput.x}, MoveY={_lastMoveInput.y}");
    }

    private void FixedUpdate()
    {
        // Aplica movimiento físico
        if (_rb != null)
        {
            Vector2 targetVelocity = _moveInput.normalized * moveSpeed;
            _rb.velocity = Vector2.Lerp(_rb.velocity, targetVelocity, 0.1f); // Suavizado
            Debug.Log($"Velocity={_rb.velocity}, TargetVelocity={targetVelocity}");
        }
    }

    // Devuelve la dirección de movimiento actual o la última no nula
    public Vector2 GetMoveDirection()
    {
        return _moveInput != Vector2.zero ? _moveInput.normalized : _lastMoveInput;
    }
}