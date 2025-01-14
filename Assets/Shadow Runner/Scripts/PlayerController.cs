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

    public static PlayerController instance;

    void Start()
    {

        instance = this;    
        // Initialize health and states
        health = 5;
        _initialposition = transform.position;
        originalstate.SetActive(true);
        hiddenstate.SetActive(false);
        _currentstate = PlayerState.visible;

        _healthbar = GameObject.FindWithTag("Healthbar")?.GetComponent<RectTransform>();
        

        _playerrigidbody = GetComponent<Rigidbody>();
       

        _touchManager = FindAnyObjectByType<TouchManager>();
        

       

        
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

  

    public void MovePlayer(Vector2 _moveinput)
    {
        
        // Movement logic for player
        Vector3 movement = new Vector3(_moveinput.x, 0f, _moveinput.y).normalized * moveSpeed;

        transform.Translate(movement * Time.fixedDeltaTime);
        

        _throwdestinationrigidbody.gameObject.SetActive(false);
    }

    public void AimProjectile(Vector2  _moveinput)
    {
        // Movement logic
        Vector3 movement = new Vector3(_moveinput.x, 0f, _moveinput.y).normalized * moveSpeed;
        _throwdestinationrigidbody.linearVelocity = new Vector3(movement.x, _playerrigidbody.linearVelocity.y, movement.z);
       
    }

   public void ShowAim() { _throwdestinationrigidbody.gameObject.SetActive(true); }

   public void HideAim() { _throwdestinationrigidbody.gameObject.SetActive(false); }
    

   
    private void OnMovementPerformed(InputAction.CallbackContext context) { moveInput = context.ReadValue<Vector2>(); }   
    private void OnMovementCanceled(InputAction.CallbackContext context) { moveInput = Vector2.zero; }
   

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

    public void SetCurrentState(PlayerState state) { _currentstate = state; }
    
    public int GetHealth() { return health; }
    
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
