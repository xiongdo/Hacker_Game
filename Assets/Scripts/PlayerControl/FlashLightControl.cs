using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class FlashLightControl : MonoBehaviour
{
    [Range(0f, 10f)]
    public float _roationSpeed = 5f;

    public bool _isOpen = false;

    private Vector2 _input;

    // Update is called once per frame
    void Update()
    {
        HandleLightRotation();
    }

    public void SwitchLight()
    {
        _isOpen = !_isOpen;
        gameObject.SetActive(_isOpen);
    }

    public bool IsLightOpen()
    {
        return _isOpen;
    }

    public void TurnOn()
    {
        _isOpen = true;
        gameObject.SetActive(_isOpen);
    }

    public void TurnOff()
    {
        _isOpen = false;
        gameObject.SetActive(_isOpen);
    }

    private void HandleLightRotation()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;
        var targetOrientation = ((mousePos - transform.position)).normalized;
        if (targetOrientation.sqrMagnitude > 0.01f)
        {
            var deltaRotation = Quaternion.FromToRotation(transform.right, targetOrientation);
            var deltaAngles = deltaRotation.eulerAngles;
            deltaAngles = new Vector3(0f, 0f, deltaAngles.z);
            var targetRotation = Quaternion.Euler(deltaAngles) * transform.localRotation;
            
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation, targetRotation, _roationSpeed * Time.deltaTime
                );
        }
    }
}
