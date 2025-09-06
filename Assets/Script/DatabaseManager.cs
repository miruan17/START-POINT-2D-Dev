using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DatabaseManager : MonoBehaviour
{
    static public DatabaseManager instance;

    private void Awake()
    {
        theHP = FindFirstObjectByType<HPBarController>();

        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches;
    public List<Item> itemList = new List<Item>();

    private HPBarController theHP;

    public void UseItem(int _itemID)
    {
        switch(_itemID)
        {
            case 12001:
                Debug.Log("HP가 10회복되었습니다.");
                theHP.TakeHeal(10);                                
                break;
            case 12002:
                Debug.Log("HP가 30회복되었습니다.");
                theHP.TakeHeal(30);
                break;

        }
    }

    void Start()
    {   
        // 장비형 아이템
        itemList.Add(new Item(10001, "나무 몽둥이", "가볍고 튼튼한 나무 몽둥이", Item.ItemType.Equip));
        itemList.Add(new Item(10002, "돌도끼", "돌로 만든 도끼, 투박하지만 파괴력은 충분하다", Item.ItemType.Equip));
        itemList.Add(new Item(10003, "금동대향로", "이 곳의 물건은 아닌 듯 하다, 화염이 느껴진다", Item.ItemType.Equip));
        itemList.Add(new Item(10004, "창", "날카로운 창이다, 거리를 벌리며 싸우는 데 탁월하다", Item.ItemType.Equip));


        // 재료형 아이템
        itemList.Add(new Item(11001, "목재", "가공된 목재, 다양한 곳에 활용할 수 있어보인다", Item.ItemType.ETC));
        itemList.Add(new Item(11002, "석재", "가공된 석재, 다양한 곳에 활용할 수 있어보인다", Item.ItemType.ETC));
        itemList.Add(new Item(11003, "섬유", "부드러운 섬유, 다양한 곳에 활용할 수 있어보인다", Item.ItemType.ETC));
        itemList.Add(new Item(11004, "철괴", "재련된 철괴, 장비를 더욱 튼튼하게 만들 수 있을 것 같다", Item.ItemType.ETC));

        // 소모형 아이템
        itemList.Add(new Item(12001, "흔한 약초", "어디서나 볼 수 있는 약초 [체력 10 회복]", Item.ItemType.Use));
        itemList.Add(new Item(12002, "꿀", "달디단 꿀이다! 아껴 먹자 [체력 30 회복]", Item.ItemType.Use));
    }
}
