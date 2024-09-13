using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Utilities
{
    public static class SpriteRendererUtilities
    {
        private static List<SpriteRenderer> spriteRenderersToStop = new List<SpriteRenderer>();  

        #region Fade Sprite Renderer

        private static Dictionary<SpriteRenderer, Task> currentFadedSpritesRenderers = new();
        public static void UFadeSpriteRenderer(SpriteRenderer currentSpriteRenderer, float duration, float newFadeValue)
        {
            if (currentFadedSpritesRenderers.Keys.Contains(currentSpriteRenderer))
            {
                spriteRenderersToStop.Add(currentSpriteRenderer);

                currentFadedSpritesRenderers[currentSpriteRenderer] = UFadeSpriteRendererCoroutine(currentSpriteRenderer, duration, newFadeValue);
            }
            else
            {
                currentFadedSpritesRenderers.Add(currentSpriteRenderer, UFadeSpriteRendererCoroutine(currentSpriteRenderer, duration, newFadeValue));
            }
        }

        private static async Task UFadeSpriteRendererCoroutine(SpriteRenderer currentSpriteRenderer, float duration, float newFadeValue)
        {
            float timer = 0;
            Color originalColor = currentSpriteRenderer.color;
            Color endColor = new Color(currentSpriteRenderer.color.r, currentSpriteRenderer.color.g, currentSpriteRenderer.color.b, newFadeValue);

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (spriteRenderersToStop.Contains(currentSpriteRenderer) && timer != 0)
                {
                    spriteRenderersToStop.Remove(currentSpriteRenderer);

                    return;
                }

                timer += Time.deltaTime;

                currentSpriteRenderer.color = Color.Lerp(originalColor, endColor, timer / duration);

                await Task.Yield();
            }

            currentSpriteRenderer.color = endColor;

            await Task.Yield();

            currentFadedSpritesRenderers.Remove(currentSpriteRenderer); 
        }

        #endregion
    }
}
