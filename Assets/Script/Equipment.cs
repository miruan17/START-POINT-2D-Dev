using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Equipment : MonoBehaviour
{
    //private OrderManager theOrder;
    //private AudioManager theAudio;
    private PlayerStat thePlayerStat;
    private Inventory theInven;
    private OkOrCancel theOOC;

    /*public string key_sound;
    public string enter_sound;
    public string open_sound;
    public string close_sound;
    public string takeoff_sound;*/

    private const int HELMET = 0, ARMOR = 1, BOOTS = 2, WEAPON = 3, RING = 4;

    public GameObject go;
    public GameObject go_OOC;

    //public Text[] text;
    public Image[] img_slots; // 장비 아이콘들
    public GameObject go_selected_Slot_UI; // 선택된 장비 슬롯 ui

    public Item[] equipItemList; // 장착된 장비 리스트

    private int selectedSlot; // 선택된 장비 슬롯

    public bool activated = false;
    private bool inputKey = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        theInven = FindFirstObjectByType<Inventory>();
        //theOrder = FindFirstObjectByType<OrderManager>();
        //theAudio = FindFirstObjectByType<AudioManager>();
        thePlayerStat = FindFirstObjectByType<PlayerStat>();
        theOOC = FindFirstObjectByType<OkOrCancel>();
    }

    public void EquipItem(Item _item)
    {
        string temp = _item.itemID.ToString();
        temp = temp.Substring(0, 3);
        //100 무기 , 200 투구, 300 갑옷, 400 신발, 500 반지
        switch (temp)
        {
            case "100": // 무기
                EquipItemCheck(WEAPON, _item);
                break;
            case "200": // 투구
                EquipItemCheck(HELMET, _item);
                break;
            case "300": // 갑옷
                EquipItemCheck(ARMOR, _item);
                break;
            case "400": // 신발
                EquipItemCheck(BOOTS, _item);
                break;
            case "500": // 반지
                EquipItemCheck(RING, _item);
                break;
        }
    }

    public void EquipItemCheck(int _count, Item _item)
    {
        if (equipItemList[_count].itemID == 0)
        {
            equipItemList[_count] = _item;
        }else
        {
            theInven.EquipToInventory(equipItemList[_count]);
            equipItemList[_count] = _item;
        }
    }

    public void SelectedSlot()
    {
        go_selected_Slot_UI.transform.position = img_slots[selectedSlot].transform.position;
    }

    public void ClearEquip()
    {
        Color color = img_slots[0].color;
        color.a = 0f;

        for (int i = 0; i < img_slots.Length; i++)
        {
            img_slots[i].sprite = null;
            img_slots[i].color = color;
        }
    }

    public void ShowEquip()
    {
        Color color = img_slots[0].color;
        color.a = 1f;

        for(int i = 0; i < img_slots.Length;i++)
        {
            if(equipItemList[i].itemID != 0)
            {
                img_slots[i].sprite = equipItemList[i].itemIcon;
                img_slots[i].color = color;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(inputKey)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                activated = !activated;

                if(activated)
                {
                    //theOrder.NotMove();
                    //theAudio.Play(open_sound);
                    go.SetActive(true);
                    selectedSlot = 0;
                    SelectedSlot();
                    ClearEquip();
                    ShowEquip();
                }
                else
                {
                    //theOrder.NotMove();
                    //theAudio.Play(close_sound);
                    go.SetActive(false);
                    ClearEquip();
                }

            }
      
            if(activated)
            {
                if(Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (selectedSlot < img_slots.Length - 1)
                        selectedSlot++;
                    else
                        selectedSlot = 0;
                    //theAudio.Play(key_sound);
                    SelectedSlot();
                    
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (selectedSlot < img_slots.Length - 1)
                        selectedSlot++;
                    else
                        selectedSlot = 0;
                    SelectedSlot();
                    //theAudio.Play(key_sound);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (selectedSlot > 0)
                        selectedSlot--;
                    else
                        selectedSlot = img_slots.Length - 1;
                    //theAudio.Play(key_sound);
                    SelectedSlot();
                    
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (selectedSlot > 0)
                        selectedSlot--;
                    else
                        selectedSlot = img_slots.Length - 1;
                    //theAudio.Play(key_sound);
                    SelectedSlot();
                    
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    //theAudio.Play(enter_sound);
                    inputKey = false;
                    StartCoroutine(OOCCoroutine("벗기", "취소"));
                }
            }
        }
    }
    IEnumerator OOCCoroutine(string _up, string _down)
    {
        go_OOC.SetActive(true);
        theOOC.ShowTwoChoice(_up, _down);
        yield return new WaitUntil(() => !theOOC.activated);
        if (theOOC.GetResult())
        {
            theInven.EquipToInventory(equipItemList[selectedSlot]);
            equipItemList[selectedSlot] = new Item(0, "", "", Item.ItemType.Equip);
            //theAudio.Play(takeoff_sound);
            ClearEquip();
            ShowEquip();
        }
        inputKey = true;
        go_OOC.SetActive(false);
    }
}
