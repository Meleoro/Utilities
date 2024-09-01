using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Utilities
{
    public static class TrUtilities
    {
        private static List<Transform> transformToStop = new List<Transform>();

        #region Shake

        // Shake position
        private static Dictionary<Transform, Task> currentShakePositions = new();
        public static void UShakePosition(this Transform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
        {
            if (currentShakePositions.Keys.Contains(tr))
            {
                currentShakePositions[tr].Dispose();
                currentShakePositions[tr] = UShakePositionAsync(tr, duration, intensity, lockX, lockY, lockZ);
            }
            else
            {
                currentShakePositions.Add(tr, UShakePositionAsync(tr, duration, intensity, lockX, lockY, lockZ));
            }
        }

        private static async Task UShakePositionAsync(Transform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
        {
            float timer = 0;
            float startIntensity = intensity;
            Vector3 originalPos = tr.position;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;

                timer += Time.deltaTime;

                intensity = Mathf.Lerp(startIntensity, 0, timer / duration);
                tr.position = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));

                if (lockX) tr.position = new Vector3(originalPos.x, tr.position.y, tr.position.z);
                if (lockY) tr.position = new Vector3(tr.position.x, originalPos.y, tr.position.z);
                if (lockZ) tr.position = new Vector3(tr.position.x, tr.position.y, originalPos.z);

                await Task.Yield();
            }

            tr.position = originalPos;
        }


        // Shake position with rect transform
        private static Dictionary<RectTransform, Task> currentRectShakePositions = new();
        public static void UShakePosition(this RectTransform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
        {
            if (currentRectShakePositions.Keys.Contains(tr))
            {
                currentRectShakePositions[tr].Dispose();
                currentRectShakePositions[tr] = UShakePositionAsync(tr, duration, intensity, lockX, lockY, lockZ);
            }
            else
            {
                currentRectShakePositions.Add(tr, UShakePositionAsync(tr, duration, intensity, lockX, lockY, lockZ));
            }
        }

        private static async Task UShakePositionAsync(RectTransform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
        {
            float timer = 0;
            float startIntensity = intensity;
            Vector3 originalPos = tr.position;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;

                timer += Time.deltaTime;

                intensity = Mathf.Lerp(startIntensity, 0, timer / duration);
                tr.position = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));

                if (lockX) tr.position = new Vector3(originalPos.x, tr.position.y, tr.position.z);
                if (lockY) tr.position = new Vector3(tr.position.x, originalPos.y, tr.position.z);
                if (lockZ) tr.position = new Vector3(tr.position.x, tr.position.y, originalPos.z);

                await Task.Yield();
            }

            tr.position = originalPos;
        }

        #endregion


        #region Position

        // Change position
        private static Dictionary<Transform, Task> currentChangedPos = new();
        public static void UChangePosition(this Transform tr, float duration, Vector3 newPos)
        {
            if (currentChangedPos.Keys.Contains(tr))
            {
                currentChangedPos[tr].Dispose();
                currentChangedPos[tr] = UChangePositionAsync(tr, duration, newPos);
            }
            else
            {
                currentChangedPos.Add(tr, UChangePositionAsync(tr, duration, newPos));
            }
        }

        private static async Task UChangePositionAsync(Transform tr, float duration, Vector3 newPos)
        {
            float timer = 0;
            Vector3 originalPos = tr.position;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;

                timer += Time.deltaTime;

                tr.position = Vector3.Lerp(originalPos, newPos, timer / duration);

                await Task.Yield();
            }

            tr.position = newPos;
        }


        // Change position with rect transform
        private static Dictionary<RectTransform, Task> currentRectChangedPos = new();
        public static void UChangePosition(this RectTransform tr, float duration, Vector3 newPos)
        {
            if (currentRectChangedPos.Keys.Contains(tr))
            {
                currentRectChangedPos[tr].Dispose();
                currentRectChangedPos[tr] = UChangePositionAsync(tr, duration, newPos);
            }
            else
            {
                currentRectChangedPos.Add(tr, UChangePositionAsync(tr, duration, newPos));
            }
        }

        private static async Task UChangePositionAsync(RectTransform tr, float duration, Vector3 newPos)
        {
            float timer = 0;
            Vector3 originalPos = tr.position;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;

                timer += Time.deltaTime;

                tr.position = Vector3.Lerp(originalPos, newPos, timer / duration);

                await Task.Yield();
            }

            tr.position = newPos;
        }

        #endregion


        #region Rotation

        // Change rotation
        private static Dictionary<Transform, Task> currentChangedRot = new();
        public static void UChangeRotation(this Transform tr, float duration, Quaternion newRot)
        {
            if (currentChangedRot.Keys.Contains(tr))
            {
                currentChangedRot[tr].Dispose();
                currentChangedRot[tr] = UChangeRotationAsync(tr, duration, newRot);
            }
            else
            {
                currentChangedRot.Add(tr, UChangeRotationAsync(tr, duration, newRot));
            }
        }

        private static async Task UChangeRotationAsync(Transform tr, float duration, Quaternion newRot)
        {
            float timer = 0;
            Quaternion originalRot = tr.rotation;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;

                timer += Time.deltaTime;

                tr.rotation = Quaternion.Lerp(originalRot, newRot, timer / duration);

                await Task.Yield();
            }

            tr.rotation = newRot;
        }


        // Change rotation with rect transform
        private static Dictionary<RectTransform, Task> currentRectChangedRot = new();
        public static void UChangeRotation(this RectTransform tr, float duration, Quaternion newRot)
        {
            if (currentRectChangedRot.Keys.Contains(tr))
            {
                currentRectChangedRot[tr].Dispose();
                currentRectChangedRot[tr] = UChangeRotationAsync(tr, duration, newRot);
            }
            else
            {
                currentRectChangedRot.Add(tr, UChangeRotationAsync(tr, duration, newRot));
            }
        }

        private static async Task UChangeRotationAsync(RectTransform tr, float duration, Quaternion newRot)
        {
            float timer = 0;
            Quaternion originalRot = tr.rotation;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;

                timer += Time.deltaTime;

                tr.rotation = Quaternion.Lerp(originalRot, newRot, timer / duration);

                await Task.Yield();
            }

            tr.rotation = newRot;
        }

        #endregion


        #region Scale

        // Change rotation
        private static Dictionary<Transform, Task> currentChangedScale = new();
        public static void UChangeScale(this Transform tr, float duration, Vector3 newSize)
        {
            if (currentChangedScale.Keys.Contains(tr))
            {
                transformToStop.Add(tr);

                currentChangedScale[tr] = UChangeScaleAsync(tr, duration, newSize);
            }
            else
            {
                currentChangedScale.Add(tr, UChangeScaleAsync(tr, duration, newSize));
            }
        }

        private static async Task UChangeScaleAsync(Transform tr, float duration, Vector3 newSize)
        {
            float timer = 0;
            Vector3 originalScale = tr.localScale;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (transformToStop.Contains(tr) && timer != 0)
                {
                    transformToStop.Remove(tr);

                    return;
                }

                timer += Time.deltaTime;

                tr.localScale = Vector3.Lerp(originalScale, newSize, timer / duration);

                await Task.Yield();
            }

            tr.localScale = newSize;

            currentChangedScale.Remove(tr);
        }


        // Change rotation with rect transform
        private static Dictionary<RectTransform, Task> currentRectChangedScale = new();
        public static void UChangeScale(this RectTransform tr, float duration, Vector3 newSize)
        {
            if (currentRectChangedScale.Keys.Contains(tr))
            {
                currentRectChangedScale[tr].Dispose();
                currentRectChangedScale[tr] = UChangeScaleAsync(tr, duration, newSize);
            }
            else
            {
                currentRectChangedScale.Add(tr, UChangeScaleAsync(tr, duration, newSize));
            }
        }

        private static async Task UChangeScaleAsync(RectTransform tr, float duration, Vector3 newSize)
        {
            float timer = 0;
            Vector3 originalScale = tr.localScale;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;

                timer += Time.deltaTime;

                tr.localScale = Vector3.Lerp(originalScale, newSize, timer / duration);

                await Task.Yield();
            }

            tr.localScale = newSize;
        }

        #endregion
    }
}
