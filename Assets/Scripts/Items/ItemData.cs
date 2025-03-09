using System;
using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable
}

public enum ConsumableType
{
    Health,
    Stamina,
    Speed
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item", order = 1)]
public class ItemData : ScriptableObject
{
   [Header("Info")]
   public string itemName;
   public string description;
   public ItemType type;
   
   [Header("Consumable")]
   public ItemDataConsumable[] consumables;
}
