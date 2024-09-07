using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public static class TextUtilities 
    {
        private static List<TextMeshProUGUI> textsToStop = new List<TextMeshProUGUI>();


        #region Fade Image

        // Fade Text
        private static Dictionary<TextMeshProUGUI, Task> currentTextChangedFade = new();
        public static void UFadeText(this TextMeshProUGUI text, float duration, float fadeValue)
        {
            if (currentTextChangedFade.Keys.Contains(text))
            {
                textsToStop.Add(text);

                currentTextChangedFade[text] = UFadeTextAsync(text, duration, fadeValue);
            }
            else
            {
                currentTextChangedFade.Add(text, UFadeTextAsync(text, duration, fadeValue));
            }
        }

        private static async Task UFadeTextAsync(TextMeshProUGUI text, float duration, float fadeValue)
        {
            float timer = 0;
            float originalFade = text.color.a;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (textsToStop.Contains(text) && timer != 0)
                {
                    textsToStop.Remove(text);

                    return;
                }

                timer += Time.deltaTime;

                text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(originalFade, fadeValue, timer / duration));

                await Task.Yield();
            }

            text.color = new Color(text.color.r, text.color.g, text.color.b, fadeValue);

            await Task.Yield();

            currentTextChangedFade.Remove(text);
        }

        #endregion
    }
}
