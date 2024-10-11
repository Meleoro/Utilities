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

        #region Glitch Text

        private static Dictionary<TextMeshProUGUI, Task> currentGlitchedText = new();
        public static void UGlitchTextLerp(this TextMeshProUGUI text, float duration, float endValue, float startValue = 0)
        {
            if (currentGlitchedText.Keys.Contains(text))
            {
                textsToStop.Add(text);

                currentGlitchedText[text] = UGlitchTextLerpAsync(text, duration, endValue, startValue);
            }
            else
            {
                currentGlitchedText.Add(text, UGlitchTextLerpAsync(text, duration, endValue, startValue));
            }
        }

        private static async Task UGlitchTextLerpAsync(TextMeshProUGUI text, float duration, float endValue, float startValue = 0)
        {
            float timer = 0;


            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (textsToStop.Contains(text) && timer != 0)
                {
                    textsToStop.Remove(text);

                    return;
                }

                timer += Time.deltaTime;

                GlitchText(Mathf.Lerp(startValue, endValue, timer / duration), text);

                await Task.Yield();
            }

            GlitchText(endValue, text);

            await Task.Yield();

            currentGlitchedText.Remove(text);
        }

        private static void GlitchText(float intensity, TextMeshProUGUI text)
        {
            text.ForceMeshUpdate();

            TMP_TextInfo textInfo = text.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                Vector3[] verticies = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

                for (int j = 0; j < 4; j++)
                {
                    Vector3 originalPos = verticies[charInfo.vertexIndex + j];
                    verticies[charInfo.vertexIndex + j] = originalPos + new Vector3(Mathf.PerlinNoise(Time.time * Random.Range(0, 10), Time.time * Random.Range(0, 10)) * intensity,
                        Mathf.PerlinNoise(Time.time * Random.Range(0, 10), Time.time * Random.Range(0, 10)) * intensity * 0.1f, 0);

                }
                textInfo.meshInfo[charInfo.materialReferenceIndex].vertices = verticies;
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                text.UpdateGeometry(meshInfo.mesh, i);
            }
        }

        #endregion
    }
}
