using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;

    bool isWallTouch;

    public LayerMask wallLayer;
    public Transform wallCheckPoint;

    private Rigidbody2D _rigidbody;

    private Vector2 _movementInput;
    private Vector2 _smoothMovementInput;
    private Vector2 _movementInputSmoothVelocity;

    private bool facingRight = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //di chuyển mượt hơn
        SetPlayerVelocity();
        RotateInDirectionOfInput();

        /*
        // Kiểm tra va chạm tường
        isWallTouch = Physics2D.OverlapBox(wallCheckPoint.position, new Vector2(0.3f, 1f), 0, wallLayer);

        // Nếu chạm tường thì xoay người
        if (isWallTouch)
        {
            Flip();
        }

        // Xoay nhân vật theo hướng di chuyển
        if (_movementInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_movementInput.x < 0 && facingRight)
        {
            Flip();
        }*/
    }

    private void SetPlayerVelocity()
    {
        _smoothMovementInput = Vector2.SmoothDamp(
                    _smoothMovementInput,
                    _movementInput,
                    ref _movementInputSmoothVelocity,
                    0.1f);

        // Di chuyển nhân vật theo đầu vào
        _rigidbody.velocity = _smoothMovementInput * _speed;
    }

    // Xoay mặt nhân vật mà không ảnh hưởng đến hướng di chuyển
    public void Flip()
    {
        facingRight = !facingRight;  // Đảo trạng thái hướng quay
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Đảo chiều đối tượng theo trục X
        transform.localScale = localScale;
    }

    private void RotateInDirectionOfInput()
    {
        //kiểm tra nếu người chơi có di chuyển hay không 
        if(_movementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _smoothMovementInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed + Time.deltaTime);

            _rigidbody.MoveRotation(rotation);  
        }
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
}
