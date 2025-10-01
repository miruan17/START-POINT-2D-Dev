using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private DatabaseManager theDatabase;
    //private OrderManager theOrder;
    private AudioManager theAudio;
    private OkOrCancel theOOC;
    private Equipment theEquip;

    public string key_sound;
    public string enter_sound;
    public string cancel_sound;
    public string open_sound;
    public string beep_sound;

    private Inventory_Slot[] slots; // 인벤토리 슬롯들

    private List<Item> inventoryItemList; // 플레이어가 소지하고 있는 아이템 리스트
    private List<Item> inventoryTabList; // 선택한 탭에 따라 다르게 보여질 아이템 리스트

    public TMP_Text Description_Text; // 부연 설명
    public string[] tabDescription; // 탭 부연 설명

    public Transform tf; // slot의 부모 객체

    public GameObject go; // 인벤토리 활성화 비활성화
    public GameObject[] selectedTabImages; // 
    public GameObject go_OOC; // 선택지 활성화 비활성화

    private int selectedItem; // 선택된 아이템.
    private int selectedTab; // 선택된 탭

    private bool activated; // 인벤토리 활성화시 true;
    private bool tabActivated; // 탭 활성화시 true;
    private bool itemActivated; // 아이템 활성화시 true;
    private bool stopKeyInput; // 키입력 제한 (질의 시, 키 입력 방지) 
    private bool preventExec; // 중복실행 제한;



    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    void Start()
    {
        instance = this;
        //theAudio = FindFirstObjectByType<AudioManager>();
        //theOrder = FindFirstObjectByType<OrderManager>();
        theDatabase = FindFirstObjectByType<DatabaseManager>();
        theOOC = FindFirstObjectByType<OkOrCancel>();
        theEquip = FindFirstObjectByType<Equipment>();

        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        slots = tf.GetComponentsInChildren<Inventory_Slot>();

        // 임시 아이템 획득 코드
        // 장비형 아이템
        inventoryItemList.Add(new Item(10001, "나무 몽둥이", "가볍고 튼튼한 나무 몽둥이", Item.ItemType.Equip));
        inventoryItemList.Add(new Item(10002, "돌도끼", "돌로 만든 도끼, 투박하지만 파괴력은 충분하다", Item.ItemType.Equip));
        inventoryItemList.Add(new Item(10003, "금동대향로", "이 곳의 물건은 아닌 듯 하다, 화염이 느껴진다", Item.ItemType.Equip));
        inventoryItemList.Add(new Item(10004, "창", "날카로운 창이다, 거리를 벌리며 싸우는 데 탁월하다", Item.ItemType.Equip));


        // 재료형 아이템
        inventoryItemList.Add(new Item(11001, "목재", "가공된 목재, 다양한 곳에 활용할 수 있어보인다", Item.ItemType.ETC));
        inventoryItemList.Add(new Item(11002, "석재", "가공된 석재, 다양한 곳에 활용할 수 있어보인다", Item.ItemType.ETC));
        inventoryItemList.Add(new Item(11003, "섬유", "부드러운 섬유, 다양한 곳에 활용할 수 있어보인다", Item.ItemType.ETC));
        inventoryItemList.Add(new Item(11004, "철괴", "재련된 철괴, 장비를 더욱 튼튼하게 만들 수 있을 것 같다", Item.ItemType.ETC));

        // 소모형 아이템
        inventoryItemList.Add(new Item(12001, "흔한 약초", "어디서나 볼 수 있는 약초 [체력 10 회복]", Item.ItemType.Use, 15));
        inventoryItemList.Add(new Item(12002, "꿀", "달디단 꿀이다! 아껴 먹자 [체력 30 회복]", Item.ItemType.Use, 16));

    }
    public void EquipToInventory(Item _item)
    {
        inventoryItemList.Add(_item);
    }
    public bool IsActivated() // 인벤토리 활성화 여부
    {
        return activated;
    }
    public void GetAnItem(int _itemID, int _count = 1) // 아이템을 얻는 함수
    {
        for (int i = 0; i < theDatabase.itemList.Count; i++) // 데이터 베이스 아이템 검색
        {
            if (_itemID == theDatabase.itemList[i].itemID) // 데이터 베이스에 아이템 발견
            {
                for (int j = 0; j < inventoryItemList.Count; j++) // 소지품에 같은 아이템 있는지 확인
                {
                    if (inventoryItemList[j].itemID == _itemID) // 있다 -> 개수만 증감
                    {
                        if (inventoryItemList[j].itemType == Item.ItemType.Use || inventoryItemList[j].itemType == Item.ItemType.ETC)
                        {
                            inventoryItemList[j].itemCount += _count;
                        }
                        else
                        {
                            inventoryItemList.Add(theDatabase.itemList[i]);
                        }
                        return;

                    }
                }
                inventoryItemList.Add(theDatabase.itemList[i]); // 소지품에 해당 아이템 추가
                inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;
                return;
            }
        }
        Debug.LogError("데이터베이스에 해당 ID값을 가진 아이템 존재x"); // 데이터베이스에 아이템 아이디 없음
    }

    public void RemoveSlot()// 인벤토리 슬롯 초기화
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    }
    public void ShowTab()// 탭활성화
    {
        RemoveSlot();
        SelectedTab();
    }

    public void SelectedTab()// 선택된 탭을 제외하고 다른 모든 탭의 컬러 알파값  0으로 조정
    {
        StopAllCoroutines();
        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
        color.a = 0f;
        for (int i = 0; i < selectedTabImages.Length; i++)
        {
            selectedTabImages[i].GetComponent<Image>().color = color;
        }
        Description_Text.text = tabDescription[selectedTab];
        StartCoroutine(SelectedTabEffectCoroutine());
    }
    IEnumerator SelectedTabEffectCoroutine() // 선택된 탭 반짝이 효과
    {
        while (tabActivated)
        {
            Color color = selectedTabImages[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a < 0f)
            {
                color.a -= 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            yield return new WaitForSeconds(0.3f);

        }
    }

    public void ShowItem() // 아이템 활성화 (inventoryTabList에 조건에 맞는  아이템들만 넣어주고, 인벤토리 슬롯에 출력)
    {
        inventoryTabList.Clear();
        RemoveSlot();
        selectedItem = 0;

        switch (selectedTab)//탭에 따른 분류, 그것을 인벤토리 탭 리스트에 추가
        {
            case 0:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Use == inventoryItemList[i].itemType)
                    {
                        inventoryTabList.Add(inventoryItemList[i]);
                    }
                }
                break;

            case 1:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Equip == inventoryItemList[i].itemType)
                    {
                        inventoryTabList.Add(inventoryItemList[i]);
                    }
                }
                break;

            case 2:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.ETC == inventoryItemList[i].itemType)
                    {
                        inventoryTabList.Add(inventoryItemList[i]);
                    }
                }
                break;

            case 3:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Quest == inventoryItemList[i].itemType)
                    {
                        inventoryTabList.Add(inventoryItemList[i]);
                    }
                }
                break;
        }

        for (int i = 0; i < inventoryTabList.Count; i++) // 인벤토리 탭 리스트에 내용을, 인벤토리 술롯에 추가
        {
            slots[i].gameObject.SetActive(true);
            slots[i].Additem(inventoryTabList[i]);
        }

        SelectedItem();
    }
    public void SelectedItem()// 선택된 아이템을 제외하고, 다른 모든 탭의 컬러 알파값을 0으로 조정

    {
        Debug.Log("SelectedItem 실행");
        StopAllCoroutines();
        if (inventoryTabList.Count > 0)
        {
            Color color = slots[0].selected_Item.GetComponent<Image>().color;
            color.a = 0f;
            for (int i = 0; i < inventoryTabList.Count; i++)
            {
                slots[i].selected_Item.GetComponent<Image>().color = color;
            }

            Description_Text.text = inventoryTabList[selectedItem].itemDescription;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else
        {
            Description_Text.text = "해당 타입의 아이템을 소유하고 있지 않습니다.";
        }

    }

    IEnumerator SelectedItemEffectCoroutine()// 선택된 아이템 반짝이 효과
    {
        Debug.Log("SelectedItemEffectCoroutine 실행");
        while (itemActivated)
        {
            Color color = slots[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            yield return new WaitForSeconds(0.3f);

        }
    }
    void Update()
    {
        if (!stopKeyInput)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                activated = !activated;

                if (activated)
                {
                    //theAudio.Play(open_sound);
                    //theOrder.NotMove() 나중에 코어플레이와 연결할 때, 인벤토리창을 열면 플레이어가 못움직이도록 해야함
                    go.SetActive(true);
                    selectedTab = 0;
                    tabActivated = true;
                    itemActivated = false;
                    ShowTab();

                }
                else
                {
                    //theAudio.Play(cancel_sound);
                    StopAllCoroutines();
                    go.SetActive(false);
                    tabActivated = false;
                    itemActivated = false;
                    //theOrder.Move();
                }
            }

            if (activated)
            {
                if (tabActivated)// 탭 활성화시 키 입력 처리
                {

                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selectedTab < selectedTabImages.Length - 1)
                        {
                            selectedTab++;
                        }
                        else
                        {
                            selectedTab = 0;
                        }
                        //theAudio.Play(key_sound);
                        SelectedTab();

                    }

                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (selectedTab > 0)
                        {
                            selectedTab--;
                        }
                        else
                        {
                            selectedTab = selectedTabImages.Length - 1;
                        }
                        //theAudio.Play(key_sound);
                        SelectedTab();

                    }

                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        //theAudio.Play(enter_sound);
                        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
                        color.a = 0.25f;
                        selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                        itemActivated = true;
                        tabActivated = false;
                        preventExec = true;
                        ShowItem();

                    }


                }

                else if (itemActivated)// 아이템 활성화시 키 입력 처리
                {
                    if (inventoryTabList.Count > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            if (selectedItem < inventoryTabList.Count - 2)
                            {
                                selectedItem += 2;
                            }
                            else
                            {
                                selectedItem %= 2;
                            }
                            //theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            if (selectedItem > 1)
                            {
                                selectedItem -= 2;
                            }
                            else
                            {
                                selectedItem = inventoryTabList.Count - 1 - selectedItem;
                            }
                            //theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if (selectedItem < inventoryTabList.Count - 1)
                            {
                                selectedItem++;
                            }
                            else
                            {
                                selectedItem = 0;

                            }
                            //theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if (selectedItem > 0)
                            {
                                selectedItem--;
                            }
                            else
                            {
                                selectedItem = inventoryTabList.Count - 1;

                            }
                            //theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.Z) && !preventExec)
                        {
                            if (selectedTab == 0) // 소모품
                            {                               
                                StartCoroutine(OOCCoroutine("사용", "취소")); 
                            }
                            else if (selectedTab == 1) // 장비 
                            {
                                StartCoroutine(OOCCoroutine("장착", "취소"));
                            }
                            else
                            {
                                //theAudio.Play(beep_sound);
                            }
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        Debug.Log("x키 감지됨");
                        //theAudio.Play(cancel_sound);
                        StopAllCoroutines();
                        itemActivated = false;
                        tabActivated = true;
                        ShowTab();
                    }
                }

                if (Input.GetKeyUp(KeyCode.Z))// 중복 실행 방지
                    preventExec = false;
            }
        }
    }


    IEnumerator OOCCoroutine(string _up, string _down)
    {
        go_OOC.SetActive(true); 
        theOOC.ShowTwoChoice(_up, _down);
        yield return new WaitUntil(() => !theOOC.activated);
        if (theOOC.GetResult())
        {/*
            for(int i = 0; i < inventoryItemList.Count; i++)
            {
                theDatabase.UseItem(inventoryItemList[i].itemID); 

                if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
                {
                    if (inventoryItemList[i].itemCount > 1)
                        inventoryItemList[i].itemCount--;
                    else
                        inventoryItemList.RemoveAt(i);

                    //theAudio.PlaySound() // 아이템 먹는 소리
                    ShowItem();
                    break;
                }
            }
            */

            int selectedID = inventoryTabList[selectedItem].itemID;
            theDatabase.UseItem(selectedID);

            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                if (inventoryItemList[i].itemID == selectedID)
                {
                    if(selectedTab == 0)
                    {
                        if (inventoryItemList[i].itemCount > 1)
                            inventoryItemList[i].itemCount--;
                        else
                            inventoryItemList.RemoveAt(i);

                        ShowItem();
                        break;
                    }else if(selectedTab == 1)
                    {
                        theEquip.EquipItem(inventoryItemList[i]);
                        inventoryItemList.RemoveAt(i);
                        ShowItem();
                        break;
                    }
                    
                }
            }
        }

        stopKeyInput = false;
        go_OOC.SetActive(false);
    }
}
 