using TMPro;
using UnityEngine;

public class InventoryItemUI : MonoBehaviour
{

    public TextMeshProUGUI _resourcename;
    public TextMeshProUGUI _resourcequantity;


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
        _resourcename.text = name;
    }

    public void SetResourceQuantity(int quantity)
    {
        _resourcequantity.text = quantity.ToString();

    }


    public int GetResourceQuantity() { return int.Parse(_resourcequantity.text); }
   

    public void Onclick()
    {

        PlayerController.instance.ShowAim();
        InventoryManager.instance.SetItemToRemoveFromInventory(_resourcename.text);

        _touchManager.SwitchActionMap(_touchManager.GetInputControls().Throw);
        
    }

}
