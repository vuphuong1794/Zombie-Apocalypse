using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    bool isWallTouch;
    public LayerMask wallLayer;
    public Transform wallCheckPoint;

    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;

    private bool facingRight = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Di chuyển nhân vật theo đầu vào, chỉ tính theo hướng X, không đảo ngược Y
        _rigidbody.velocity = new Vector2(_movementInput.x * _speed, _movementInput.y * _speed);

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
        }
    }

    // Xoay mặt nhân vật mà không ảnh hưởng đến hướng di chuyển
    public void Flip()
    {
        facingRight = !facingRight;  // Đảo trạng thái hướng quay
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Đảo chiều đối tượng theo trục X
        transform.localScale = localScale;
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
}
