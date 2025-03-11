using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;
    
    public GameObject curInteractGameObject;
    private IInteractable curInteractable;
    
    public TextMeshProUGUI promptText;
    public Camera[] TPFPcameras;
    public Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.gameObject.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            OnUseItem();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
    
    public void OnUseItem()
    {
        ItemData itemData = CharacterManager.Instance.Player.itemData;
        
        if (itemData.type == ItemType.Consumable)
        {
            for (int i = 0; i < itemData.consumables.Length; i++)
            {
                switch (itemData.consumables[i].type)
                {
                    case ConsumableType.Health:
                        CharacterManager.Instance.Player.condition.Heal(itemData.consumables[i].value); break;
                    case ConsumableType.Stamina:
                        CharacterManager.Instance.Player.condition.HealStamina(itemData.consumables[i].value); break;
                    case ConsumableType.Speed:
                        CharacterManager.Instance.Player.controller.SpeedUp();
                        break;
                }
            }
            
        }
    }

    public void changeCam()
    {
        if (!CharacterManager.Instance.Player.controller.isFP)
        {
            maxCheckDistance = 3f;
            camera = Camera.main;
        }
        else if(CharacterManager.Instance.Player.controller.isFP)
        {
            maxCheckDistance = 20f;
            camera = TPFPcameras[1];
        }
    }

}
