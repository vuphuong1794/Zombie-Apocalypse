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

    public GameObject bulletPrefab; // Đổi tên biến thành bulletPrefab cho rõ ràng hơn
    public Transform firePos;
    public float TimeBtwFire = 0.2f;
    public float bulletForce;

    private float timeBtwFire;

    //Được gọi khi script được khởi tạo.
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        timeBtwFire = 0; // Khởi tạo timeBtwFire
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
        timeBtwFire -= Time.deltaTime; // Giảm thời gian chờ

        if (Input.GetMouseButton(0) && timeBtwFire <= 0) // Kiểm tra nếu có thể bắn
        {
            FireBullet();
        }
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
        foreach (Transform child in transform)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 lookDir = mousePos - child.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.Euler(0, 0, angle+90);
            child.rotation = rotation;

            
        }
    }

    // Được gọi bởi Input System khi phát hiện đầu vào chuyển động
    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }


    void FireBullet()
    {
        timeBtwFire = TimeBtwFire; // Đặt lại thời gian chờ

        // Tạo một góc quay mới cho viên đạn, xoay thêm 90 độ so với hướng hiện tại của firePos
        Quaternion bulletRotation = firePos.rotation * Quaternion.Euler(0, 0, 90);

        // Sử dụng góc quay mới này khi tạo ra viên đạn
        GameObject bulletInstance = Instantiate(bulletPrefab, firePos.position, bulletRotation);

        // Thay đổi tỷ lệ của viên đạn để nó nhỏ hơn 10 lần
        bulletInstance.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        // Thêm lực cho viên đạn
        Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();
        if (rb != null) // Kiểm tra xem Rigidbody2D có tồn tại không
        {
            rb.AddForce(firePos.up * bulletForce, ForceMode2D.Impulse);
        }
    }


}