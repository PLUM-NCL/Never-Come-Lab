using UnityEngine;
using UnityEngine.UI;
using System;


public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Player player;
    public Button thisButton;
    public GameObject weaponbutton;

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
                        else // 다른 무기라면 off 시킴  
                        {
                            child.GetComponent<Weapon>().Itemdata.isSelected = false;
                            foreach (Transform button in weaponbutton.transform)
                            {
                                if(button.name != data.itemType.ToString())
                                {
                                    Button buttonComponent = button.GetComponent<Button>();

                                    ColorBlock colorBlock = buttonComponent.colors; // 현재 색상 블록 가져오기
                                    colorBlock.normalColor = Color.white;
                                    colorBlock.selectedColor = Color.white;
                                    colorBlock.highlightedColor= Color.white;
                                    button.GetComponent<Button>().colors = colorBlock; // 수정된 색상 블록 다시 할당
                                }
                            }
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
                    ColorBlock thisColorBlock = thisButton.GetComponent<Button>().colors; // 현재 색상 블록 가져오기
                    thisColorBlock.normalColor = Color.gray;
                    thisColorBlock.selectedColor = Color.gray;
                    thisColorBlock.highlightedColor = Color.gray;
                    thisButton.GetComponent<Button>().colors = thisColorBlock;


                }
                else // 무기on상태에서 버튼 클릭시 
                {
                    data.isSelected = false;

                    ColorBlock thisColorBlock = thisButton.GetComponent<Button>().colors; // 현재 색상 블록 가져오기
                    thisColorBlock.normalColor = Color.white;
                    thisColorBlock.selectedColor = Color.white;
                    thisColorBlock.highlightedColor = Color.white;
                    thisButton.GetComponent<Button>().colors = thisColorBlock;
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
