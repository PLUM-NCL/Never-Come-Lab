using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSetting : MonoBehaviour
{
    SpriteRenderer sprite;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sprite.color = new Color(1, 1, 1, 0.5f);
        }
        //else if (collision.CompareTag("Monster")) { 
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sprite.color = new Color(1, 1, 1, 1f);
        }
    }
}
