using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    private GameObject heldItem;  // 현재 플레이어가 들고 있는 아이템

    void Update()
    {
        // 스페이스바 입력을 확인합니다.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (heldItem != null)  // 아이템을 들고 있다면
            {
                DropItem();
            }
            else  // 아이템을 들고 있지 않다면
            {
                TryPickupItem();
            }
        }
    }

    private void TryPickupItem()
    {
        // 플레이어 주변에 있는 아이템을 탐색합니다.
        Collider2D[] nearbyItems = Physics2D.OverlapCircleAll(transform.position, 1.0f);

        foreach (Collider2D collider in nearbyItems)
        {
            if (collider.CompareTag("Clue"))
            {
                PickupItem(collider.gameObject);
                break;
            }
        }
    }

    private void PickupItem(GameObject item)
    {
        heldItem = item;

        // 아이템을 플레이어의 자식으로 설정하여 들고 있는 것처럼 보이게
        item.transform.SetParent(transform);
        item.transform.localPosition = new Vector3(0, 0.4f, 0);  // 플레이어의 위에 위치하도록 조정

        // 물리적 상호작용을 막기 위해 콜라이더와 Rigidbody2D 비활성화
        item.GetComponent<Collider2D>().enabled = false;
    }

    private void DropItem()
    {
        if (heldItem != null)
        {
            // 아이템을 플레이어의 자식에서 해제합니다.
            heldItem.transform.SetParent(null);

            // 아이템을 현재 위치에 놓습니다.
            heldItem.transform.position = transform.position + new Vector3(0, -0.3f, 0);  // 플레이어의 아래쪽에 위치하도록 조정

            // 콜라이더와 Rigidbody2D를 다시 활성화
            Collider2D collider = heldItem.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = true;
            }

            

            heldItem = null;  // 플레이어가 더 이상 아이템을 들고 있지 않음
        }
    }
}
