using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed; // Tốc độ di chuyển của người chơi

    [SerializeField]
    private float _rotationSpeed; // Tốc độ xoay của người chơi

    bool isWallTouch; //kiểm tra xem người chơi có chạm tường không
    public LayerMask wallLayer; 
    public Transform wallCheckPoint; 

    private Rigidbody2D _rigidbody; 
    private Vector2 _movementInput; 
    private Vector2 _smoothMovementInput; 
    private Vector2 _movementInputSmoothVelocity; 
    private bool facingRight = true; 

    //Được gọi khi script được khởi tạo.
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Áp dụng chuyển động và xoay trong FixedUpdate để có vật lý nhất quán
        SetPlayerVelocity();
        RotateInDirectionOfInput();

        // Đoạn mã sau đây bị comment, nhưng nó sẽ xử lý phát hiện tường và lật người chơi
        /*
        // Kiểm tra va chạm với tường
        isWallTouch = Physics2D.OverlapBox(wallCheckPoint.position, new Vector2(0.3f, 1f), 0, wallLayer);
        
        // Nếu chạm tường, lật người chơi
        if (isWallTouch)
        {
            Flip();
        }
        
        // Lật người chơi dựa trên hướng di chuyển
        if (_movementInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_movementInput.x < 0 && facingRight)
        {
            Flip();
        }
        */
    }

    private void SetPlayerVelocity()
    {
        // Làm mượt đầu vào chuyển động
        _smoothMovementInput = Vector2.SmoothDamp(
                    _smoothMovementInput,
                    _movementInput,
                    ref _movementInputSmoothVelocity,
                    0.1f);

        _rigidbody.velocity = _smoothMovementInput * _speed;
    }

    // Lật sprite của người chơi theo chiều ngang
    public void Flip()
    {
        facingRight = !facingRight;  // Đảo ngược hướng mặt
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Đảo ngược tỷ lệ X để lật sprite
        transform.localScale = localScale;
    }

    private void RotateInDirectionOfInput()
    {
        // Kiểm tra nếu có bất kỳ đầu vào chuyển động nào
        if (_movementInput != Vector2.zero)
        {
            // Tính toán góc xoay mục tiêu dựa trên hướng chuyển động
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _smoothMovementInput);

            // Xoay tới góc xoay mục tiêu
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed + Time.deltaTime);

            // Áp dụng góc xoay cho Rigidbody2D
            _rigidbody.MoveRotation(rotation);
        }
    }

    // Được gọi bởi Input System khi phát hiện đầu vào chuyển động
    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
}