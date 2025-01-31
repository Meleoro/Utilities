using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;


namespace Utilities
{
    public static class VolumeUtilities 
    {
        private static List<Volume> volumesToStop = new List<Volume>();


        #region Lerp Volume

        // Lerp Volume Weight
        private static Dictionary<Volume, Task> currentLerpedVolumeWeight = new();
        public static void ULerpWeight(this Volume volume, float duration, float endValue, bool unscaledTime = false)
        {
            if(duration == 0)
            {
                volume.weight = endValue;
                return;
            }

            if (currentLerpedVolumeWeight.Keys.Contains(volume))
            {
                volumesToStop.Add(volume);

                currentLerpedVolumeWeight[volume] = ULerpWeightAsync(volume, duration, endValue, unscaledTime);
            }
            else
            {
                currentLerpedVolumeWeight.Add(volume, ULerpWeightAsync(volume, duration, endValue, unscaledTime));
            }
        }

        private static async Task ULerpWeightAsync(Volume volume, float duration, float endValue, bool unscaledTime)
        {
            float timer = 0;
            float originalWeight = volume.weight;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (volumesToStop.Contains(volume) && timer != 0)
                {
                    volumesToStop.Remove(volume);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                volume.weight = Mathf.Lerp(originalWeight, endValue, timer / duration);

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            volume.weight = endValue;
            currentLerpedVolumeWeight.Remove(volume);
        }

        #endregion


        #region Flash Volume

        // Lerp Volume Weight
        private static Dictionary<Volume, Task> currentFlashedVolumeWeight = new();
        public static void UFlashWeight(this Volume volume, float duration1, float flashValue, float duration2, float endValue, bool unscaledTime = false)
        {
            if (currentFlashedVolumeWeight.Keys.Contains(volume))
            {
                volumesToStop.Add(volume);

                currentFlashedVolumeWeight[volume] = UFlashWeightAsync(volume, duration1, flashValue, duration2, endValue, unscaledTime);
            }
            else
            {
                currentFlashedVolumeWeight.Add(volume, UFlashWeightAsync(volume, duration1, flashValue, duration2, endValue, unscaledTime));
            }
        }

        private static async Task UFlashWeightAsync(Volume volume, float duration1, float flashValue, float duration2, float endValue, bool unscaledTime)
        {
            float timer = 0;
            float originalWeight = volume.weight;

            while (timer < duration1)
            {
                if (!Application.isPlaying) return;
                if (volumesToStop.Contains(volume) && timer != 0)
                {
                    volumesToStop.Remove(volume);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                volume.weight = Mathf.Lerp(originalWeight, flashValue, timer / duration1);

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            timer = 0;
            volume.weight = flashValue;

            while (timer < duration2)
            {
                if (!Application.isPlaying) return;
                if (volumesToStop.Contains(volume))
                {
                    volumesToStop.Remove(volume);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                volume.weight = Mathf.Lerp(flashValue, endValue, timer / duration2);

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            volume.weight = endValue;
            currentFlashedVolumeWeight.Remove(volume);
        }

        #endregion
    }
}
