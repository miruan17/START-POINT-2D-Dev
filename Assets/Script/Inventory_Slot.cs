using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory_Slot : MonoBehaviour
{
    public Image icon;
    public TMP_Text itemName_Text;
    public TMP_Text itemCount_Text;
    public GameObject selected_Item;

    public void Additem(Item _item)
    {
        itemName_Text.text = _item.itemName;
        icon.sprite = _item.itemIcon;
        if (Item.ItemType.Use == _item.itemType)
        {
            if (_item.itemCount > 0)
            {
                itemCount_Text.text = "x " + _item.itemCount.ToString();
            }
            else
            {
                itemCount_Text.text = "";
            }
        }

        if (Item.ItemType.ETC == _item.itemType)
        {
            if (_item.itemCount > 0)
            {
                itemCount_Text.text = "x " + _item.itemCount.ToString();
            }
            else
            {
                itemCount_Text.text = "";
            }
        }
    }

    public void RemoveItem()
    {
        itemName_Text.text = "";
        itemCount_Text.text = "";
        icon.sprite = null; 
    }
}
