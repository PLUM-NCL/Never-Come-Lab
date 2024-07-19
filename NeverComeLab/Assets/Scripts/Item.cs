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
        icon = GetComponentsInChildren<Image>()[1];  //자식 오브젝트의 컴포넌트가 필요하므로
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
    //        //다른 버튼 interactable = false 시켜줘야함. 
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
    //        //다른 버튼 interactable = false 시켜줘야함. 
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
                    print("나 몇번불렸게?");
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
                    print("나 몇번불렸게? 2");
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
