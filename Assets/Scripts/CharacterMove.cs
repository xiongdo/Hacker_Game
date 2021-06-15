using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float _moveSpeed;

    protected Vector2 _input = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        transform.position += 
            new Vector3(_input.x * _moveSpeed * Time.deltaTime, _input.y * _moveSpeed * Time.deltaTime, 0.0f);
    }
}
