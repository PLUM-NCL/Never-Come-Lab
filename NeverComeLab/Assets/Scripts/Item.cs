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
        icon = GetComponentsInChildren<Image>()[1];  //�ڽ� ������Ʈ�� ������Ʈ�� �ʿ��ϹǷ�
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

                if (data.isSelected == false) // ����off���¿��� ��ư Ŭ���� 
                {
                    GameObject existingWeapon = null;
                    foreach (Transform child in player.transform)   //�����Ǿ� �ִ� ���� ã�� �ٸ� ���� Seleted false��Ű��
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

                    if (existingWeapon == null) //���� �ȵǾ��ִ� ����Ÿ���̶�� �����ϱ� 
                    {
                        GameObject newWeapon = new GameObject();
                        weapon = newWeapon.AddComponent<Weapon>();
                        weapon.Init(data);
                        data.isSelected = true;
                    }
                    else //���� �Ǿ��ִ� ������ Selected�� ���� 
                    {
                        data.isSelected = true;
                    }


                }
                else // ����on���¿��� ��ư Ŭ���� 
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
