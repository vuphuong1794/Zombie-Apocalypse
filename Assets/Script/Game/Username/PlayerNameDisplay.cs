using TMPro;
using UnityEngine;

public class PlayerNameDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;

    [SerializeField]
    private Vector3 offsetPosition;

    [SerializeField]
    private GameObject _target;

    [SerializeField]
    private Camera _camera;

    private void Start()
    {
        nameText=GetComponent<TMP_Text>();

        _target = transform.parent.parent.gameObject;
        Debug.Log("_target: " + _target);
        if (_camera == null)
        {
            _camera = Camera.main; // Camera chính sẽ tự động được gán.
        }
        // Lấy tên từ PlayerPrefs
        string username = PlayerPrefs.GetString("PlayerUsername", "Player");

        // Gán tên vào Text
        if (nameText != null)
        {
            nameText.text = username;
        }
        else
        {
            Debug.LogWarning("Name Text is not assigned!");
        }
    }

    void Update()
    {
        transform.rotation = _camera.transform.rotation;
        transform.position = _target.transform.position + offsetPosition;
    }
}
