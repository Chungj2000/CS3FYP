using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{

    public static MouseHandler INSTANCE {get; private set;}
    [SerializeField] private LayerMask gridLayerMask;

    private void  Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("MouseHandler instance created.");
        } else {
            Debug.LogError("More than one MouseHandler instance created.");
            Destroy(this);
            return;
        }
    }

    private void Update()
    {
        transform.position = MouseHandler.INSTANCE.GetMousePosition();
    }

    public Vector3 GetMousePosition() {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, MouseHandler.INSTANCE.gridLayerMask)) {
            return raycastHit.point;
        } else {
            return new Vector3(0,0,0);
        }
    }
}
