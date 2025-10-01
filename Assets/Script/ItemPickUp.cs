using UnityEngine;

// 임시 아이템 획득 스크립트

public class ItemPickUp : MonoBehaviour
{   
    
    public int itemID;
    public int _count;
    //public string pickUpSound;

    
    void OnMouseDown() // 오브젝트 클릭 시 자동 호출
    {
        if (Inventory.instance != null && Inventory.instance.IsActivated())
            return;
        //AudioManager.instance.Play(pickUpSound);
        Inventory.instance.GetAnItem(itemID, _count);
        Destroy(gameObject);
    }
    
        
    
}
