using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightControl : MonoBehaviour
{
    [Range(0f, 10f)]
    public float _roationSpeed = 5f;

    private Vector2 _input;

    // Update is called once per frame
    void Update()
    {
        HandleLightRotation();
    }

    private void HandleLightRotation()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;
        var targetOrientation = (mousePos - transform.position).normalized;
        if (targetOrientation.sqrMagnitude > 0.01f)
        {
            var targetRotation = Quaternion.LookRotation(targetOrientation, Vector3.forward);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, targetRotation, _roationSpeed * Time.deltaTime
                );
        }
    }
}
