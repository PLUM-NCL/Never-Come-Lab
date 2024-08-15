using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityAudio : MonoBehaviour
{
    public Transform player; // �÷��̾��� ��ġ
    public AudioSource audioSource; // ����� �ҽ�
    [SerializeField] private float maxVolumeDistance = 5f; // ������ �ִ밡 �Ǵ� �Ÿ�
    [SerializeField] private float minVolumeDistance = 17f; // ������ �ּҰ� �Ǵ� �Ÿ�

    void Update()
    {
        // �÷��̾�� ����� �ҽ� ������ �Ÿ� ���
        float distance = Vector3.Distance(player.position, transform.position);

        // �Ÿ��� ���� ���� ���
        if (distance < maxVolumeDistance)
        {
            audioSource.volume = 1f; // �ּ� �Ÿ����� ������ �ִ� ����
        }
        else if (distance > minVolumeDistance)
        {
            audioSource.volume = 0f; // �ִ� �Ÿ����� �ָ� ���� 0
        }
        else
        {
            // �Ÿ��� ���� ������ ���������� ����
            audioSource.volume = 1f - ((distance - maxVolumeDistance) / (minVolumeDistance - maxVolumeDistance));
        }
    }
}
