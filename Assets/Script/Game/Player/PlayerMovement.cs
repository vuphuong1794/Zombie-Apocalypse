using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f; // Tốc độ di chuyển của người chơi
    private Rigidbody2D _rigidbody; // Thân vật lý của người chơi
    private Vector2 _movementInput; // Đầu vào chuyển động
    private Inventory inventory;

    private void Awake()    
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        inventory = new Inventory();
    }

    private void FixedUpdate()
    {
        // Di chuyển và xoay nhân vật trong FixedUpdate để đảm bảo tính nhất quán của vật lý
        MovePlayer();
        RotateTowardsMouse();
    }

    private void MovePlayer()
    {
        // Tính toán vị trí mục tiêu mới
        Vector2 targetPosition = _rigidbody.position + _movementInput * _speed * Time.fixedDeltaTime;

        // Di chuyển đối tượng đến vị trí mục tiêu bằng MovePosition
        _rigidbody.MovePosition(targetPosition);
    }

    private void RotateTowardsMouse()
    {
        // Lấy vị trí chuột trong không gian thế giới 2D
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Tính toán vector hướng từ vị trí của nhân vật đến vị trí của chuột
        Vector2 direction = mousePos - transform.position;

        // Tính góc giữa hướng này và trục x, sau đó chuyển từ radian sang độ
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Xoay nhân vật sao cho nó hướng về phía chuột
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Được gọi bởi Input System khi phát hiện đầu vào chuyển động
    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
}
