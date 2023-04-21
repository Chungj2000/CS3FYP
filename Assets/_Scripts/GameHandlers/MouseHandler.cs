using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script used to determine where the mouse cursor is currently located on the application.
 */
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

    //Shows where the mouse is currently pointed at via a gameObject visual this Script is attached to.
    private void Update() {
        transform.position = MouseHandler.INSTANCE.GetMousePosition();
    }

    //Return the co-ordinates of the mouse on a 3D plain on screen.
    public Vector3 GetMousePosition() {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, MouseHandler.INSTANCE.gridLayerMask)) {
            return raycastHit.point;
        } else {
            return new Vector3(0,0,0);
        }
    }
}
