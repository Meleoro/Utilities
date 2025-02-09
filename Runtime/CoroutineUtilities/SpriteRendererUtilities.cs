using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Utilities
{
    public static class SpriteRendererUtilities
    {

        #region Fade Sprite Renderer

        public static void UStopSpriteRendererFade(SpriteRenderer spriteRenderer)
        {
            if (currentFadedSpritesRenderers.ContainsKey(spriteRenderer))
            {
                fadesToStop.Add(spriteRenderer);
                currentFadedSpritesRenderers.Remove(spriteRenderer);
            }
        }

        private static List<SpriteRenderer> fadesToStop = new List<SpriteRenderer>();
        private static Dictionary<SpriteRenderer, Task> currentFadedSpritesRenderers = new();
        public static void UFadeSpriteRenderer(this SpriteRenderer currentSpriteRenderer, float duration, float newFadeValue, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if(duration == 0)
            {
                currentSpriteRenderer.color = new Color(currentSpriteRenderer.color.r, currentSpriteRenderer.color.g, currentSpriteRenderer.color.b, newFadeValue);
                return;
            }

            if (currentFadedSpritesRenderers.Keys.Contains(currentSpriteRenderer))
            {
                fadesToStop.Add(currentSpriteRenderer);

                currentFadedSpritesRenderers[currentSpriteRenderer] = UFadeSpriteRendererCoroutine(currentSpriteRenderer, duration, newFadeValue, curve, unscaledTime);
            }
            else
            {
                currentFadedSpritesRenderers.Add(currentSpriteRenderer, UFadeSpriteRendererCoroutine(currentSpriteRenderer, duration, newFadeValue, curve, unscaledTime));
            }
        }

        private static async Task UFadeSpriteRendererCoroutine(this SpriteRenderer currentSpriteRenderer, float duration, float newFadeValue, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            float timer = 0;
            Color originalColor = currentSpriteRenderer.color;
            Color endColor = new Color(currentSpriteRenderer.color.r, currentSpriteRenderer.color.g, currentSpriteRenderer.color.b, newFadeValue);

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (fadesToStop.Contains(currentSpriteRenderer) && timer != 0)
                {
                    fadesToStop.Remove(currentSpriteRenderer);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                currentSpriteRenderer.color = Color.Lerp(originalColor, endColor, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            if (fadesToStop.Contains(currentSpriteRenderer) && timer != 0)
            {
                fadesToStop.Remove(currentSpriteRenderer);

                return;
            }

            currentSpriteRenderer.color = endColor;
            currentFadedSpritesRenderers.Remove(currentSpriteRenderer); 
        }

        #endregion


        #region Lerp Color Renderer

        public static void UStopSpriteRendererLerpColor(SpriteRenderer spriteRenderer)
        {
            if (currentLerpedColorSpritesRenderers.ContainsKey(spriteRenderer))
            {
                lerpsToStop.Add(spriteRenderer);
                currentLerpedColorSpritesRenderers.Remove(spriteRenderer);
            }
        }

        private static List<SpriteRenderer> lerpsToStop = new List<SpriteRenderer>();
        private static Dictionary<SpriteRenderer, Task> currentLerpedColorSpritesRenderers = new();
        public static void ULerpColorSpriteRenderer(this SpriteRenderer spriteRenderer, float duration, Color newColor, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if (duration == 0)
            {
                spriteRenderer.color = newColor;
                return;
            }

            if (currentLerpedColorSpritesRenderers.Keys.Contains(spriteRenderer))
            {
                lerpsToStop.Add(spriteRenderer);

                currentLerpedColorSpritesRenderers[spriteRenderer] = ULerpColorSpriteRendererAsync(spriteRenderer, duration, newColor, curve, unscaledTime);
            }
            else
            {
                currentLerpedColorSpritesRenderers.Add(spriteRenderer, ULerpColorSpriteRendererAsync(spriteRenderer, duration, newColor, curve, unscaledTime));
            }
        }

        private static async Task ULerpColorSpriteRendererAsync(this SpriteRenderer spriteRenderer, float duration, Color newColor, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Color originalColor = spriteRenderer.color;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (lerpsToStop.Contains(spriteRenderer) && timer != 0)
                {
                    lerpsToStop.Remove(spriteRenderer);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                spriteRenderer.color = Color.Lerp(originalColor, newColor, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            if (fadesToStop.Contains(spriteRenderer) && timer != 0)
            {
                fadesToStop.Remove(spriteRenderer);

                return;
            }

            spriteRenderer.color = newColor;
            currentLerpedColorSpritesRenderers.Remove(spriteRenderer);
        }

        #endregion


        #region Bounce Color Renderer

        public static void UStopSpriteRendererBounceColor(SpriteRenderer spriteRenderer)
        {
            if (currentBouncedSpriteRenderers.ContainsKey(spriteRenderer))
            {
                bouncesToStop.Add(spriteRenderer);
                currentBouncedSpriteRenderers.Remove(spriteRenderer);
            }
        }

        private static List<SpriteRenderer> bouncesToStop = new List<SpriteRenderer>();
        private static Dictionary<SpriteRenderer, Task> currentBouncedSpriteRenderers = new();
        public static void UBounceColorSpriteRenderer(this SpriteRenderer spriteRenderer, float duration1, Color color1, float duration2, Color color2, CurveType curve = CurveType.None, bool loop = false, bool unscaledTime = false)
        {
            if (currentBouncedSpriteRenderers.Keys.Contains(spriteRenderer))
            {
                bouncesToStop.Add(spriteRenderer);

                currentBouncedSpriteRenderers[spriteRenderer] = UBounceColorSpriteRendererAsync(spriteRenderer, duration1, color1, duration2, color2, curve, loop, unscaledTime);
            }
            else
            {
                currentBouncedSpriteRenderers.Add(spriteRenderer, UBounceColorSpriteRendererAsync(spriteRenderer, duration1, color1, duration2, color2, curve, loop, unscaledTime));
            }
        }

        private static async Task UBounceColorSpriteRendererAsync(this SpriteRenderer spriteRenderer, float duration1, Color color1, float duration2, Color color2, CurveType curve, bool loop, bool unscaledTime)
        {
            bool firstLoop = true;

            while(firstLoop || loop)
            {
                float timer = 0;
                Color originalColor = spriteRenderer.color;

                while (timer < duration1)
                {
                    if (!Application.isPlaying) return;
                    if (bouncesToStop.Contains(spriteRenderer) && timer != 0)
                    {
                        bouncesToStop.Remove(spriteRenderer);

                        return;
                    }

                    timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                    spriteRenderer.color = Color.Lerp(originalColor, color1, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration1));

                    await Task.Yield();
                }

                if (!Application.isPlaying) return;
                if (bouncesToStop.Contains(spriteRenderer) && timer != 0)
                {
                    bouncesToStop.Remove(spriteRenderer);

                    return;
                }

                timer = 0;
                spriteRenderer.color = color1;

                while (timer < duration1)
                {
                    if (!Application.isPlaying) return;
                    if (bouncesToStop.Contains(spriteRenderer) && timer != 0)
                    {
                        bouncesToStop.Remove(spriteRenderer);

                        return;
                    }

                    timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                    spriteRenderer.color = Color.Lerp(color1, color2, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration1));

                    await Task.Yield();
                }

                if (!Application.isPlaying) return;
                if (bouncesToStop.Contains(spriteRenderer) && timer != 0)
                {
                    bouncesToStop.Remove(spriteRenderer);

                    return;
                }

                spriteRenderer.color = color2;
                firstLoop = false;
            }

            currentBouncedSpriteRenderers.Remove(spriteRenderer);
        }

        #endregion
    }
}
