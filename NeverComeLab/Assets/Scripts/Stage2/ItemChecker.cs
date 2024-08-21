using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    public bool isOn;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            return;

        if (collision.CompareTag("Clue"))
        {
            isOn = true;
            Debug.Log("Clue = " + isOn);
        }
        else
        {
            isOn = false;
            Debug.Log(isOn);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Clue"))
        {
            isOn = false;
            Debug.Log(isOn);
        }
    }
}
