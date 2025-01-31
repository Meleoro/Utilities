using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;

namespace Utilities
{
    public static class LightUtilities 
    {
        private static List<Light> lightsToStop = new List<Light>();
        private static List<Light2D> lights2DToStop = new List<Light2D>();


        #region Lerp Light

        private static Dictionary<Light, Task> currentLerpedLights = new();
        public static void ULerpIntensity(this Light light, float duration, float newIntensity, AnimationCurve curve = null, bool unscaledTime = false)
        {
            if(duration == 0)
            {
                light.intensity = newIntensity;
                return;
            }

            if (currentLerpedLights.Keys.Contains(light))
            {
                lightsToStop.Add(light);
                currentLerpedLights[light] = LerpIntensityAsync(light, duration, newIntensity, curve, unscaledTime);
            }
            else
            {
                currentLerpedLights.Add(light, LerpIntensityAsync(light, duration, newIntensity, curve, unscaledTime));
            }
        }

        private static async Task LerpIntensityAsync(Light light, float duration, float newIntensity, AnimationCurve curve, bool unscaledTime)
        {
            float timer = 0;
            float startIntensity = light.intensity;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (lightsToStop.Contains(light) && timer != 0)
                {
                    lightsToStop.Remove(light);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                if(curve is null)
                    light.intensity = Mathf.Lerp(startIntensity, newIntensity, timer / duration);

                else
                    light.intensity = Mathf.Lerp(startIntensity, newIntensity, curve.Evaluate(timer / duration));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            light.intensity = newIntensity;
            currentLerpedLights.Remove(light);
        }

        #endregion


        #region Lerp Light 2D

        private static Dictionary<Light2D, Task> currentLerpedLights2D = new();
        public static void ULerpIntensity(this Light2D light, float duration, float newIntensity, AnimationCurve curve = null, bool unscaledTime = false)
        {
            if(duration == 0)
            {
                light.intensity = newIntensity;
                return;
            }

            if (currentLerpedLights2D.Keys.Contains(light))
            {
                lights2DToStop.Add(light);
                currentLerpedLights2D[light] = LerpIntensityAsync(light, duration, newIntensity, curve, unscaledTime);
            }
            else
            {
                currentLerpedLights2D.Add(light, LerpIntensityAsync(light, duration, newIntensity, curve, unscaledTime));
            }
        }

        private static async Task LerpIntensityAsync(Light2D light, float duration, float newIntensity, AnimationCurve curve, bool unscaledTime)
        {
            float timer = 0;
            float startIntensity = light.intensity;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (lights2DToStop.Contains(light) && timer != 0)
                {
                    lights2DToStop.Remove(light);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                if (curve is null)
                    light.intensity = Mathf.Lerp(startIntensity, newIntensity, timer / duration);

                else
                    light.intensity = Mathf.Lerp(startIntensity, newIntensity, curve.Evaluate(timer / duration));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            light.intensity = newIntensity;
            currentLerpedLights2D.Remove(light);
        }

        #endregion
    }
}
