using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;

    Image icon;
    Text textLevel;
    //GameObject newWeapon;


    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];  //�ڽ� ������Ʈ�� ������Ʈ�� �ʿ��ϹǷ�
        icon.sprite = data.ItemIcon;

        Text[] texts = GetComponentsInChildren<Text>();

    }

    //public void OnClick()
    //{
    //    switch (data.itemType)
    //    {
    //        case ItemData.ItemType.Weapon0:
    //            if (level == 0)
    //            {
    //                newWeapon = new GameObject();
    //                newWeapon = GameManager.Instance.pool.Get(data.itemId);
    //                weapon = newWeapon.AddComponent<Weapon>();
    //                weapon.Init(data);
    //                level++;
    //            }
    //            break;
    //        //�ٸ� ��ư interactable = false ���������. 
    //        case ItemData.ItemType.Weapon1:
    //            if (level == 0)
    //            {
    //                newWeapon = new GameObject();
    //                newWeapon = GameManager.Instance.pool.Get(data.itemId);
    //                weapon = newWeapon.AddComponent<Weapon>();
    //                weapon.Init(data);
    //                level++;
    //            }
    //            break;
    //    }
    //}

    //public void OnClick()
    //{
    //    switch (data.itemType)
    //    {
    //        case ItemData.ItemType.Weapon0:
    //            newWeapon = GameManager.Instance.pool.Get(data.itemId);
    //            weapon = newWeapon.AddComponent<Weapon>();
    //            weapon.Init(data);
    //            level++;
    //            break;
    //        //�ٸ� ��ư interactable = false ���������. 
    //        case ItemData.ItemType.Weapon1:
    //            newWeapon = GameManager.Instance.pool.Get(data.itemId);
    //            weapon = newWeapon.AddComponent<Weapon>();
    //            weapon.Init(data);
    //            level++;
    //            break;
    //    }
    //}

    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Weapon0:
                if (level == 0)
                {
                    print("�� ����ҷȰ�?");
                    GameObject newWeapon = new GameObject();
                    //GameObject newWeapon = GameManager.Instance.pool.Get(data.itemId);
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                    level++;
                }
                break;
            case ItemData.ItemType.Weapon1:
                if (level == 0)
                {
                    print("�� ����ҷȰ�? 2");
                    GameObject newWeapon = new GameObject();
                    //GameObject newWeapon = GameManager.Instance.pool.Get(data.itemId);
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                    level++;
                }
                break;
        }
    }
}
