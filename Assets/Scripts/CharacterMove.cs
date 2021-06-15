using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float _moveSpeed;

    protected Vector2 _input = Vector2.zero;

    private Vector3 _curTilePos;

    private SpriteRenderer _sprite;

    private Rigidbody2D _rigidBody;

    private FlashLightControl _lightControl;

    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _lightControl = GetComponentInChildren<FlashLightControl>(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchFlashLight();
        }
        
        HandleInput();
        UpdateTilePos();
        UpdateCharacterSpriteDirection();
    }

    private void HandleInput()
    {
        _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        var pos = 
            new Vector3(_input.x * _moveSpeed, _input.y * _moveSpeed, 0.0f);

        _rigidBody.velocity = pos;
    }

    private void UpdateTilePos()
    {
        _curTilePos = GameWorld.Instance.Map.WorldToCell(transform.position);
        Debug.Log("Cur Cell pos: " + _curTilePos);
    }

    private void UpdateCharacterSpriteDirection()
    {
        if (_input.x < -0.01)
        {
            if (_sprite)
            {
                _sprite.flipX = true;
            }
        }
        else if (_input.x > 0.01)
        {
            if (_sprite)
            {
                _sprite.flipX = false;
            }
        }
    }

    private void SwitchFlashLight()
    {
        _lightControl.SwitchLight();
    }
}
