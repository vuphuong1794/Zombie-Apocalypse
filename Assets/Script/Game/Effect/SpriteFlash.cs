using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    //private SpriteRenderer _spriteRenderer;
    private SpriteRenderer[] _spriteRenderers;

    private void Awake()
    {
        // Lấy tất cả các SpriteRenderer của các bộ phận (cả trong đối tượng và con của nó)
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public IEnumerator FlashCoroutine(float flashDuration, Color flashColor, int numberOfFlashes)
    {
        //Color startColor = _spriteRenderer.color;

        // Lưu màu gốc của tất cả các sprite
        List<Color> startColors = new List<Color>();
        foreach (var spriteRenderer in _spriteRenderers)
        {
            startColors.Add(spriteRenderer.color);
        }

        float elapsedFlashTime = 0;
        float elapsedFlashPercentage = 0;

        while (elapsedFlashTime < flashDuration)
        {
            elapsedFlashTime += Time.deltaTime;
            elapsedFlashPercentage = elapsedFlashTime / flashDuration;

            if (elapsedFlashPercentage > 1)
            {
                elapsedFlashPercentage = 1;
            }

            // Nhận giá trị giữa 0 và 1
            // Nếu elapsedFlashPercentage * 2 * numberOfFlashes = 1.1-2 thì pingPongPercentage = 0.9-0
            float pingPongPercentage = Mathf.PingPong(elapsedFlashPercentage * 2 * numberOfFlashes, 1);
            //_spriteRenderer.color = Color.Lerp(startColor, flashColor, pingPongPercentage);

            // Cập nhật màu cho tất cả các SpriteRenderer
            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                _spriteRenderers[i].color = Color.Lerp(startColors[i], flashColor, pingPongPercentage);
            }

            yield return null;
        }
    }
}