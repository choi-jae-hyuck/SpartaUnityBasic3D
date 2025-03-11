using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public Interaction interaction;
    
    public ItemData itemData;
    public Action addItem;
    
    

    private void Awake()
    {
        CharacterManager.Instance.Player = this;  
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        interaction = GetComponent<Interaction>();
    }
}