using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gasi : MonoBehaviour
{
    [SerializeField] Player player;
    Rigidbody2D playerRigid;
    private void Start()
    {
        //player = FindObjectOfType<Player>();
        playerRigid = player.GetComponent<Rigidbody2D>();
        Debug.Log(playerRigid != null ? "Rigidbody2D found!" : "Rigidbody2D not found!");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(GasiHit());
        }
    }

    IEnumerator GasiHit()
    {
        player.isObstacleHit = true;

        // �÷��̾��� �ӵ��� �ʱ�ȭ
        playerRigid.velocity = Vector2.zero;

        // Gasi�� Player�� ��ġ ���̿� ����� �������� ���� ���Ѵ�
        Vector2 forceDirection = player.transform.position - transform.position;
        Debug.Log($"Force Direction: {forceDirection}");

        // �˹� ���� ����, ũ�⸦ ũ�� ����
        playerRigid.AddForce(2 * new Vector2(-1,0), ForceMode2D.Impulse);

        Debug.Log(playerRigid.velocity);

        yield return new WaitForSeconds(0.5f);
        player.isObstacleHit = false;
    }
}
