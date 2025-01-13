using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    // Movement variables
    public float moveSpeed = 5f;
    private Vector2 moveInput; // Stores the input values for movement
    private Rigidbody _playerrigidbody; // Reference to the Rigidbody component
    private TouchManager _touchManager;

    public Rigidbody _throwdestinationrigidbody; // Destination Rigidbody for throwing
   

    // Player state variables
    public GameObject originalstate; // GameObject for the visible player
    public GameObject hiddenstate;   // GameObject for the hidden player
    private int health;
    private Vector3 _initialposition;
    public enum PlayerState { hidden, visible, sneakattack, powerblow, hit, respawn };
    private PlayerState _currentstate;

    private bool isHidden = false;
    private RectTransform _healthbar;

    void Start()
    {
        // Initialize health and states
        health = 5;
        _initialposition = transform.position;
        originalstate.SetActive(true);
        hiddenstate.SetActive(false);
        _currentstate = PlayerState.visible;

        _healthbar = GameObject.FindWithTag("Healthbar")?.GetComponent<RectTransform>();
        if (_healthbar == null)
        {
            Debug.LogError("Healthbar not found! Ensure the Healthbar is tagged correctly.");
        }

        _playerrigidbody = GetComponent<Rigidbody>();
        if (_playerrigidbody == null)
        {
            Debug.LogError("Player Rigidbody is not assigned!");
        }

        _touchManager = FindAnyObjectByType<TouchManager>();
        if (_touchManager == null)
        {
            Debug.LogError("TouchManager not found in the scene!");
        }

        if (_throwdestinationrigidbody == null)
        {
            Debug.LogError("Throw Destination Rigidbody is not assigned! Please assign it in the Inspector.");
        }

        
    }

    private void Update()
    {
        // Handle player state logic
        switch (_currentstate)
        {
            case PlayerState.hidden:
                Hidden();
                break;
            case PlayerState.visible:
                Visible();
                break;
        }
    }

    private void FixedUpdate()
    {
        if (_touchManager.GetInputControls().Player.enabled)
        {
            MovePlayer();
            // Subscribe to player movement input actions
            _touchManager.GetInputControls().Player.PlayerMovement.performed += OnMovementPerformed;
            _touchManager.GetInputControls().Player.PlayerMovement.canceled += OnMovementCanceled;

            // Unsubscribe from throw movement
            _touchManager.GetInputControls().Throw.ProjectileSetdestination.performed -= OnMovementPerformed;
            _touchManager.GetInputControls().Throw.ProjectileSetdestination.canceled -= OnMovementCanceled;
        }
        else if (_touchManager.GetInputControls().Throw.enabled)
        {
            AimProjectile();
            ThrowProjectile();
            

            // Subscribe to throw movement
            _touchManager.GetInputControls().Throw.ProjectileSetdestination.performed += OnMovementPerformed;
            _touchManager.GetInputControls().Throw.ProjectileSetdestination.canceled += OnMovementCanceled;

            // Unsubscribe from player movement input actions
            _touchManager.GetInputControls().Player.PlayerMovement.performed -= OnMovementPerformed;
            _touchManager.GetInputControls().Player.PlayerMovement.canceled -= OnMovementCanceled;
        }
    }

    private void MovePlayer()
    {
        if (_playerrigidbody == null || _throwdestinationrigidbody == null) return;

        // Movement logic for player
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y).normalized * moveSpeed;
        _playerrigidbody.linearVelocity = new Vector3(movement.x, _playerrigidbody.linearVelocity.y, movement.z);

        // Hide the destination movement
        AimProjectile();
        _throwdestinationrigidbody.gameObject.SetActive(false);
    }

    public void AimProjectile()
    {
        
        // Movement logic
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y).normalized * moveSpeed;
        _throwdestinationrigidbody.linearVelocity = new Vector3(movement.x, _playerrigidbody.linearVelocity.y, movement.z);
        _throwdestinationrigidbody.gameObject.SetActive(true);
        //SwipeDetection.instance.SetCurrentSwipeDirection(SwipeDetection.SwipeDirection.None);

       
    }

    public void ThrowProjectile()
    {
       // if (SwipeDetection.instance.DetectSwipe() == SwipeDetection.SwipeDirection.Up)
        {
            Debug.Log("Throw action triggered!");
            //_touchManager.SwitchActionMap(_touchManager.GetInputControls().Player);
        }
    }

    // Capture movement input
    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        // Get the movement input from the action context
        moveInput = context.ReadValue<Vector2>();
    }

    // Reset movement input when canceled
    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    void Hidden()
    {
        // Switch to hidden state
        originalstate.SetActive(false);
        hiddenstate.SetActive(true);
    }

    void Visible()
    {
        // Switch to visible state
        originalstate.SetActive(true);
        hiddenstate.SetActive(false);
    }

    public void SetCurrentState(PlayerState state)
    {
        _currentstate = state;
    }

    public bool GetIsHidden()
    {
        return isHidden;
    }

    public void SetIsHidden(bool hidden)
    {
        isHidden = hidden;
    }

    public Vector3 GetInitialPosition()
    {
        return _initialposition;
    }

    public int GetHealth()
    {
        return health;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        // Update health bar UI
        if (_healthbar != null)
        {
            _healthbar.localScale = new Vector3(_healthbar.localScale.x - 0.1f, _healthbar.localScale.y, _healthbar.localScale.z);
        }
    }

    public void Respawn(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public PlayerState GetPlayerState()
    {
        return _currentstate;
    }
}
