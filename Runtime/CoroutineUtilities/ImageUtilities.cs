using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public static class ImageUtilities 
    {
        private static List<Image> imagesToStop = new List<Image>();


        #region Fade Image

        // Fade Image
        private static Dictionary<Image, Task> currentChangedFade = new();
        public static void UFadeImage(this Image image, float duration, float fadeValue)
        {
            if (currentChangedFade.Keys.Contains(image))
            {
                imagesToStop.Add(image);

                currentChangedFade[image] = UFadeImageAsync(image, duration, fadeValue);
            }
            else
            {
                currentChangedFade.Add(image, UFadeImageAsync(image, duration, fadeValue));
            }
        }

        private static async Task UFadeImageAsync(Image image, float duration, float fadeValue)
        {
            float timer = 0;
            float originalFade = image.color.a;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (imagesToStop.Contains(image) && timer != 0)
                {
                    imagesToStop.Remove(image);

                    return;
                }

                timer += Time.deltaTime;

                image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(originalFade, fadeValue, timer / duration));

                await Task.Yield();
            }

            image.color = new Color(image.color.r, image.color.g, image.color.b, fadeValue);

            currentChangedFade.Remove(image);
        }

        #endregion
    }
}
