using System.Collections;
using UnityEngine;

public class CorpseDestroy : MonoBehaviour
{
    private SpriteRenderer spriteCorpseRenderer;
    private float fadeDuration = 1.0f; // Thời gian để mờ dần (1 giây)
    private float lifetime = 3.0f; // Thời gian tồn tại trước khi bắt đầu mờ dần

    private void Start()
    {
        // Thiết lập `SpriteRenderer`
        spriteCorpseRenderer = GetComponent<SpriteRenderer>();
        spriteCorpseRenderer.sortingOrder = 1;

        // Bắt đầu Coroutine để xử lý việc biến mất dần dần
        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        // Chờ trong khoảng thời gian tồn tại trước khi mờ dần
        yield return new WaitForSeconds(lifetime);

        // Bắt đầu quá trình mờ dần
        float startAlpha = spriteCorpseRenderer.color.a;
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        // Dần dần giảm alpha (độ trong suốt) của sprite
        while (progress < 1.0f)
        {
            Color tmpColor = spriteCorpseRenderer.color;
            tmpColor.a = Mathf.Lerp(startAlpha, 0, progress);
            spriteCorpseRenderer.color = tmpColor;

            progress += rate * Time.deltaTime;
            yield return null;
        }

        // Đảm bảo alpha bằng 0 sau khi kết thúc mờ dần
        Color finalColor = spriteCorpseRenderer.color;
        finalColor.a = 0;
        spriteCorpseRenderer.color = finalColor;

        // Hủy đối tượng sau khi mờ dần
        Destroy(gameObject);
    }
}
