using System.Numerics;

public class InputProvider
{
    private InputControls _input = new();


    public Vector2 Movement()
    {
        return _input.Player.PlayerMovement.ReadValue<Vector2>();
    }


}
