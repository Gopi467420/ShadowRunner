using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class YouWin : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Text;
    public GameObject _playagainbutton;
    public GameObject _endagainbutton;
    void Start()
    {
        _playagainbutton.SetActive(false);
        _endagainbutton.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            
        {
            _playagainbutton.SetActive(true);
            _endagainbutton.SetActive(true);
            Text.GetComponent<TextMeshProUGUI>().text = "YouWin";
        }
        
    }
}
