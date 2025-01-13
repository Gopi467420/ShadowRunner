using UnityEngine;

public class ShadowFollower : MonoBehaviour
{
    
    private PlayerController _player;
    private void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.SetCurrentState(PlayerController.PlayerState.hidden);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.SetCurrentState(PlayerController.PlayerState.visible);
        }
    }

    
}
