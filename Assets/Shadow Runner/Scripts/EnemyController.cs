using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.LowLevelPhysics;
using UnityEngine.Rendering;


public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]    
    public float _patrolradius;
    public float _attackradius;

    [Header("Idle State Settings")]
    public int _idlestatedelaytime;

    private NavMeshAgent _NavMeshAgent;
    private bool _attackplayer;
    bool findnewdestination;

    public float _rotationspeed;
    private bool _playerinzone;


    private bool _canattack;
    private bool _isAttacking = false;


    public enum EnemyState { Idle, Patrol, Chase, Attack };
    public EnemyState _currentenemystate;

    private PlayerController _player;
    private GameManager _gamemanager;
    private Vector3 _initialposition;
    private Vector3 _patrolTarget;

    private bool _movingTowardsTarget = true;

    private int _idledelay;

    public GameObject _TEMPTEXT;

    private bool _doonce;

    private int _countdown;

    

    // Start is called before the first frame update
    void Start()
    {
        _initialposition = transform.position; // Store the initial position
        
        

        _NavMeshAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _gamemanager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        _currentenemystate = EnemyState.Idle; 

        // Ensure the NavMeshAgent is enabled initially
        _NavMeshAgent.enabled = true;
         findnewdestination = true;
        _rotationspeed = 5.0f;
        _idledelay = _idlestatedelaytime = 0;
        _idlestatedelaytime = 700;

        _doonce = true;

        _countdown = 1;
        


        _TEMPTEXT.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        switch (_currentenemystate)
        {
            case EnemyState.Idle:
                Idle();
                break; 
           
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Attack:
                Attack(); 
                break;
        } 

        //Debug.Log("Enemy state" +  _currentenemystate);

    }
    private void Idle() 
    {
        
        _idledelay++;        
        if (_idledelay % _idlestatedelaytime == 0)
        {
            _currentenemystate = EnemyState.Patrol;
            findnewdestination = true;
           
        }

    }
    private void Patrol() 
    {
        // Reset idle delay
        _idledelay = 0;

        if (findnewdestination)
        {
            // Generate a new random destination within the patrol radius
            Vector3 randomPoint = _initialposition + Random.insideUnitSphere * _patrolradius;
            Vector3 finalDestination = new Vector3(randomPoint.x, _initialposition.y, randomPoint.z);

            // Store the destination
            _NavMeshAgent.SetDestination(finalDestination);
            findnewdestination = false; // Prevent continuous new destination calculation
        }

        // Calculate the direction to the current destination
        Vector3 directionToTarget = (_NavMeshAgent.destination - transform.position).normalized;

        // Rotate towards the destination
        if (directionToTarget != Vector3.zero) // Avoid NaN errors
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationspeed);
        }
             
        // If the agent is very close to the destination, switch to Idle state
        if (!_NavMeshAgent.pathPending && _NavMeshAgent.remainingDistance <= _NavMeshAgent.stoppingDistance) 
        {
            _currentenemystate = EnemyState.Idle;
            findnewdestination = true; // Allow finding a new destination when the agent stops
        }
    }
    private void Chase()
    {

        if(_canattack) { _currentenemystate = EnemyState.Attack; }
        _NavMeshAgent.SetDestination(_player.transform.position);
       

       
    }
    private void Attack()
    {
        if (!_isAttacking && _canattack)
        {
            StartCoroutine(PerformAttack());
        }
        else if (!_canattack)
        {
            StopAllCoroutines(); // Stops attack if the enemy can't attack
            _TEMPTEXT.SetActive(false);
            _currentenemystate = EnemyState.Chase;
            _isAttacking = false;
        }
    }

    private IEnumerator PerformAttack()
    {
        _isAttacking = true; // Prevents multiple coroutines from running at the same time

        while (_canattack) // Continues attacking as long as the player is in the trigger zone
        {
            _TEMPTEXT.SetActive(true);

            // Play animation or perform attack logic
            Debug.Log("Attack");
            _gamemanager.OnEnemyAttack();

            yield return new WaitForSeconds(1f); // Wait for 1 seconds between attacks

            _TEMPTEXT.SetActive(false); // Hide attack message (if needed)
        }

        _isAttacking = false; // Reset attack flag when exiting the loop
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" )
        {

            _playerinzone = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if ((other.tag == "Player"))
        {
            _canattack = true;
            Debug.Log("inside");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Player"))
        {
            _canattack = false;
            
        }
       
    }

    public bool GetPlayerInZone() {  return _playerinzone; }
    public bool GetHitplayer() { return _attackplayer; }
    public bool GetCanAttack() { return _canattack; }
    public EnemyState GetCurrentState() { return _currentenemystate; }
    public void SetPlayerInZone(bool alert) { _playerinzone = alert; }
    public void SetHitplayer(bool hitplayer) { _attackplayer = hitplayer; }
    public void SetCurrentState(EnemyState newstate) { _currentenemystate = newstate; }
    
    
    
}
