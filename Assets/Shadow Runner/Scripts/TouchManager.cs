using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class TouchManager : MonoBehaviour
{
    private InputControls _inputControl;
    public Camera mainCamera;
    private GameObject _Player;
    public InventoryManager _inventorymanager;
    private bool isSubscribed = false;

    

    private void Start()
    {
        _Player = GameObject.FindWithTag("Player");
        _inputControl = new InputControls();
        _inputControl.Player.Disable(); // Enable the Player action map by default
        _inputControl.Throw.Enable();
    }

    private void Update()
    {
               
        // Ensure subscriptions for the Collect action
        if (_inputControl.Player.enabled && !isSubscribed)
        {
            _inputControl.Player.Collectresource.performed += OnCollectResource;
            isSubscribed = true;
        }
        else if (!_inputControl.Player.enabled && isSubscribed)
        {
            _inputControl.Player.Collectresource.performed -= OnCollectResource;
            isSubscribed = false;
        }
    }

    public InputControls GetInputControls()
    {
        return _inputControl;
    }

    // Collect resource on click
    private void OnCollectResource(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Vector2 touchPosition = context.ReadValue<Vector2>();

            // Raycast logic
            if (mainCamera != null)
            {
                Ray ray = mainCamera.ScreenPointToRay(touchPosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Gatherable") && Vector3.Distance(hit.collider.gameObject.transform.position, _Player.transform.position) <= 3f)
                    {
                        _inventorymanager.gameObject.SetActive(true);
                        _inventorymanager.AddtoInventory(hit.collider.gameObject);
                    }
                }
                else
                {
                   // Debug.Log("Raycast didn't hit any objects.");
                }
            }
            else
            {
                Debug.LogError("Camera reference is null.");
            }
        }
    }

    private void OnDisable()
    {
        if (isSubscribed)
        {
            _inputControl.Player.Collectresource.performed -= OnCollectResource;
            isSubscribed = false;
        }
    }

    public void SwitchActionMap(InputActionMap actionMap)
    {
       
        if (_inputControl.asset.FindActionMap("Throw") == actionMap)
        {
            _inputControl.Throw.Enable();
            _inputControl.Player.Disable();
        }
        
        else if (_inputControl.asset.FindActionMap("Player") == actionMap)
        {
            _inputControl.Throw.Disable();
            _inputControl.Player.Enable();
        }
    }

   

}
