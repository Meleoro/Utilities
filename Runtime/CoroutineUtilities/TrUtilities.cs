using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;

namespace Utilities
{
    public static class TrUtilities
    {
        private static List<Transform> transformToStop = new List<Transform>();
        private static List<Transform> positionsToStop = new List<Transform>();
        private static List<Transform> scalesToStop = new List<Transform>();
        private static List<Transform> rotationsToStop = new List<Transform>();


        #region Shake

        // Shake position
        private static Dictionary<Transform, Task> currentShakePositions = new();
        public static void UShakePosition(this Transform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
        {
            if (currentShakePositions.Keys.Contains(tr))
            {
                transformToStop.Add(tr);

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
                if (transformToStop.Contains(tr) && timer != 0)
                {
                    transformToStop.Remove(tr);

                    return;
                }

                timer += Time.unscaledDeltaTime;

                intensity = Mathf.Lerp(startIntensity, 0, timer / duration);
                tr.position = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));

                if (lockX) tr.position = new Vector3(originalPos.x, tr.position.y, tr.position.z);
                if (lockY) tr.position = new Vector3(tr.position.x, originalPos.y, tr.position.z);
                if (lockZ) tr.position = new Vector3(tr.position.x, tr.position.y, originalPos.z);

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.position = originalPos;
            currentShakePositions.Remove(tr);
        }



        // Shake position with rect transform
        private static Dictionary<RectTransform, Task> currentRectShakePositions = new();
        public static void UShakePosition(this RectTransform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
        {
            if (currentRectShakePositions.Keys.Contains(tr))
            {
                transformToStop.Add(tr);

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
                if (transformToStop.Contains(tr) && timer != 0)
                {
                    transformToStop.Remove(tr);

                    return;
                }

                timer += Time.unscaledDeltaTime;

                intensity = Mathf.Lerp(startIntensity, 0, timer / duration);
                tr.position = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));

                if (lockX) tr.position = new Vector3(originalPos.x, tr.position.y, tr.position.z);
                if (lockY) tr.position = new Vector3(tr.position.x, originalPos.y, tr.position.z);
                if (lockZ) tr.position = new Vector3(tr.position.x, tr.position.y, originalPos.z);

                await Task.Yield();
            }

            tr.position = originalPos;

            await Task.Yield();

            currentRectShakePositions.Remove(tr);
        }

        #endregion


        #region Position

        // Change position
        private static Dictionary<Transform, Task> currentChangedPos = new();
        public static void UChangePosition(this Transform tr, float duration, Vector3 newPos, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if (duration == 0) 
            {
                tr.position = newPos;
                return;
            }

            if (currentChangedPos.Keys.Contains(tr))
            {
                if(!positionsToStop.Contains(tr))
                    positionsToStop.Add(tr);

                currentChangedPos[tr] = UChangePositionAsync(tr, duration, newPos, curve, unscaledTime);
            }
            else
            {
                currentChangedPos.Add(tr, UChangePositionAsync(tr, duration, newPos, curve, unscaledTime));
            }
        }

