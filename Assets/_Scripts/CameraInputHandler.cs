using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraInputHandler : MonoBehaviour
{
    
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private CinemachineVirtualCamera vCamera;
    private const float MIN_FOV = 25f;
    private const float MAX_FOV = 60f;
    private CinemachineTransposer transposer;
    private InputActions cameraInputActions;

    private void Start() {
        transposer = vCamera.GetCinemachineComponent<CinemachineTransposer>();
        cameraInputActions = new InputActions();
        cameraInputActions.Camera.Enable();
    }


    private void Update() {
        MoveCamera();
        RotateCamera();
        ZoomCamera();
    }

    private void MoveCamera() {
        Vector2 cameraMoveInput = cameraInputActions.Camera.CameraMoveInputs.ReadValue<Vector2>();
        Vector3 cameraMoveVector = new Vector3(cameraMoveInput.x, 0f, cameraMoveInput.y);
        //Ensure camera move inputs reflect camera rotation.
        Vector3 cameraMoveDirection = transform.forward * cameraMoveVector.z + transform.right * cameraMoveVector.x;
        transform.position += cameraMoveDirection * moveSpeed * Time.deltaTime;
    }

    private void RotateCamera() {
        Vector3 cameraRotateVector = new Vector3(0, cameraInputActions.Camera.CameraRotateInputs.ReadValue<float>(), 0);
        transform.eulerAngles += cameraRotateVector * rotationSpeed * Time.deltaTime;
    }

    private void ZoomCamera() {
        Vector2 cameraZoomInput = cameraInputActions.Camera.CameraZoomInputs.ReadValue<Vector2>().normalized;
        //Change to + for reverse zoom.
        vCamera.m_Lens.FieldOfView -= cameraZoomInput.y;
        //Establish zoom limits.
        vCamera.m_Lens.FieldOfView = Mathf.Clamp(vCamera.m_Lens.FieldOfView, MIN_FOV, MAX_FOV);
    }

}
