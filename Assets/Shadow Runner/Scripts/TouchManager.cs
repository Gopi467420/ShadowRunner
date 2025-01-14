using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.Rendering;

public class TouchManager : MonoBehaviour
{
    private InputControls _inputControl;
    public Camera mainCamera;
    private PlayerController _Player;
    public InventoryManager _inventorymanager;

    public static TouchManager instance;


    private PlayerInput _playerinput;



    private void Start()
    {

        instance = this;
        _Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _inputControl = new InputControls();
        _inputControl.Player.Disable(); // Enable the Player action map by default
        _inputControl.Throw.Enable();

        _playerinput = gameObject.GetComponent<PlayerInput>();



        
        



    }

    private void Update()
    {
        if (_inputControl.Player.enabled)
        {
            PlayerController.instance.MovePlayer(_inputControl.Player.PlayerMovement.ReadValue<Vector2>());
            _inputControl.Player.Collectresource.performed += ctx => OnCollectResource(ctx);

        }
        else if(_inputControl.Throw.enabled) 
        {
            PlayerController.instance.AimProjectile(_inputControl.Throw.ProjectileSetdestination.ReadValue<Vector2>());
            
            

            if (SwipeDetection.instance.CheckSwipeQuadrants(SwipeDetection.Quadrants.Right))
            {
                
                //ThrowProjectile();
            }
           
        }
        
    }


    
    public InputControls GetInputControls()
    {
        return _inputControl;
    }

    // Collect resource on click
    public void OnCollectResource(InputAction.CallbackContext context)
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

    
   
    //public void AimProjectile(InputAction.CallbackContext context) { PlayerController.instance.AimProjectile(context.ReadValue<Vector2>()); }
    public void ThrowProjectile()
    {

       
        if ( SwipeDetection.instance.DetectSwipe() == SwipeDetection.SwipeDirection.Up)
        {
            InventoryManager.instance.RemoveFromInventory();
            SwipeDetection.instance.ResetSwipeParametres();
            SwitchActionMap(_inputControl.Player); 
            PlayerController.instance.HideAim();
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
