using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour, Iinteractable
{
    [SerializeField] private SpriteRenderer interactCheckSprite;
    private Player player;
  
    private const float INTERACT_DISTANCE = 2f;

    protected bool isTalkable = true;
    void Start()
    {
        player = FindObjectOfType<Player>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsInteract())
        {
            
            Interact();
            Player.isStop = true;
        }

        if (interactCheckSprite.gameObject.activeSelf && !IsInteract() || !isTalkable)
        {
            interactCheckSprite.gameObject.SetActive(false);
            Player.isStop = false;
        }

        else if(!interactCheckSprite.gameObject.activeSelf && IsInteract())
        {
            interactCheckSprite.gameObject.SetActive(true);
            
        }
    }

    public abstract void Interact();

    protected bool IsInteract()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < INTERACT_DISTANCE && isTalkable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
