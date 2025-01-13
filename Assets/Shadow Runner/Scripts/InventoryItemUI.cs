using TMPro;
using UnityEngine;

public class InventoryItemUI : MonoBehaviour
{

    public GameObject _resourcename;
    public GameObject _resourcequantity;


    private TouchManager _touchManager;



    




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        _touchManager = FindAnyObjectByType<TouchManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetResourceName( string name)
    {
        _resourcename.GetComponent<TextMeshProUGUI>().text = name;
    }

    public void SetResourceQuantity(int quantity)
    {
        _resourcequantity.GetComponent<TextMeshProUGUI>().text = quantity.ToString();

    }


    public int GetResourceQuantity() { return int.Parse(_resourcequantity.GetComponent<TextMeshProUGUI>().text); }
   

    public void Onclick()
    {

        _touchManager.SwitchActionMap(_touchManager.GetInputControls().Throw);
        
    }

}
