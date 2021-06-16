using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CellDirection
{
    Left,
    Top,
    Right,
    Down
}

public class CharacterMove : MonoBehaviour
{
    public float _moveSpeed;

    protected Vector2 _input = Vector2.zero;

    private Vector3Int _curTilePos;

    private Vector3 _targetMovePosition;

    private SpriteRenderer _sprite;

    private Rigidbody2D _rigidBody;

    private FlashLightControl _lightControl;

    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _lightControl = GetComponentInChildren<FlashLightControl>(true);

        _targetMovePosition = transform.position;
        
        UpdateTilePos();
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
        _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        HandleMove();
    }

    private void HandleMove()
    {
        transform.position = Vector3.MoveTowards(
            transform.position, _targetMovePosition, _moveSpeed * Time.deltaTime
            );
        if (Vector3.Distance(transform.position, _targetMovePosition) < 0.05f)
        {
            var castVector = Vector3.zero;
            var moveVector = Vector3Int.zero;
            if (Input.GetKeyDown(KeyCode.D))
            {
                castVector = Vector3.right;
                moveVector = Vector3Int.right;
            }   
            else if (Input.GetKeyDown(KeyCode.A))
            {
                castVector = Vector3.left;
                moveVector = Vector3Int.left;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                castVector = Vector3.up;
                moveVector = Vector3Int.up;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                castVector = Vector3.down;
                moveVector = Vector3Int.down;
            }

            if (moveVector.sqrMagnitude > 0.01f)
            {
                var hit = Physics2D.Linecast(transform.position, transform.position + castVector * 0.8f); 
                if (!hit)
                {
                    Vector3 cellSize = GameWorld.Instance.Map.cellSize;
                    _targetMovePosition = 
                        GameWorld.Instance.Map.CellToWorld(_curTilePos + moveVector) + 
                        new Vector3((cellSize).x / 2f, cellSize.y / 2f);
                }
            }
        }
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
