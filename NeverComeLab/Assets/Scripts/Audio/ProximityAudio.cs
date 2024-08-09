using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityAudio : MonoBehaviour
{
    public Transform player; // 플레이어의 위치
    public AudioSource audioSource; // 오디오 소스
    [SerializeField] private float maxVolumeDistance = 5f; // 볼륨이 최대가 되는 거리
    [SerializeField] private float minVolumeDistance = 17f; // 볼륨이 최소가 되는 거리

    void Update()
    {
        // 플레이어와 오디오 소스 사이의 거리 계산
        float distance = Vector3.Distance(player.position, transform.position);

        // 거리에 따른 볼륨 계산
        if (distance < maxVolumeDistance)
        {
            audioSource.volume = 1f; // 최소 거리보다 가까우면 최대 볼륨
        }
        else if (distance > minVolumeDistance)
        {
            audioSource.volume = 0f; // 최대 거리보다 멀면 볼륨 0
        }
        else
        {
            // 거리에 따라 볼륨을 선형적으로 조정
            audioSource.volume = 1f - ((distance - maxVolumeDistance) / (minVolumeDistance - maxVolumeDistance));
        }
    }
}
