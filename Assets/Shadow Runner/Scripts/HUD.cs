using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject _healthtext;
    private TextMeshProUGUI _healthvalue;
    private GameManager _gamemanager;



    // Start is called before the first frame update
    void Start()
    {
        _healthvalue =  _healthtext.GetComponent<TextMeshProUGUI>();
        _gamemanager =  GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _healthvalue.text = _gamemanager.GetPlayer().GetHealth().ToString() ;
    }

    public void UpdateHealth(int _newhealth)
    {
        _healthvalue.text = _newhealth.ToString();
    }
} 
