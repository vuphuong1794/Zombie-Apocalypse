using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool canMove=true;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    private float _obstacleCheckCircleRadius=0.45f;

    [SerializeField]
    private float _obstacleCheckDistance=1;

    [SerializeField]
    private LayerMask _obstacleLayerMask;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 _targetDirection;
    private float _changeDirectionCooldown;
    private RaycastHit2D[] _obstacleCollisions;
    private float _obstacleAvoidanceCooldown;
    private Vector2 _obstacleAvoidanceTargetDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up;
        _obstacleCollisions = new RaycastHit2D[10];
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            UpdateTargetDirection();
            RotateTowardsTarget();
            SetVelocity();
        }
    }

    //update hướng đi của enemy
    private void UpdateTargetDirection()
    {
        HandleRandomDirectionChange();
        HandlePlayerTargeting();
        HandleObstacles();
    }

    // Hành vi chuyển động loạn xạ của zombie khi không tìm thấy người chơi
    private void HandleRandomDirectionChange()
    {
        _changeDirectionCooldown -= Time.deltaTime;

        if (_changeDirectionCooldown <= 0)
        {
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            _targetDirection = rotation * _targetDirection;

            _changeDirectionCooldown = Random.Range(1f, 5f);
        }
    }

    // Tìm vị trị player
    private void HandlePlayerTargeting()
    {
        if (_playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
    }

    private void HandleObstacles()
    {
        // Giảm thời gian chờ để thay đổi hướng tránh vật cản mỗi khung hình.
        _obstacleAvoidanceCooldown -= Time.deltaTime;

        // Tạo một bộ lọc liên hệ để chỉ kiểm tra va chạm với các đối tượng thuộc lớp vật cản.
        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_obstacleLayerMask); // Đặt lớp cần kiểm tra dựa trên `_obstacleLayerMask`.

        // Thực hiện CircleCast để kiểm tra vật cản trong bán kính xác định từ vị trí kẻ thù.
        int numberOfCollisions = Physics2D.CircleCast(
            transform.position,                  // Vị trí kẻ thù.
            _obstacleCheckCircleRadius,           // Bán kính vòng tròn kiểm tra va chạm.
            transform.up,                         // Hướng kiểm tra va chạm.
            contactFilter,                        // Bộ lọc liên hệ để chỉ kiểm tra lớp vật cản.
            _obstacleCollisions,                  // Mảng chứa các va chạm phát hiện được.
            _obstacleCheckDistance);              // Khoảng cách tối đa để kiểm tra va chạm.

        // Lặp qua tất cả các va chạm phát hiện được.
        for (int index = 0; index < numberOfCollisions; index++)
        {
            var obstacleCollision = _obstacleCollisions[index]; // Lấy đối tượng va chạm tại vị trí index.

            // Kiểm tra xem vật cản này có phải là chính kẻ thù không, nếu đúng thì bỏ qua.
            if (obstacleCollision.collider.gameObject == gameObject)
            {
                continue; // Bỏ qua và chuyển sang vật cản tiếp theo.
            }

            // Nếu thời gian chờ tránh vật cản đã hết, cho phép thay đổi hướng tránh.
            if (_obstacleAvoidanceCooldown <= 0)
            {
                // Đặt hướng tránh vật cản là pháp tuyến của vật cản (hướng ra khỏi bề mặt).
                _obstacleAvoidanceTargetDirection = obstacleCollision.normal;

                // Đặt lại thời gian chờ tránh vật cản để không đổi hướng quá nhanh.
                _obstacleAvoidanceCooldown = 0.5f;
            }

            // Tính toán góc quay mục tiêu để hướng mặt trước của kẻ thù về phía tránh vật cản.
            var targetRotation = Quaternion.LookRotation(transform.forward, _obstacleAvoidanceTargetDirection);

            // Tạo góc quay trung gian giữa góc quay hiện tại và `targetRotation` với tốc độ xác định.
            var rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            // Cập nhật hướng di chuyển `_targetDirection` theo hướng đã quay để tránh vật cản.
            _targetDirection = rotation * Vector2.up;
            break; // Thoát vòng lặp sau khi xử lý vật cản đầu tiên.
        }

    }

    //quay theo hướng player
    private void RotateTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        _rigidbody.SetRotation(rotation);
    }

    //cho enemy tiến lại player
    private void SetVelocity()
    {
        _rigidbody.velocity = _targetDirection * _speed;
    }
}
