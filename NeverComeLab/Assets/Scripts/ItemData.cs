using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Weapon0, Weapon1};

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    public Sprite ItemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public bool isSelected = false;

    [Header("# Weapon")]
    public GameObject projectile;

}
