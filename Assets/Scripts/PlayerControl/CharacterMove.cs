using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Packages.Rider.Editor.UnitTesting;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;

public enum CellDirection
{
    Left,
    LeftUp,
    Up,
    RightUp,
    Right,
    RightDown,
    Down,
    LeftDown
}

public static class CellDirectionExtension
{
    public static Vector3Int GetVectorFromDirection(this CellDirection direction)
    {
        switch (direction)
        {
            case CellDirection.Left:
                return Vector3Int.left;
            case CellDirection.Down:
                return Vector3Int.down;
            case CellDirection.Right:
                return Vector3Int.right;
            case CellDirection.Up:
                return Vector3Int.up;
            case CellDirection.LeftDown:
                return new Vector3Int(-1, -1, 0);
            case CellDirection.RightDown:
                return new Vector3Int(1, -1, 0);
            case CellDirection.RightUp:
                return new Vector3Int(1, 1, 0);
            case CellDirection.LeftUp:
                return new Vector3Int(-1, 1, 0);
            default:
                return Vector3Int.zero;
        }
    }

    public static Vector3Int GetDirectionCellPos(this CellDirection direction, Vector3Int cellPos)
    {
        return cellPos + direction.GetVectorFromDirection();
    }

    public static Vector3Int GetOppositeCellPos(this CellDirection direction, Vector3Int cellPos)
    {
        return cellPos - direction.GetVectorFromDirection();
    }
}

public class CharacterMove : MonoBehaviour
{
    public float _moveSpeed;

    protected Vector2 _input = Vector2.zero;

    private Vector3Int _curTilePos;

    private Vector3 _targetMovePosition;

    private bool _isInSetMode = false;

    private SpriteRenderer _sprite;

    private FlashLightControl _lightControl;

    private TransformerSetter _setter;

    private Transform _setModeLight;

    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _setter = GetComponent<TransformerSetter>();
        _lightControl = GetComponentInChildren<FlashLightControl>(true);
        _setModeLight = transform.Find("SetModeLight");

        _targetMovePosition = transform.position;
        
        UpdateTilePos();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _isInSetMode = !_isInSetMode;
            if (!_isInSetMode)
            {
                ExitSetMode();
            }
            else
            {
                EnterSetMode();
            }
        }

        if (_isInSetMode)
        {
            HandleSetMode();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchFlashLight();
            }
        
            HandleInput();
            UpdateTilePos();
            UpdateCharacterSpriteDirection();
        }
    }

    private void ExitSetMode()
    {
        _setter.HideSetter();
        _setModeLight.gameObject.SetActive(false);
        GameWorld.Instance.TurnOnGloablLight(1F);
        _isInSetMode = false;
    }

    private void EnterSetMode()
    {
        _setModeLight.gameObject.SetActive(true);
        GameWorld.Instance.TurnOffGlobalLight();
    }

    private void HandleSetMode()
    {
        var mouseGridPos = GameWorld.Instance.Map.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     _setter.Preview(_curTilePos + Vector3Int.right);
        // }   
        // else if (Input.GetKeyDown(KeyCode.A))
        // {
        //     _setter.Preview(_curTilePos + Vector3Int.left);
        // }
        // else if (Input.GetKeyDown(KeyCode.W))
        // {
        //     _setter.Preview(_curTilePos + Vector3Int.up);
        // }
        // else if (Input.GetKeyDown(KeyCode.S))
        // {
        //     _setter.Preview(_curTilePos + Vector3Int.down);
        // }
        
        _setter.Preview(mouseGridPos);

        if (Input.GetMouseButtonUp(0))
        {
            _setter.SetTransformer();
        }
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
                        GameWorld.Instance.GetCellCenterWorldPos(_curTilePos + moveVector);
                }
            }
        }
    }

    private void UpdateTilePos()
    {
        _curTilePos = GameWorld.Instance.Map.WorldToCell(transform.position);
        // Debug.Log("Cur Cell pos: " + _curTilePos);
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
