using UnityEngine;
using UnityEngine.UI;
using System;


public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Player player;

    Image icon;
    Text textLevel;
    //GameObject newWeapon;


    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];  //자식 오브젝트의 컴포넌트가 필요하므로
        icon.sprite = data.ItemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        //player = GameManager.Instance.player;

    }

    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Weapon0:
            case ItemData.ItemType.Weapon1:
                //if (player == null)
                //    print("test1");
                //else if (player.transform == null)
                //    print("test2");

                if (data.isSelected == false) // 무기off상태에서 버튼 클릭시 
                {
                    GameObject existingWeapon = null;
                    foreach (Transform child in player.transform)   //생성되어 있는 무기 찾고 다른 무기 Seleted false시키기
                    {
                        string weaponName = GetWeaponName(data.itemType);
                        if (child.name == weaponName)
                        {
                            existingWeapon = child.gameObject;
                        }
                        else
                        {
                            child.GetComponent<Weapon>().Itemdata.isSelected = false;
                        }
                    }

                    if (existingWeapon == null) //생성 안되어있는 무기타입이라면 생성하기 
                    {
                        GameObject newWeapon = new GameObject();
                        weapon = newWeapon.AddComponent<Weapon>();
                        weapon.Init(data);
                        data.isSelected = true;
                    }
                    else //생성 되어있는 무기라면 Selected만 딸깍 
                    {
                        data.isSelected = true;
                    }


                }
                else // 무기on상태에서 버튼 클릭시 
                {
                    data.isSelected = false;
                    print("test3" + data.isSelected);
                }

                break;

        }
    }
    private string GetWeaponName(ItemData.ItemType itemType)
    {
        switch (itemType)
        {
            case ItemData.ItemType.Weapon0:
                return "Weapon 0";
            case ItemData.ItemType.Weapon1:
                return "Weapon 1";
            default:
                return itemType.ToString();

        }
    }
}
