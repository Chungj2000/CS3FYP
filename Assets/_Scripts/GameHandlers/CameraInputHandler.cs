using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/*
 * The Script that serves as the controller logic for manipulating the camera position and angles.
 * Camera is independant for Player clients.
 */
public class CameraInputHandler : MonoBehaviour {

    public static CameraInputHandler INSTANCE {get; private set;}
    
    [Header("Camera Controll Parameters")]
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private CinemachineVirtualCamera vCamera;

    [Header("Restrict Camera To")]
    [SerializeField] private float lockCameraX_Min = 0f;
    [SerializeField] private float lockCameraX_Max = 20f;
    [SerializeField] private float lockCameraZ_Min = 0f;
    [SerializeField] private float lockCameraZ_Max = 20f;

    private Vector3 cameraMoveX_Min;
    private Vector3 cameraMoveX_Max;
    private Vector3 cameraMoveZ_Min;
    private Vector3 cameraMoveZ_Max;


    private const float MIN_FOV = 25f;
    private const float MAX_FOV = 60f;
    private CinemachineTransposer transposer;
    private InputActions cameraInputActions;

    private void Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("CameraInputHandler instance created.");
        } else {
            Debug.Log("More than one CameraInputHandler instance created.");
            Destroy(this);
            return;
        }
    }

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

    //Initiate Camera for Player 2.
    public void InitiateCamera() {
        if(!PlayerHandler.INSTANCE.IsPlayer1()) {
            transform.eulerAngles = new Vector3(transform.rotation.x, 180, transform.rotation.z);
            transform.position = new Vector3(transform.position.x, transform.position.y, 15);
        }
    }

    //WASD input keys for moving the camera.
    private void MoveCamera() {
        Vector2 cameraMoveInput = cameraInputActions.Camera.CameraMoveInputs.ReadValue<Vector2>();
        Vector3 cameraMoveVector = new Vector3(cameraMoveInput.x, 0f, cameraMoveInput.y);
        //Ensure camera move inputs reflect camera rotation.
        Vector3 cameraMoveDirection = transform.forward * cameraMoveVector.z + transform.right * cameraMoveVector.x;
        
        transform.position += cameraMoveDirection * moveSpeed * Time.deltaTime;

        //Set boundaries for where the camera can move to using mix & max values.
        cameraMoveX_Min = new Vector3(lockCameraX_Min, 0f, transform.position.z);
        cameraMoveX_Max = new Vector3(lockCameraX_Max, 0f, transform.position.z);
        cameraMoveZ_Min = new Vector3(transform.position.x, 0f, lockCameraZ_Min);
        cameraMoveZ_Max = new Vector3(transform.position.x, 0f, lockCameraZ_Max);

        //Transform position logic for boundaries.
        if(transform.position.x < lockCameraX_Min) {
            transform.position = cameraMoveX_Min;
        } else if (transform.position.x > lockCameraX_Max) {
            transform.position = cameraMoveX_Max;
        } else if (transform.position.z < lockCameraZ_Min) {
            transform.position = cameraMoveZ_Min;
        } else if (transform.position.z > lockCameraZ_Max) {
            transform.position = cameraMoveZ_Max;
        }
    }

    //Q & E input keys for pivoting the camera left and right.
    private void RotateCamera() {
        Vector3 cameraRotateVector = new Vector3(0, cameraInputActions.Camera.CameraRotateInputs.ReadValue<float>(), 0);
        transform.eulerAngles += cameraRotateVector * rotationSpeed * Time.deltaTime;
    }

    //Scroll wheel input for panning the FOV up to a defined maximum and minimum value.
    private void ZoomCamera() {
        Vector2 cameraZoomInput = cameraInputActions.Camera.CameraZoomInputs.ReadValue<Vector2>().normalized;
        //Change to + for reverse zoom.
        vCamera.m_Lens.FieldOfView -= cameraZoomInput.y * zoomSpeed;
        //Establish zoom limits.
        vCamera.m_Lens.FieldOfView = Mathf.Clamp(vCamera.m_Lens.FieldOfView, MIN_FOV, MAX_FOV);
    }

}
