using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void UpdateHealthBar() {
        healthBar.fillAmount = unit.GetCurrentHealth();
    }

    private void UnitHandler_OnDamaged(object sender, EventArgs e) {
        UpdateHealthBar();
    }

}
