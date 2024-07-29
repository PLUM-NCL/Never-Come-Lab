using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Windows;

[RequireComponent(typeof(PolygonCollider2D))]
public class ArcMonsterSensor : MonoBehaviour
{
    private Monster monster;

    public float radius = 2f; // ȣ�� ������
    public float angle = 45f; // ȣ�� ���� (����: ��)
    public int segments = 100; // ȣ�� �����ϴ� ���׸�Ʈ ��

    void Start()
    {
        CreateArcCollider();
        monster = GetComponentInParent<Monster>();
    }

    private void Update()
    {
        Vector3 velocity = monster.agent.velocity;

        if (velocity.x > 0f && Mathf.Abs(velocity.y) < Mathf.Abs(velocity.x)) // right �̵�
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (velocity.x < 0f && Mathf.Abs(velocity.y) < Mathf.Abs(velocity.x)) // left �̵�
        {
            transform.rotation = Quaternion.Euler(0, 0, 180f);
        }
        else if (velocity.y > 0f && Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x)) // back �̵�
        {
            transform.rotation = Quaternion.Euler(0, 0, 90f);
        }
        else if (velocity.y < 0f && Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x)) // forward �̵�
        {
            transform.rotation = Quaternion.Euler(0, 0, -90f);
        }
    }

    void CreateArcCollider()
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        List<Vector2> points = new List<Vector2>();

        // ȣ�� ���� ����
        float startAngle = -angle / 2;
        float startRad = Mathf.Deg2Rad * startAngle; // �������� ����

        // �� ���׸�Ʈ ������ ����
        float segmentAngle = angle / segments;
        float segmentRad = Mathf.Deg2Rad * segmentAngle; // �������� ����

        // ȣ�� �����ϴ� �� ����
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = startRad + i * segmentRad;
            float x = radius * Mathf.Cos(currentAngle);
            float y = radius * Mathf.Sin(currentAngle);
            points.Add(new Vector2(x, y));
        }

        // ȣ�� �� �� �߰�
        points.Add(Vector2.zero); // ����

        // Collider �� ����
        collider.points = points.ToArray();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            monster.SetPlayerDetected(true);
        }
    }
}