using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour, Iinteractable
{
    [SerializeField] private SpriteRenderer interactCheckSprite;
    private Transform playerTransform;
  
    private const float INTERACT_DISTANCE = 2f;

    protected bool isTalkable = true;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsInteract())
        {
            Interact();
        }

        if (interactCheckSprite.gameObject.activeSelf && !IsInteract() || !isTalkable)
        {
            interactCheckSprite.gameObject.SetActive(false);
        }

        else if(!interactCheckSprite.gameObject.activeSelf && IsInteract())
        {
            interactCheckSprite.gameObject.SetActive(true);
        }
    }

    public abstract void Interact();

    protected virtual bool IsInteract()
    {
        if (Vector2.Distance(playerTransform.position, transform.position) < INTERACT_DISTANCE && isTalkable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}