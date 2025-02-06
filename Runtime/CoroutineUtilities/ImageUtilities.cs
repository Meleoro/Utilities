using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public static class ImageUtilities 
    {

        #region Fade Image

        public static void UStopFadeImage(this Image image)
        {
            if (currentChangedFade.Keys.Contains(image))
            {
                fadesToStop.Add(image);
                currentChangedFade.Remove(image);
            }
        }

        private static List<Image> fadesToStop = new List<Image>();
        private static Dictionary<Image, Task> currentChangedFade = new();
        public static void UFadeImage(this Image image, float duration, float fadeValue, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if(duration == 0)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, fadeValue);
                return;
            }

            if (currentChangedFade.Keys.Contains(image))
            {
                fadesToStop.Add(image);

                currentChangedFade[image] = UFadeImageAsync(image, duration, fadeValue, curve, unscaledTime);
            }
            else
            {
                currentChangedFade.Add(image, UFadeImageAsync(image, duration, fadeValue, curve, unscaledTime));
            }
        }

        private static async Task UFadeImageAsync(Image image, float duration, float fadeValue, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            float originalFade = image.color.a;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (fadesToStop.Contains(image) && timer != 0)
                {
                    fadesToStop.Remove(image);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(originalFade, fadeValue, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration)));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            image.color = new Color(image.color.r, image.color.g, image.color.b, fadeValue);
            currentChangedFade.Remove(image);
        }

        #endregion


        #region Lerp Image Color

        public static void UStopLerpImageColor(this Image image)
        {
            if (currentLerpedColor.Keys.Contains(image))
            {
                lerpedColorsToStop.Add(image);
                currentLerpedColor.Remove(image);
            }
        }

        private static List<Image> lerpedColorsToStop = new List<Image>();
        private static Dictionary<Image, Task> currentLerpedColor = new();
        public static void ULerpImageColor(this Image image, float duration, Color endColor, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if (duration == 0)
            {
                image.color = endColor;
                return;
            }

            if (currentLerpedColor.Keys.Contains(image))
            {
                lerpedColorsToStop.Add(image);

                currentLerpedColor[image] = ULerpImageColorAsync(image, duration, endColor, curve, unscaledTime);
            }
            else
            {
                currentLerpedColor.Add(image, ULerpImageColorAsync(image, duration, endColor, curve, unscaledTime));
            }
        }

        private static async Task ULerpImageColorAsync(Image image, float duration, Color endColor, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Color originalColor = image.color;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (lerpedColorsToStop.Contains(image) && timer != 0)
                {
                    lerpedColorsToStop.Remove(image);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                image.color = Color.Lerp(originalColor, endColor, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            image.color = endColor;
            currentLerpedColor.Remove(image);
        }

        #endregion


        #region Bounce Image Color

        public static void UStopBounceImageColor(this Image image)
        {
            if (currentBouncedColors.Keys.Contains(image))
            {
                bouncedColorsToStop.Add(image);
                currentBouncedColors.Remove(image);
            }
        }


        private static List<Image> bouncedColorsToStop = new List<Image>();
        private static Dictionary<Image, Task> currentBouncedColors = new();
        public static void UBounceImageColor(this Image image, float duration1, Color endColor1, float duration2, Color endColor2, CurveType curve = CurveType.None, bool loop = false, bool unscaledTime = false)
        {
            if (currentBouncedColors.Keys.Contains(image))
            {
                bouncedColorsToStop.Add(image);

                currentBouncedColors[image] = UBounceImageColorAsync(image, duration1, endColor1, duration2, endColor2, curve, loop, unscaledTime);
            }
            else
            {
                currentBouncedColors.Add(image, UBounceImageColorAsync(image, duration1, endColor1, duration2, endColor2, curve, loop, unscaledTime));
            }
        }

        private static async Task UBounceImageColorAsync(Image image, float duration1, Color endColor1, float duration2, Color endColor2, CurveType curve, bool loop, bool unscaledTime)
        {
            bool firstIteration = true;
            
            while (loop || firstIteration)
            {
                float timer = 0;
                Color originalColor = image.color;
                while (timer < duration1)
                {
                    if (!Application.isPlaying) return;
                    if (bouncedColorsToStop.Contains(image) && timer != 0)
                    {
                        bouncedColorsToStop.Remove(image);

                        return;
                    }

                    timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                    image.color = Color.Lerp(originalColor, endColor1, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration1));

                    await Task.Yield();
                }

                if (!Application.isPlaying) return;
                image.color = endColor1;
                timer = 0;

                await Task.Yield();

                while (timer < duration2)
                {
                    if (!Application.isPlaying) return;
                    if (bouncedColorsToStop.Contains(image) && timer != 0)
                    {
                        bouncedColorsToStop.Remove(image);

                        return;
                    }

                    timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                    image.color = Color.Lerp(endColor1, endColor2, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration2));

                    await Task.Yield();
                }

                if (!Application.isPlaying) return;
                image.color = endColor2;

                firstIteration = false;
            }

            currentBouncedColors.Remove(image);
        }

        #endregion
    }
}
