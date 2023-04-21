using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Script which deals with the visual healthbar component that are aligned overhead of Units.
 */
public class UnitOverheadUI : MonoBehaviour {

    [SerializeField] private UnitHandler unit;
    [SerializeField] private Image healthBar;
    private Transform camera;

    private void Awake() {
        camera = Camera.main.transform;
    }

    private void Start() {
        unit.OnDamaged += UnitHandler_OnDamaged;
        UpdateHealthBar();
    }

    private void LateUpdate() {
        //Make UI always face the camera.
        Vector3 directionToCamera = (camera.position - transform.position).normalized;
        transform.LookAt(transform.position + directionToCamera * -1);
    }

    //Functions below ensure that the Healthbar is always visually updated whenever health values are changed.
    private void UpdateHealthBar() {
        //Adjust fill amount via a normalized value of MaxHP and HP of unit.
        healthBar.fillAmount = unit.GetCurrentHealth();
    }

    private void UnitHandler_OnDamaged(object sender, EventArgs e) {
        UpdateHealthBar();
    }

}
