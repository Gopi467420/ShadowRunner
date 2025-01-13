using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chekpoint : MonoBehaviour
{
    private Vector3 _lastcheckpointpoisition;
    public string _checkpoint;
    private GameManager _gamemanager;
    // Start is called before the first frame update
    void Start()
    {
        _checkpoint = name;
       
        _gamemanager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        { 
            _gamemanager.SetLastCheckpointposition(transform.position);
        }
        
    }

    public Vector3 GetlastCheckpoint() { return _lastcheckpointpoisition; }
}
