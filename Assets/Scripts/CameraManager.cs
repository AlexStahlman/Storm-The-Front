using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerManager;
    [SerializeField] private Transform cameraTransform;

    private Vector3 cameraOffset;
    private float cameraMoveSpeed;

    void Start()
    {
        cameraOffset = new Vector3(0f, 2f, -7f);
        cameraMoveSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        float yPos = 2f;
        Vector3 cameraEndPosition = new Vector3(playerManager.transform.position.x, yPos, playerManager.transform.position.z) + cameraOffset;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraEndPosition, cameraMoveSpeed * Time.deltaTime);
    }
}
