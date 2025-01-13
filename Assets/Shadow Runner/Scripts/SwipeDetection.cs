using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeDetection : MonoBehaviour
{
   private TouchManager _touchManager;

    private void Start()
    {

        _touchManager = gameObject.GetComponent<TouchManager>();
        
    }
    private void Update()
    {

        _touchManager.GetInputControls().Throw.PrimaryContact.performed += ctx => StartTouchPrimary(ctx);
        
    }

    private void StartTouchPrimary(InputAction.CallbackContext callbackContext)
    {


    }

}
