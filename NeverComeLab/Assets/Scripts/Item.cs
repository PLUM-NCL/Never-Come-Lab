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
                        else // �ٸ� ������ off ��Ŵ  
                        {
                            child.GetComponent<Weapon>().Itemdata.isSelected = false;
                            foreach (Transform button in weaponbutton.transform)
                            {
                                if(button.name != data.itemType.ToString())
                                {
                                    Button buttonComponent = button.GetComponent<Button>();

                                    ColorBlock colorBlock = buttonComponent.colors; // ���� ���� ��� ��������
                                    colorBlock.normalColor = Color.white;
                                    colorBlock.selectedColor = Color.white;
                                    colorBlock.highlightedColor= Color.white;
                                    button.GetComponent<Button>().colors = colorBlock; // ������ ���� ��� �ٽ� �Ҵ�
                                }
                            }
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
                    ColorBlock thisColorBlock = thisButton.GetComponent<Button>().colors; // ���� ���� ��� ��������
                    thisColorBlock.normalColor = Color.gray;
                    thisColorBlock.selectedColor = Color.gray;
                    thisColorBlock.highlightedColor = Color.gray;
                    thisButton.GetComponent<Button>().colors = thisColorBlock;


                }
                else // ����on���¿��� ��ư Ŭ���� 
                {
                    data.isSelected = false;

                    ColorBlock thisColorBlock = thisButton.GetComponent<Button>().colors; // ���� ���� ��� ��������
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
