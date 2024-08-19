using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Windows;

[RequireComponent(typeof(PolygonCollider2D))]
public class ArcMonsterSensor : MonoBehaviour
{
    private Monster monster;

    public float radius = 2f; // 호의 반지름
    public float angle = 45f; // 호의 각도 (단위: 도)
    public int segments = 100; // 호를 형성하는 세그먼트 수

    void Start()
    {
        CreateArcCollider();
        monster = GetComponentInParent<Monster>();
    }

    private void Update()
    {
        Vector3 velocity = monster.agent.velocity;

        if (velocity.x > 0f && Mathf.Abs(velocity.y) < Mathf.Abs(velocity.x)) // right 이동
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (velocity.x < 0f && Mathf.Abs(velocity.y) < Mathf.Abs(velocity.x)) // left 이동
        {
            transform.rotation = Quaternion.Euler(0, 0, 180f);
        }
        else if (velocity.y > 0f && Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x)) // back 이동
        {
            transform.rotation = Quaternion.Euler(0, 0, 90f);
        }
        else if (velocity.y < 0f && Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x)) // forward 이동
        {
            transform.rotation = Quaternion.Euler(0, 0, -90f);
        }
    }

    void CreateArcCollider()
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        List<Vector2> points = new List<Vector2>();

        // 호의 시작 각도
        float startAngle = -angle / 2;
        float startRad = Mathf.Deg2Rad * startAngle; // 라디안으로 변경

        // 각 세그먼트 사이의 각도
        float segmentAngle = angle / segments;
        float segmentRad = Mathf.Deg2Rad * segmentAngle; // 라디안으로 변경

        // 호를 정의하는 점 생성
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = startRad + i * segmentRad;
            float x = radius * Mathf.Cos(currentAngle);
            float y = radius * Mathf.Sin(currentAngle);
            points.Add(new Vector2(x, y));
        }

        // 호의 끝 점 추가
        points.Add(Vector2.zero); // 원점

        // Collider 점 설정
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