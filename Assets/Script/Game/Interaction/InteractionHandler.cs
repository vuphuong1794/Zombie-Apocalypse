using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField]
    private float interactionRange = 2f; // Phạm vi tương tác
    [SerializeField]
    private KeyCode interactionKey = KeyCode.E; // Phím để tương tác
    private bool isOpened = false; // Trạng thái đã mở hay chưa
    private Transform player; // Vị trí của người chơi

    [Header("Audio Settings")]
    [SerializeField]
    private AudioClip openSound; // Âm thanh khi mở
    [SerializeField]
    private AudioClip closeSound; // Âm thanh khi đóng
    private AudioSource audioSource; // Nguồn phát âm thanh

    private void Start()
    {
        // Tìm người chơi dựa trên tag "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the Player has the tag 'Player'.");
        }

        // Kiểm tra và thêm AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        // Kiểm tra nếu người chơi nằm trong phạm vi tương tác
        if (player != null && Vector2.Distance(player.position, transform.position) <= interactionRange)
        {
            ShowInteractionPrompt(); // Hiển thị thông báo tương tác (nếu có)

            // Kiểm tra phím bấm
            if (Input.GetKeyDown(interactionKey))
            {
                Interact();
            }
        }
        else
        {
            HideInteractionPrompt(); // Ẩn thông báo tương tác (nếu có)
        }
    }

    private void Interact()
    {
        if (!isOpened)
        {
            isOpened = true;
            Debug.Log("Opening " + name);
            PlaySound(openSound); // Phát âm thanh mở
            OpenAction();
        }
        else
        {
            isOpened = false;
            Debug.Log("Closing " + name);
            PlaySound(closeSound); // Phát âm thanh đóng
            CloseAction();
        }
    }

    private void OpenAction()
    {
        // Thực hiện hành động khi mở (ví dụ: xoay cửa 90 độ)
        //transform.rotation = Quaternion.Euler(0, 0, 90);

        // Tăng rotation thêm 90 độ
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z + 90
        );
    }

    private void CloseAction()
    {
        // Thực hiện hành động khi đóng (ví dụ: xoay cửa trở lại)
        //transform.rotation = Quaternion.Euler(0, 0, 0);

        // Giảm rotation thêm 90 độ
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z - 90
        );
    }

    private void ShowInteractionPrompt()
    {
        // Hiển thị thông báo tương tác (nếu có UI, bạn có thể thêm Text ở đây)
        Debug.Log("Press " + interactionKey + " to interact with " + name);
    }

    private void HideInteractionPrompt()
    {
        // Ẩn thông báo tương tác
        Debug.Log("Out of range: " + name);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
