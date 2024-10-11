using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoving : MonoBehaviour
{
    public Rigidbody2D BulletBody;
    public SpriteRenderer BulletRenderer;
    public float speedBullet=30f;
    public CounterScript counterScript;
    // Start is called before the first frame update
    void Start()
    {
        float defScale = 0.5f;
        this.transform.localScale = new Vector3 (defScale, defScale, defScale);
        
        BulletRenderer = GetComponent<SpriteRenderer>();
        BulletRenderer.sortingOrder = 4;
        if (BulletRenderer.sprite.name=="LightBulb"|| BulletRenderer.sprite.name == "LightBall")
        {
            BulletRenderer.color = new Color(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1));
        }
        
        BulletBody = GetComponent<Rigidbody2D>();
       
        gameObject.layer = 3;
        ShootBullet();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y < -55)
        {
            Destroy(gameObject);
        }
    }

    void ShootBullet()
    {
        //Lay vi tri cua chuot trong the gioi 2d
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Tinh toan huong tu vi tri hien tai den vi tri chuot
        Vector2 mouseDirection = (mouseWorldPosition - (Vector2)transform.position).normalized;
        //Dat van toc cho RigidBody2D de di chuyen doi tuong theo huong chuot voi toc do da dinh
        BulletBody.velocity= mouseDirection * speedBullet;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {       
        if (collision.gameObject.tag=="Enemy")
        {
            Destroy(gameObject);
        }
    }
}
