using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    private GameObject heldItem;  // ���� �÷��̾ ��� �ִ� ������

    void Update()
    {
        // �����̽��� �Է��� Ȯ���մϴ�.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (heldItem != null)  // �������� ��� �ִٸ�
            {
                DropItem();
            }
            else  // �������� ��� ���� �ʴٸ�
            {
                TryPickupItem();
            }
        }
    }

    private void TryPickupItem()
    {
        // �÷��̾� �ֺ��� �ִ� �������� Ž���մϴ�.
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

        // �������� �÷��̾��� �ڽ����� �����Ͽ� ��� �ִ� ��ó�� ���̰�
        item.transform.SetParent(transform);
        item.transform.localPosition = new Vector3(0, 0.4f, 0);  // �÷��̾��� ���� ��ġ�ϵ��� ����

        // ������ ��ȣ�ۿ��� ���� ���� �ݶ��̴��� Rigidbody2D ��Ȱ��ȭ
        item.GetComponent<Collider2D>().enabled = false;
    }

    private void DropItem()
    {
        if (heldItem != null)
        {
            // �������� �÷��̾��� �ڽĿ��� �����մϴ�.
            heldItem.transform.SetParent(null);

            // �������� ���� ��ġ�� �����ϴ�.
            heldItem.transform.position = transform.position + new Vector3(0, -0.3f, 0);  // �÷��̾��� �Ʒ��ʿ� ��ġ�ϵ��� ����

            // �ݶ��̴��� Rigidbody2D�� �ٽ� Ȱ��ȭ
            Collider2D collider = heldItem.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = true;
            }

            

            heldItem = null;  // �÷��̾ �� �̻� �������� ��� ���� ����
        }
    }
}
