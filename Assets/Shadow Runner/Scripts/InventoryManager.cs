using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject _InventoryItem;
    GameObject _plankresourceui;
    GameObject _stoneresourceui;
    RectTransform _inventoryRectTransform;

    InventoryItemUI _plank;
    InventoryItemUI _stone;

    // Use a List or Dictionary to store items
    private List<InventoryItemUI> Inventory = new List<InventoryItemUI>();

    // Start is called once before the first execution of Update
    void Start()
    {
        // Hide the inventory at the start
        gameObject.SetActive(false);

        // Create UI for Plank
        _plankresourceui = Instantiate(_InventoryItem, transform); // Attach to the parent (e.g., canvas or UI container)
        _plankresourceui.SetActive(false);
        _plankresourceui.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); // Set position
        _plank = _plankresourceui.GetComponent<InventoryItemUI>();
        _plank.SetResourceName("Plank");

        // Create UI for Stone
        _stoneresourceui = Instantiate(_InventoryItem, transform);
        _plankresourceui.SetActive(false);
        _stoneresourceui.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -180); // Offset position
        _stone = _stoneresourceui.GetComponent<InventoryItemUI>();
        _stone.SetResourceName("Stone");

        Inventory.Add(_plank);
        Inventory.Add(_stone);

        _inventoryRectTransform = GetComponent<RectTransform>();

       
    }

    public void AddtoInventory(GameObject _itemtoadd)
    {
        var gatherable = _itemtoadd.GetComponent<Gatherable>();  // Get the Gatherable component

        if (gatherable is GatherablePlank)
        {
            _plank.SetResourceQuantity(_plank.GetResourceQuantity() + 1);
        }
        else if (gatherable is GatherableStone)
        {
            _stone.SetResourceQuantity(_stone.GetResourceQuantity() + 1);
        }
        else
        {
            Debug.Log("Item is neither a Plank nor a Stone.");
        }

        // Check and show inventory if it has any items
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        bool hasItems = false;
        int Noofitems = 0;       

        foreach (InventoryItemUI _resourceitem in Inventory)
        {
            if (_resourceitem.GetResourceQuantity() > 0)
            {
                _resourceitem.gameObject.SetActive(true);
                hasItems = true;
                Noofitems++;
            }
            else { _resourceitem.gameObject.SetActive(false); }
                       
        }

        if (Noofitems > 1) { _inventoryRectTransform.sizeDelta = new Vector2(_inventoryRectTransform.sizeDelta.x, 370); }
        else { _inventoryRectTransform.sizeDelta = new Vector2(_inventoryRectTransform.sizeDelta.x, 190); }
        
        // Set inventory visibility based on whether there are any items
        gameObject.SetActive(hasItems);
    }



}
