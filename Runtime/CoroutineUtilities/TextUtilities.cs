using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer.Explorer;
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

        #region Fade Text

        public static void UStopFadeText(this TextMeshProUGUI text)
        {
            if (currentTextChangedFade.Keys.Contains(text))
            {
                fadedTextToStop.Add(text);
            }
        }

        private static List<TextMeshProUGUI> fadedTextToStop = new List<TextMeshProUGUI>();
        private static Dictionary<TextMeshProUGUI, Task> currentTextChangedFade = new();
        public static void UFadeText(this TextMeshProUGUI text, float duration, float fadeValue, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if(duration == 0)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, fadeValue);
                return;
            }

            if (currentTextChangedFade.Keys.Contains(text))
            {
                fadedTextToStop.Add(text);

                currentTextChangedFade[text] = UFadeTextAsync(text, duration, fadeValue, curve, unscaledTime);
            }
            else
            {
                currentTextChangedFade.Add(text, UFadeTextAsync(text, duration, fadeValue, curve, unscaledTime));
            }
        }

        private static async Task UFadeTextAsync(TextMeshProUGUI text, float duration, float fadeValue, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            float originalFade = text.color.a;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (fadedTextToStop.Contains(text) && timer != 0)
                {
                    fadedTextToStop.Remove(text);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(originalFade, fadeValue, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration)));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            text.color = new Color(text.color.r, text.color.g, text.color.b, fadeValue);
            currentTextChangedFade.Remove(text);
        }

        #endregion


        #region Lerp Text Color

        public static void UStopLerpTextColor(this TextMeshProUGUI text)
        {
            if (currentLerpedTextColors.Keys.Contains(text))
            {
                lerpedColorTextsToStop.Add(text);
            }
        }

        private static List<TextMeshProUGUI> lerpedColorTextsToStop = new List<TextMeshProUGUI>();
        private static Dictionary<TextMeshProUGUI, Task> currentLerpedTextColors = new();
        public static void ULerpTextColor(this TextMeshProUGUI text, float duration, Color finalColor, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if (duration == 0)
            {
                text.color = finalColor;
                return;
            }

            if (currentLerpedTextColors.Keys.Contains(text))
            {
                lerpedColorTextsToStop.Add(text);

                currentLerpedTextColors[text] = ULerpTextColorAsync(text, duration, finalColor, curve, unscaledTime);
            }
            else
            {
                currentLerpedTextColors.Add(text, ULerpTextColorAsync(text, duration, finalColor, curve, unscaledTime));
            }
        }

        private static async Task ULerpTextColorAsync(TextMeshProUGUI text, float duration, Color finalColor, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Color originalColor = text.color;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (lerpedColorTextsToStop.Contains(text) && timer != 0)
                {
                    lerpedColorTextsToStop.Remove(text);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                text.color = Color.Lerp(originalColor, finalColor, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            text.color = finalColor;
            currentLerpedTextColors.Remove(text);
        }

        #endregion


        #region Bounce Text Color

        public static void UStopBouncedTextColor(this TextMeshProUGUI text)
        {
            if (currentBouncedTextColors.Keys.Contains(text))
            {
                bouncedTextColorsToStop.Add(text);
            }
        }

        private static List<TextMeshProUGUI> bouncedTextColorsToStop = new List<TextMeshProUGUI>();
        private static Dictionary<TextMeshProUGUI, Task> currentBouncedTextColors = new();
        public static void UBounceTextColor(this TextMeshProUGUI text, float duration1, Color finalColor1, float duration2, Color finalColor2, bool loop = false, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if (currentBouncedTextColors.Keys.Contains(text))
            {
                bouncedTextColorsToStop.Add(text);

                currentBouncedTextColors[text] = UBounceTextColorAsync(text, duration1, finalColor1, duration2, finalColor2, loop, curve, unscaledTime);
            }
            else
            {
                currentBouncedTextColors.Add(text, UBounceTextColorAsync(text, duration1, finalColor1, duration2, finalColor2, loop, curve, unscaledTime));
            }
        }

        private static async Task UBounceTextColorAsync(TextMeshProUGUI text, float duration1, Color finalColor1, float duration2, Color finalColor2, bool loop, CurveType curve, bool unscaledTime)
        {
            bool firstIteration = true;
            
            while (loop || firstIteration)
            {
                float timer = 0;
                Color originalColor = text.color;

                while (timer < duration1)
                {
                    if (!Application.isPlaying) return;
                    if (bouncedTextColorsToStop.Contains(text) && timer != 0)
                    {
                        bouncedTextColorsToStop.Remove(text);

                        return;
                    }

                    timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                    text.color = Color.Lerp(originalColor, finalColor1, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration1));

                    await Task.Yield();
                }

                timer = 0;
                text.color = finalColor1;

                while (timer < duration2)
                {
                    if (!Application.isPlaying) return;
                    if (bouncedTextColorsToStop.Contains(text) && timer != 0)
                    {
                        bouncedTextColorsToStop.Remove(text);

                        return;
                    }

                    timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                    text.color = Color.Lerp(finalColor1, finalColor2, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration2));

                    await Task.Yield();
                }

                if (!Application.isPlaying) return;
                text.color = finalColor2;

                firstIteration = false;
            }
            
            currentBouncedTextColors.Remove(text);
        }

        #endregion


        #region Glitch Text

        private static List<TextMeshProUGUI> glitchedTextsToStop = new();
        private static Dictionary<TextMeshProUGUI, Task> currentGlitchedText = new();
        public static void UGlitchTextLerp(this TextMeshProUGUI text, float duration, float endValue, float startValue = 0)
        {
            if (currentGlitchedText.Keys.Contains(text))
            {
                glitchedTextsToStop.Add(text);

                currentGlitchedText[text] = UGlitchTextLerpAsync(text, duration, endValue, startValue);
            }
            else
            {
                currentGlitchedText.Add(text, UGlitchTextLerpAsync(text, duration, endValue, startValue));
            }
        }

        private static async Task UGlitchTextLerpAsync(TextMeshProUGUI text, float duration, float endValue, float startValue)
        {
            float timer = 0;


            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (glitchedTextsToStop.Contains(text) && timer != 0)
                {
                    glitchedTextsToStop.Remove(text);

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
                    verticies[charInfo.vertexIndex + j] = originalPos + new Vector3(Mathf.PerlinNoise(Time.time * Random.Range(-10, 10), Time.time * Random.Range(-10, 10)) * intensity - intensity * 0.5f,
                        Mathf.PerlinNoise(Time.time * Random.Range(-10, 10), Time.time * Random.Range(-10, 10)) * intensity * 0.1f, 0);

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
