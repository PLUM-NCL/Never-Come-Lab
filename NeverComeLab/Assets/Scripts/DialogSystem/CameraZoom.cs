using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;  // �ó׸ӽ� ���� ī�޶�
    public float zoomInSize = 3f;  // ������ ���� Orthographic Size
    public float zoomOutSize = 5f;  // �ܾƿ��� ���� Orthographic Size
    public float zoomSpeed = 5f;  // ����/�ܾƿ� �ӵ�

    private float targetSize;  // ��ǥ�� �ϴ� Orthographic Size
    private bool isZoomIn = false;

    void Start()
    {
        if (virtualCamera == null)
            virtualCamera = GetComponent<CinemachineVirtualCamera>();

        // ���� ī�޶��� Orthographic Size�� ��ǥ ũ��� ����
        targetSize = virtualCamera.m_Lens.OrthographicSize;
    }

    void Update()
    {
        
        // Z Ű�� ������ �� ����
        if (DialogController.IsConversation && !isZoomIn)
        {
            targetSize = zoomInSize;
            isZoomIn = true;
        }

        // X Ű�� ������ �� �ܾƿ�
        if (!DialogController.IsConversation && isZoomIn)
        {
            targetSize = zoomOutSize;
            isZoomIn= false;
        }

        // �ó׸ӽ� ī�޶��� Orthographic Size�� ���������� �����մϴ�.
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetSize, Time.deltaTime * zoomSpeed);
    }
}
