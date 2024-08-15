using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;  // 시네머신 가상 카메라
    public float zoomInSize = 3f;  // 줌인할 때의 Orthographic Size
    public float zoomOutSize = 5f;  // 줌아웃할 때의 Orthographic Size
    public float zoomSpeed = 5f;  // 줌인/줌아웃 속도

    private float targetSize;  // 목표로 하는 Orthographic Size
    private bool isZoomIn = false;

    void Start()
    {
        if (virtualCamera == null)
            virtualCamera = GetComponent<CinemachineVirtualCamera>();

        // 현재 카메라의 Orthographic Size를 목표 크기로 설정
        targetSize = virtualCamera.m_Lens.OrthographicSize;
    }

    void Update()
    {
        
        // Z 키를 눌렀을 때 줌인
        if (DialogController.IsConversation && !isZoomIn)
        {
            targetSize = zoomInSize;
            isZoomIn = true;
        }

        // X 키를 눌렀을 때 줌아웃
        if (!DialogController.IsConversation && isZoomIn)
        {
            targetSize = zoomOutSize;
            isZoomIn= false;
        }

        // 시네머신 카메라의 Orthographic Size를 점진적으로 변경합니다.
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetSize, Time.deltaTime * zoomSpeed);
    }
}
