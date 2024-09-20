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
        public static void UFadeSpriteRenderer(this SpriteRenderer currentSpriteRenderer, float duration, float newFadeValue)
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

        private static async Task UFadeSpriteRendererCoroutine(this SpriteRenderer currentSpriteRenderer, float duration, float newFadeValue)
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

        #region Lerp Color Renderer

        private static Dictionary<SpriteRenderer, Task> currentLerpedColorSpritesRenderers = new();
        public static void ULerpColorSpriteRenderer(this SpriteRenderer currentSpriteRenderer, float duration, Color newColor)
        {
            if (currentLerpedColorSpritesRenderers.Keys.Contains(currentSpriteRenderer))
            {
                spriteRenderersToStop.Add(currentSpriteRenderer);

                currentLerpedColorSpritesRenderers[currentSpriteRenderer] = ULerpColorSpriteRendererAsync(currentSpriteRenderer, duration, newColor);
            }
            else
            {
                currentLerpedColorSpritesRenderers.Add(currentSpriteRenderer, ULerpColorSpriteRendererAsync(currentSpriteRenderer, duration, newColor));
            }
        }

        private static async Task ULerpColorSpriteRendererAsync(this SpriteRenderer currentSpriteRenderer, float duration, Color newColor)
        {
            float timer = 0;
            Color originalColor = currentSpriteRenderer.color;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (spriteRenderersToStop.Contains(currentSpriteRenderer) && timer != 0)
                {
                    spriteRenderersToStop.Remove(currentSpriteRenderer);

                    return;
                }

                timer += Time.deltaTime;

                currentSpriteRenderer.color = Color.Lerp(originalColor, newColor, timer / duration);

                await Task.Yield();
            }

            currentSpriteRenderer.color = newColor;

            await Task.Yield();

            currentFadedSpritesRenderers.Remove(currentSpriteRenderer);
        }

        #endregion
    }
}
