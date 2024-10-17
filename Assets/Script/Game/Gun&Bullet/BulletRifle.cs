using UnityEngine;

public class BulletRifle : MonoBehaviour
{
    [SerializeField] private float timeMaxExist = 3f; // Thời gian tồn tại tối đa của viên đạn
    private Rigidbody2D bulletBody;
    float timeCount;

    private void Start()
    {
        //Do viên đạn nó bị vuông góc theo trục y thay vì song song
        //với trục x nên khi tạo ra cần xoay nó lại 90 độ để nó
        //đúng hướng với hướng người chơi
        transform.Rotate(new Vector3(0, 0, 90));
        //Định dạng kích thước mặc định của viên đạn
        float defScaleBullet = 0.1f;        
        gameObject.transform.localScale = new Vector3(defScaleBullet,defScaleBullet,defScaleBullet);

        //Lấy cơ thể vật lí của đạn
        bulletBody = GetComponent<Rigidbody2D>();

        //Thời gian đếm tồn tại của đạn
        timeCount = 0;
        //Kiểm tra object đạn có tồn tại ko
        if (bulletBody == null)
        {
            Debug.LogError("Rigidbody2D không được tìm thấy trên viên đạn.");
            return;
        }
    }
    private void Update()
    {
        if (timeCount > timeMaxExist)
        {
            Destroy(gameObject);
        }
        timeCount+=Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu viên đạn va chạm với đối tượng có tag "Zombie"
        if (collision.gameObject.tag=="Zombie")
        {
            // Hủy viên đạn khi va chạm với zombie
            Destroy(gameObject);
        }
        // Kiểm tra nếu viên đạn va chạm với đối tượng có tag "Wall"
        else if (collision.gameObject.tag=="Wall")
        {
            // Lấy vận tốc hiện tại của viên đạn
            Vector3 currentVel = bulletBody.velocity;

            // Lấy điểm tiếp xúc đầu tiên khi va chạm với tường
            ContactPoint2D contactPoint = collision.contacts[0];

            // Lấy vector pháp tuyến của điểm va chạm
            Vector2 collisionNormal = contactPoint.normal;

            // Tính toán vận tốc phản xạ
            Vector2 reflectedVel = Vector2.Reflect(currentVel, collisionNormal);

            // Đặt vận tốc mới cho viên đạn để nó phản xạ khỏi tường
            bulletBody.velocity = reflectedVel;
        }
    }



}
