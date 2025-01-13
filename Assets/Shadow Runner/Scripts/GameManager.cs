using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerController _player;
    private EnemyController[] _enemies;
    private GameObject _healthuigameobject;

    private bool _gameepaused;
    private Chekpoint _checkpoint;
    private Vector3 _lastcheckpointpoisition;

    public GameObject _enemyparent; // Reference to the enemy parent
    private GameObject[] _shadows;
    bool doonce;

    // Start is called before the first frame update
    void Start()
    {
        

        _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _enemies = _enemyparent.GetComponentsInChildren<EnemyController>();

        // Find shadows
        _shadows = GameObject.FindGameObjectsWithTag("Shadow");
        _gameepaused = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameepaused)
        {
            // Paused logic can be added here
        }
        else
        {
            // If the player is a shadow, set shadow mode active
            // Update each enemy's state if they are alerted to the player
            foreach (EnemyController _currentEnemy in _enemies)
            {
                if (_player.GetPlayerState() == PlayerController.PlayerState.hidden)
                {
                    SetAllEnemiesState(true,EnemyController.EnemyState.Patrol);

                    _currentEnemy.SetPlayerInZone(false);

                }
               else //visible
                {
                    if (_currentEnemy.GetPlayerInZone())
                    {
                        //start chase
                        foreach (EnemyController _CurrentEnemy in _enemies)
                        {
                            SetAllEnemiesState(true, EnemyController.EnemyState.Chase);
                        }
                    }

                   
                }

               
                
            }
        }
    }

    
    private void SetAllEnemiesState(bool DoOnce, EnemyController.EnemyState _newenemystate)
    {
        // if the previous state was chase then reset bool so that the enemies can go into patrol state
        foreach (EnemyController _CurrentEnemy in _enemies)
        {
            if (_CurrentEnemy.GetCurrentState() == EnemyController.EnemyState.Chase && _newenemystate == EnemyController.EnemyState.Patrol) { doonce = false; }
            if (_CurrentEnemy.GetCurrentState() == EnemyController.EnemyState.Patrol && _newenemystate == EnemyController.EnemyState.Chase) { doonce = false; }
        }

        if (DoOnce)
        {
            if (!doonce)
            {
                foreach (EnemyController _CurrentEnemy in _enemies)
                {
                    _CurrentEnemy.SetCurrentState(_newenemystate);
                }
                doonce = true;
            }

        }
        else
        {
            foreach (EnemyController _CurrentEnemy in _enemies)
            {
                _CurrentEnemy.SetCurrentState(_newenemystate);
            }
            doonce = true;

        }
         
               
    }


    public void OnEnemyAttack()
    {
        
        _player.TakeDamage(10);
    }



    public PlayerController GetPlayer() { return _player; }
    public void SetLastCheckpointposition(Vector3 _checkpoint) { _lastcheckpointpoisition = _checkpoint; }
}
