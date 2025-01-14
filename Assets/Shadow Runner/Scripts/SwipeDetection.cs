using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeDetection : MonoBehaviour
{
    private TouchManager _touchManager;
    private Vector2 _swipestartposition, _swipeendposition, _touchposition;
    private float _swipetime;
    public float _desiredswipedistance;

    public enum SwipeDirection { None, Left, Right, Up, Down };
    private SwipeDirection _currentswipedirection;

    public enum Quadrants { None, Left, Right, Up, Down };
    private Quadrants _currentquadrant;

    public static SwipeDetection instance;

    private float screenWidth;
    private float screenHeight;

    private void Awake()
    {
        // Singleton pattern enforcement
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        _touchManager = gameObject.GetComponent<TouchManager>();

        _swipetime = 0;
        _currentswipedirection = SwipeDirection.None;
        _currentquadrant = Quadrants.None;

        screenWidth = Screen.width;
        screenHeight = Screen.height;


        




    }
    private void Update()
    {

        TouchManager.instance.GetInputControls().Throw.PrimaryContact.performed += ctx => StartTouchPrimary(ctx);
        TouchManager.instance.GetInputControls().Throw.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);


    }

   

    public void StartTouchPrimary(InputAction.CallbackContext callbackContext)
    {
        _touchposition = TouchManager.instance.GetInputControls().Throw.PrimaryPosition.ReadValue<Vector2>();   
        _swipestartposition = _touchposition;
       // Debug.Log("Start " + _touchposition);
       
       
    }

    public void EndTouchPrimary(InputAction.CallbackContext callbackContext)
    {
        _touchposition = TouchManager.instance.GetInputControls().Throw.PrimaryPosition.ReadValue<Vector2>();
        _swipeendposition = _touchposition;
       // Debug.Log("End" + _touchposition);
        DetectSwipe();
        Debug.Log("Swipee" + DetectSwipe());
        


    }

    public Quadrants CheckVectorposition(Vector2 positiontocheck)
    {
        // Check for the Right quadrant
        if (positiontocheck.x > screenWidth / 2 && positiontocheck.x < screenWidth &&
            positiontocheck.y > 0 && positiontocheck.y < screenHeight)
        {
            _currentquadrant = Quadrants.Right;
        }
        // Check for the Left quadrant
        else if (positiontocheck.x > 0 && positiontocheck.x < screenWidth / 2 &&
                 positiontocheck.y > 0 && positiontocheck.y < screenHeight)
        {
            _currentquadrant = Quadrants.Left;
        }
        // Check for the Up quadrant
        else if (positiontocheck.y > 0 && positiontocheck.y < screenHeight / 2 &&
                 positiontocheck.x > 0 && positiontocheck.x < screenWidth)
        {
            _currentquadrant = Quadrants.Up;
        }
        // Check for the Down quadrant
        else if (positiontocheck.y > screenHeight / 2 && positiontocheck.y < screenHeight &&
                 positiontocheck.x > 0 && positiontocheck.x < screenWidth)
        {
            _currentquadrant = Quadrants.Down;
        }

        return _currentquadrant;
    }

    public bool CheckSwipeQuadrants(Quadrants _quadranttocheck)
    {
        if (CheckVectorposition(_swipestartposition) == _quadranttocheck &&
            CheckVectorposition(_swipeendposition) == _quadranttocheck)
        {
            return true;
        }

        return false;
    }

    public void ResetSwipeParametres()
    {
        _touchposition =_swipeendposition = _swipestartposition = Vector2.zero;
        _currentswipedirection = SwipeDirection.None;
        _currentquadrant = Quadrants.None;
        
    }


    // Method to detect swipe direction
    public SwipeDirection DetectSwipe()
    {
        if (Vector2.Distance(_swipestartposition, _swipeendposition) >= _desiredswipedistance)
        {
            // Calculate the normalized direction vector
            Vector2 swipeDirection = (_swipeendposition - _swipestartposition).normalized;

            // Determine the primary direction of the swipe
            if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
            {
                // Horizontal swipe
                if (swipeDirection.x > 0)
                    _currentswipedirection = SwipeDirection.Right;
                else
                    _currentswipedirection = SwipeDirection.Left;
            }
            else
            {
                // Vertical swipe
                if (swipeDirection.y > 0)
                    _currentswipedirection = SwipeDirection.Up;
                else
                    _currentswipedirection = SwipeDirection.Down;
            }

            
        }
        else
        {
            _currentswipedirection = SwipeDirection.None;
        }

        return _currentswipedirection;
        

    }
}