        private static async Task UChangePositionAsync(Transform tr, float duration, Vector3 newPos, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Vector3 originalPos = tr.position;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (positionsToStop.Contains(tr) && timer != 0)
                {
                    positionsToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                tr.position = Vector3.Lerp(originalPos, newPos, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.position = newPos;
            currentChangedPos.Remove(tr);
        }



        // Change position with rect transform
        private static Dictionary<RectTransform, Task> currentRectChangedPos = new();
        public static void UChangePosition(this RectTransform tr, float duration, Vector3 newPos, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if(duration == 0)
            {
                tr.position = newPos;
                return;
            }

            if (currentRectChangedPos.Keys.Contains(tr))
            {
                if (!positionsToStop.Contains(tr))
                    positionsToStop.Add(tr);

                currentRectChangedPos[tr] = UChangePositionAsync(tr, duration, newPos, curve, unscaledTime);
            }
            else
            {
                currentRectChangedPos.Add(tr, UChangePositionAsync(tr, duration, newPos, curve, unscaledTime));
            }
        }

        private static async Task UChangePositionAsync(RectTransform tr, float duration, Vector3 newPos, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Vector3 originalPos = tr.position;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (positionsToStop.Contains(tr) && timer != 0)
                {
                    positionsToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                tr.position = Vector3.Lerp(originalPos, newPos, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration));

                await Task.Yield();
            }

            tr.position = newPos;

            currentRectChangedPos.Remove(tr);
        }

        #endregion


        #region Rotation

        // Change rotation
        private static Dictionary<Transform, Task> currentChangedRot = new();
        public static void UChangeRotation(this Transform tr, float duration, Quaternion newRot, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if(duration == 0)
            {
                tr.rotation = newRot;
                return;
            }

            if (currentChangedRot.Keys.Contains(tr))
            {
                if (!rotationsToStop.Contains(tr))
                    rotationsToStop.Add(tr);

                currentChangedRot[tr] = UChangeRotationAsync(tr, duration, newRot, curve, unscaledTime);
            }
            else
            {
                currentChangedRot.Add(tr, UChangeRotationAsync(tr, duration, newRot, curve,unscaledTime));
            }
        }

        private static async Task UChangeRotationAsync(Transform tr, float duration, Quaternion newRot, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Quaternion originalRot = tr.rotation;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (rotationsToStop.Contains(tr) && timer != 0)
                {
                    rotationsToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                tr.rotation = Quaternion.Lerp(originalRot, newRot, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.rotation = newRot;
            currentChangedRot.Remove(tr);
        }



        // Change rotation with rect transform
        private static Dictionary<RectTransform, Task> currentRectChangedRot = new();
        public static void UChangeRotation(this RectTransform tr, float duration, Quaternion newRot, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if (duration == 0)
            {
                tr.rotation = newRot;
                return;
            }

            if (currentRectChangedRot.Keys.Contains(tr))
            {
                if (!rotationsToStop.Contains(tr))
                    rotationsToStop.Add(tr);

                currentRectChangedRot[tr] = UChangeRotationAsync(tr, duration, newRot, curve, unscaledTime);
            }
            else
            {
                currentRectChangedRot.Add(tr, UChangeRotationAsync(tr, duration, newRot, curve, unscaledTime));
            }
        }

        private static async Task UChangeRotationAsync(RectTransform tr, float duration, Quaternion newRot, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Quaternion originalRot = tr.rotation;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (rotationsToStop.Contains(tr) && timer != 0)
                {
                    rotationsToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                tr.rotation = Quaternion.Lerp(originalRot, newRot, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.rotation = newRot;
            currentRectChangedRot.Remove(tr);
        }

        #endregion


        #region Scale

        // Change scale
        private static Dictionary<Transform, Task> currentChangedScale = new();
        public static void UChangeScale(this Transform tr, float duration, Vector3 newSize, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if(duration == 0)
            {
                tr.localScale = newSize;
                return;
            }

            if (currentChangedScale.Keys.Contains(tr))
            {
                if(!scalesToStop.Contains(tr))
                    scalesToStop.Add(tr);

                currentChangedScale[tr] = UChangeScaleAsync(tr, duration, newSize, curve, unscaledTime);
            }
            else
            {
                currentChangedScale.Add(tr, UChangeScaleAsync(tr, duration, newSize, curve, unscaledTime));
            }
        }

        private static async Task UChangeScaleAsync(Transform tr, float duration, Vector3 newSize, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Vector3 originalScale = tr.localScale;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (scalesToStop.Contains(tr) && timer != 0)
                {
                    scalesToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                tr.localScale = Vector3.Lerp(originalScale, newSize, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.localScale = newSize;
            currentChangedScale.Remove(tr);
        }



        private static Dictionary<RectTransform, Task> currentRectChangedScale = new();
        public static void UChangeScale(this RectTransform tr, float duration, Vector3 newSize, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if (duration == 0)
            {
                tr.localScale = newSize;
                return;
            }

            if (currentRectChangedScale.Keys.Contains(tr))
            {
                if (!scalesToStop.Contains(tr))
                    scalesToStop.Add(tr);

                currentRectChangedScale[tr] = UChangeScaleAsync(tr, duration, newSize, curve, unscaledTime);
            }
            else
            {
                currentRectChangedScale.Add(tr, UChangeScaleAsync(tr, duration, newSize, curve, unscaledTime));
            }
        }

        private static async Task UChangeScaleAsync(RectTransform tr, float duration, Vector3 newSize, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Vector3 originalScale = tr.localScale;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (scalesToStop.Contains(tr) && timer != 0)
                {
                    scalesToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                tr.localScale = Vector3.Lerp(originalScale, newSize, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.localScale = newSize;
            currentRectChangedScale.Remove(tr);
        }

        #endregion


        #region Bounce Scale

        // Bounce
        public static void UBounce(this Transform tr, float duration1, Vector3 bounceSize, float duration2, Vector3 endSize, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if (currentChangedScale.Keys.Contains(tr))
            {
                scalesToStop.Add(tr);

                currentChangedScale[tr] = UBounceAsync(tr, duration1, bounceSize, duration2, endSize, curve, unscaledTime);
            }
            else
            {
                currentChangedScale.Add(tr, UBounceAsync(tr, duration1, bounceSize, duration2, endSize, curve, unscaledTime));
            }
        }

        private static async Task UBounceAsync(this Transform tr, float duration1, Vector3 bounceSize, float duration2, Vector3 endSize, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Vector3 originalScale = tr.localScale;

            while (timer < duration1)
            {
                if (!Application.isPlaying) return;
                if (scalesToStop.Contains(tr) && timer != 0)
                {
                    scalesToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                tr.localScale = Vector3.Lerp(originalScale, bounceSize, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration1));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.localScale = bounceSize;
            timer = 0;

            while (timer < duration2)
            {
                if (!Application.isPlaying) return;
                if (scalesToStop.Contains(tr) && timer != 0)
                {
                    scalesToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                tr.localScale = Vector3.Lerp(bounceSize, endSize, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration2));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.localScale = endSize;
            currentChangedScale.Remove(tr);
        }



        // Change rotation with rect transform
        private static Dictionary<Transform, Task> currentBouncedRectTr = new();
        public static void UBounce(this RectTransform tr, float duration1, Vector3 bounceSize, float duration2, Vector3 endSize, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if (currentBouncedRectTr.Keys.Contains(tr))
            {
                transformToStop.Add(tr);

                currentBouncedRectTr[tr] = UBounceAsync(tr, duration1, bounceSize, duration2, endSize, curve, unscaledTime);
            }
            else
            {
                currentBouncedRectTr.Add(tr, UBounceAsync(tr, duration1, bounceSize, duration2, endSize, curve, unscaledTime));
            }
        }

        private static async Task UBounceAsync(this RectTransform tr, float duration1, Vector3 bounceSize, float duration2, Vector3 endSize, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Vector3 originalScale = tr.localScale;

            while (timer < duration1)
            {
                if (!Application.isPlaying) return;
                if (transformToStop.Contains(tr) && timer != 0)
                {
                    transformToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                tr.localScale = Vector3.Lerp(originalScale, bounceSize, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration1));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.localScale = bounceSize;
            timer = 0;

            while (timer < duration2)
            {
                if (!Application.isPlaying) return;
                if (transformToStop.Contains(tr) && timer != 0)
                {
                    transformToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                tr.localScale = Vector3.Lerp(bounceSize, endSize, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration2));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.localScale = endSize;
            currentBouncedRectTr.Remove(tr);
        }

        #endregion


        /// ---------------- LOCAL VERSIONS --------------------


        #region Shake Local

        // Shake position
        private static Dictionary<Transform, Task> currentShakePositionsLocal = new();
        public static void UShakeLocalPosition(this Transform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
        {
            if (currentShakePositionsLocal.Keys.Contains(tr))
            {
                transformToStop.Add(tr);

                currentShakePositionsLocal[tr] = UShakeLocalPositionAsync(tr, duration, intensity, lockX, lockY, lockZ);
            }
            else
            {
                currentShakePositionsLocal.Add(tr, UShakeLocalPositionAsync(tr, duration, intensity, lockX, lockY, lockZ));
            }
        }

        private static async Task UShakeLocalPositionAsync(Transform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
        {
            float timer = 0;
            float startIntensity = intensity;
            Vector3 originalPos = tr.localPosition;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (transformToStop.Contains(tr) && timer != 0)
                {
                    transformToStop.Remove(tr);

                    return;
                }

                timer += Time.unscaledDeltaTime;

                intensity = Mathf.Lerp(startIntensity, 0, timer / duration);
                tr.localPosition = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));

                if (lockX) tr.localPosition = new Vector3(originalPos.x, tr.localPosition.y, tr.localPosition.z);
                if (lockY) tr.localPosition = new Vector3(tr.localPosition.x, originalPos.y, tr.localPosition.z);
                if (lockZ) tr.localPosition = new Vector3(tr.localPosition.x, tr.localPosition.y, originalPos.z);

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.localPosition = originalPos;
            currentShakePositionsLocal.Remove(tr);
        }



        // Shake position with rect transform
        private static Dictionary<RectTransform, Task> currentRectShakePositionsLocal = new();
        public static void UShakeLocalPosition(this RectTransform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
        {
            if (currentRectShakePositionsLocal.Keys.Contains(tr))
            {
                transformToStop.Add(tr);

                currentRectShakePositionsLocal[tr] = UShakeLocalPositionAsync(tr, duration, intensity, lockX, lockY, lockZ);
            }
            else
            {
                currentRectShakePositionsLocal.Add(tr, UShakeLocalPositionAsync(tr, duration, intensity, lockX, lockY, lockZ));
            }
        }

        private static async Task UShakeLocalPositionAsync(RectTransform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
        {
            float timer = 0;
            float startIntensity = intensity;
            Vector3 originalPos = tr.localPosition;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (transformToStop.Contains(tr) && timer != 0)
                {
                    transformToStop.Remove(tr);

                    return;
                }

                timer += Time.unscaledDeltaTime;

                intensity = Mathf.Lerp(startIntensity, 0, timer / duration);
                tr.localPosition = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));

                if (lockX) tr.localPosition = new Vector3(originalPos.x, tr.localPosition.y, tr.localPosition.z);
                if (lockY) tr.localPosition = new Vector3(tr.localPosition.x, originalPos.y, tr.localPosition.z);
                if (lockZ) tr.localPosition = new Vector3(tr.localPosition.x, tr.localPosition.y, originalPos.z);

                await Task.Yield();
            }

            tr.localPosition = originalPos;

            await Task.Yield();

            currentRectShakePositionsLocal.Remove(tr);
        }

        #endregion


        #region Position Local

        // Change position
        private static Dictionary<Transform, Task> currentChangedPosLocal = new();
        public static void UChangeLocalPosition(this Transform tr, float duration, Vector3 newPos, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if(duration == 0)
            {
                tr.localPosition = newPos;
                return;
            }

            if (currentChangedPosLocal.Keys.Contains(tr))
            {
                transformToStop.Add(tr);

                currentChangedPosLocal[tr] = UChangeLocalPositionAsync(tr, duration, newPos, curve, unscaledTime);
            }
            else
            {
                currentChangedPosLocal.Add(tr, UChangeLocalPositionAsync(tr, duration, newPos, curve, unscaledTime));
            }
        }

        private static async Task UChangeLocalPositionAsync(Transform tr, float duration, Vector3 newPos, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Vector3 originalPos = tr.localPosition;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (transformToStop.Contains(tr) && timer != 0)
                {
                    transformToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                tr.localPosition = Vector3.Lerp(originalPos, newPos, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.localPosition = newPos;
            currentChangedPosLocal.Remove(tr);
        }



        // Change position with rect transform
        private static Dictionary<RectTransform, Task> currentRectChangedPosLocal = new();
        public static void UChangeLocalPosition(this RectTransform tr, float duration, Vector3 newPos, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if (duration == 0)
            {
                tr.localPosition = newPos;
                return;
            }

            if (currentRectChangedPosLocal.Keys.Contains(tr))
            {
                transformToStop.Add(tr);

                currentRectChangedPosLocal[tr] = UChangeLocalPositionAsync(tr, duration, newPos, curve, unscaledTime);
            }
            else
            {
                currentRectChangedPosLocal.Add(tr, UChangeLocalPositionAsync(tr, duration, newPos, curve, unscaledTime));
            }
        }

        private static async Task UChangeLocalPositionAsync(RectTransform tr, float duration, Vector3 newPos, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Vector3 originalPos = tr.localPosition;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (transformToStop.Contains(tr) && timer != 0)
                {
                    transformToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                tr.localPosition = Vector3.Lerp(originalPos, newPos, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.localPosition = newPos;
            currentRectChangedPosLocal.Remove(tr);
        }

        #endregion


        #region Rotation Local

        // Change rotation
        private static Dictionary<Transform, Task> currentChangedRotLocal = new();
        public static void UChangeLocalRotation(this Transform tr, float duration, Quaternion newRot, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if(duration == 0)
            {
                tr.localRotation = newRot;
                return;
            }

            if (currentChangedRotLocal.Keys.Contains(tr))
            {
                transformToStop.Add(tr);

                currentChangedRotLocal[tr] = UChangeLocalRotationAsync(tr, duration, newRot, curve, unscaledTime);
            }
            else
            {
                currentChangedRotLocal.Add(tr, UChangeLocalRotationAsync(tr, duration, newRot, curve, unscaledTime));
            }
        }

        private static async Task UChangeLocalRotationAsync(Transform tr, float duration, Quaternion newRot, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Quaternion originalRot = tr.localRotation;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (transformToStop.Contains(tr) && timer != 0)
                {
                    transformToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                tr.localRotation = Quaternion.Lerp(originalRot, newRot, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.localRotation = newRot;
            currentChangedRotLocal.Remove(tr);
        }



        // Change rotation with rect transform
        private static Dictionary<RectTransform, Task> currentRectChangedRotLocal = new();
        public static void UChangeLocalRotation(this RectTransform tr, float duration, Quaternion newRot, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if (duration == 0)
            {
                tr.localRotation = newRot;
                return;
            }

            if (currentRectChangedRotLocal.Keys.Contains(tr))
            {
                transformToStop.Add(tr);

                currentRectChangedRotLocal[tr] = UChangeLocalRotationAsync(tr, duration, newRot, curve, unscaledTime);
            }
            else
            {
                currentRectChangedRotLocal.Add(tr, UChangeLocalRotationAsync(tr, duration, newRot, curve, unscaledTime));
            }
        }

        private static async Task UChangeLocalRotationAsync(RectTransform tr, float duration, Quaternion newRot, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Quaternion originalRot = tr.localRotation;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (transformToStop.Contains(tr) && timer != 0)
                {
                    transformToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                tr.localRotation = Quaternion.Lerp(originalRot, newRot, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.localRotation = newRot;
            currentRectChangedRotLocal.Remove(tr);
        }

        #endregion
    }
}
