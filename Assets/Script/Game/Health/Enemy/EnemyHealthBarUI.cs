using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    [SerializeField]
    private Vector3 offsetPosition;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private GameObject _target;

    void Start()
    {
        _target = transform.parent.parent.gameObject;
        if (_camera == null)
        {
            _camera = Camera.main; // Camera chính sẽ tự động được gán.
        }
    }


    void Update()
    {
        transform.rotation = _camera.transform.rotation;
        transform.position = _target.transform.position + offsetPosition;
    }

    //Thay đổi độ dài tối đa thanh máu
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    //Thay đổi độ dài thanh máu hiện tại
    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
