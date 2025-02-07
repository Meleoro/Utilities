using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;

namespace Utilities
{
    public enum ShakeLockType
    {
        None,
        X,
        Y,
        Z,
        XY,
        YZ,
        XZ
    }


    public static class TrUtilities
    {

        #region Shake

        public static void UStopShakePosition(this Transform tr)
        {
            if (currentShakePositions.Keys.Contains(tr))
            {
                trShakePositionsToStop.Add(tr);
                currentShakePositions.Remove(tr);
            }
        }

        private static List<Transform> trShakePositionsToStop = new List<Transform>();
        private static Dictionary<Transform, Task> currentShakePositions = new();
        public static void UShakePosition(this Transform tr, float duration, float intensity, float vibrato = 0.05f, ShakeLockType lockedAxis = ShakeLockType.None, bool unscaledTime = false)
        {
            if (currentShakePositions.Keys.Contains(tr))
            {
                trShakePositionsToStop.Add(tr);

                currentShakePositions[tr] = UShakePositionAsync(tr, duration, intensity, vibrato, lockedAxis, unscaledTime);
            }
            else
            {
                currentShakePositions.Add(tr, UShakePositionAsync(tr, duration, intensity, vibrato, lockedAxis, unscaledTime));
            }
        }

        private static async Task UShakePositionAsync(Transform tr, float duration, float intensity, float vibrato, ShakeLockType lockedAxis, bool unscaledTime)
        {
            float timer = 0;
            float startIntensity = intensity;
            Vector3 originalPos = tr.position;

            float stepTimer = 0;
            Vector3 previousPos = tr.position;
            Vector3 currentWantedPos = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (trShakePositionsToStop.Contains(tr) && timer != 0)
                {
                    trShakePositionsToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                stepTimer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                if(stepTimer > vibrato)
                {
                    stepTimer = 0;
                    previousPos = currentWantedPos;
                    currentWantedPos = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));
                }

                intensity = Mathf.Lerp(startIntensity, 0, timer / duration);
                tr.position = Vector3.Lerp(previousPos, currentWantedPos, stepTimer / vibrato);

                if (lockedAxis == ShakeLockType.X || lockedAxis == ShakeLockType.XY || lockedAxis == ShakeLockType.XZ) 
                    tr.position = new Vector3(originalPos.x, tr.position.y, tr.position.z);

                if (lockedAxis == ShakeLockType.Y || lockedAxis == ShakeLockType.XY || lockedAxis == ShakeLockType.YZ) 
                    tr.position = new Vector3(tr.position.x, originalPos.y, tr.position.z);

                if (lockedAxis == ShakeLockType.Z || lockedAxis == ShakeLockType.YZ || lockedAxis == ShakeLockType.XZ) 
                    tr.position = new Vector3(tr.position.x, tr.position.y, originalPos.z);

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.position = originalPos;
            currentShakePositions.Remove(tr);
        }



        public static void UStopShakePosition(this RectTransform tr)
        {
            if (currentRectShakePositions.Keys.Contains(tr))
            {
                rectTrShakePositionsToStop.Add(tr);
                currentRectShakePositions.Remove(tr);
            }
        }

        private static List<RectTransform> rectTrShakePositionsToStop = new List<RectTransform>();
        private static Dictionary<RectTransform, Task> currentRectShakePositions = new();
        public static void UShakePosition(this RectTransform tr, float duration, float intensity, float vibrato = 0.05f, ShakeLockType lockedAxis = ShakeLockType.None, bool unscaledTime = false)
        {
            if (currentRectShakePositions.Keys.Contains(tr))
            {
                rectTrShakePositionsToStop.Add(tr);

                currentRectShakePositions[tr] = UShakePositionAsync(tr, duration, intensity, vibrato, lockedAxis, unscaledTime);
            }
            else
            {
                currentRectShakePositions.Add(tr, UShakePositionAsync(tr, duration, intensity, vibrato, lockedAxis, unscaledTime));
            }
        }

        private static async Task UShakePositionAsync(RectTransform tr, float duration, float intensity, float vibrato, ShakeLockType lockedAxis, bool unscaledTime)
        {
            float timer = 0;
            float startIntensity = intensity;
            Vector3 originalPos = tr.position;

            float stepTimer = 0;
            Vector3 previousPos = tr.position;
            Vector3 currentWantedPos = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (rectTrShakePositionsToStop.Contains(tr) && timer != 0)
                {
                    rectTrShakePositionsToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                stepTimer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                if (stepTimer > vibrato)
                {
                    stepTimer = 0;
                    previousPos = currentWantedPos;
                    currentWantedPos = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));
                }

                intensity = Mathf.Lerp(startIntensity, 0, timer / duration);
                tr.position = Vector3.Lerp(previousPos, currentWantedPos, stepTimer / vibrato);

                if (lockedAxis == ShakeLockType.X || lockedAxis == ShakeLockType.XY || lockedAxis == ShakeLockType.XZ)
                    tr.position = new Vector3(originalPos.x, tr.position.y, tr.position.z);

                if (lockedAxis == ShakeLockType.Y || lockedAxis == ShakeLockType.XY || lockedAxis == ShakeLockType.YZ)
                    tr.position = new Vector3(tr.position.x, originalPos.y, tr.position.z);

                if (lockedAxis == ShakeLockType.Z || lockedAxis == ShakeLockType.YZ || lockedAxis == ShakeLockType.XZ)
                    tr.position = new Vector3(tr.position.x, tr.position.y, originalPos.z);

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.position = originalPos;
            currentRectShakePositions.Remove(tr);
        }

        #endregion


        #region Position

        public static void UStopChangePosition(this Transform tr)
        {
            if (currentChangedPos.Keys.Contains(tr))
            {
                trPositionsToStop.Add(tr);
                currentChangedPos.Remove(tr);
            }
        }

        private static List<Transform> trPositionsToStop = new List<Transform>();
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
                if(!trPositionsToStop.Contains(tr))
                    trPositionsToStop.Add(tr);

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
                if (trPositionsToStop.Contains(tr) && timer != 0)
                {
                    trPositionsToStop.Remove(tr);

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


        public static void UStopChangePosition(this RectTransform tr)
        {
            if (currentRectChangedPos.Keys.Contains(tr))
            {
                rectTrPositionsToStop.Add(tr);
                currentRectChangedPos.Remove(tr);
            }
        }

        private static List<RectTransform> rectTrPositionsToStop = new List<RectTransform>();
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
                if (!rectTrPositionsToStop.Contains(tr))
                    rectTrPositionsToStop.Add(tr);

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
                if (rectTrPositionsToStop.Contains(tr) && timer != 0)
                {
                    rectTrPositionsToStop.Remove(tr);

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

        public static void UStopChangeRotation(this Transform tr)
        {
            if (currentChangedRot.Keys.Contains(tr))
            {
                trRotationsToStop.Add(tr);
                currentChangedRot.Remove(tr);
            }
        }

        private static List<Transform> trRotationsToStop = new List<Transform>();
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
                if (!trRotationsToStop.Contains(tr))
                    trRotationsToStop.Add(tr);

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
                if (trRotationsToStop.Contains(tr) && timer != 0)
                {
                    trRotationsToStop.Remove(tr);

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
        
        public static void UChangeRotation(this Transform tr, float duration, Vector3 newRot, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if(duration == 0)
            {
                tr.rotation = Quaternion.Euler(newRot);
                return;
            }

            if (currentChangedRot.Keys.Contains(tr))
            {
                if (!trRotationsToStop.Contains(tr))
                    trRotationsToStop.Add(tr);

                currentChangedRot[tr] = UChangeRotationAsync(tr, duration, newRot, curve, unscaledTime);
            }
            else
            {
                currentChangedRot.Add(tr, UChangeRotationAsync(tr, duration, newRot, curve,unscaledTime));
            }
        }

        private static async Task UChangeRotationAsync(Transform tr, float duration, Vector3 newRot, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Vector3 originalRot = tr.eulerAngles;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (trRotationsToStop.Contains(tr) && timer != 0)
                {
                    trRotationsToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                tr.rotation = Quaternion.Euler(Vector3.Lerp(originalRot, newRot, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration)));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.rotation = Quaternion.Euler(newRot);
            currentChangedRot.Remove(tr);
        }



        public static void UStopChangeRotation(this RectTransform tr)
        {
            if (currentRectChangedRot.Keys.Contains(tr))
            {
                rectTrRotationsToStop.Add(tr);
                currentRectChangedRot.Remove(tr);
            }
        }

        private static List<RectTransform> rectTrRotationsToStop = new List<RectTransform>();
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
                if (!rectTrRotationsToStop.Contains(tr))
                    rectTrRotationsToStop.Add(tr);

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
                if (rectTrRotationsToStop.Contains(tr) && timer != 0)
                {
                    rectTrRotationsToStop.Remove(tr);

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

        public static void UStopChangeScale(this Transform tr)
        {
            if (currentChangedScale.Keys.Contains(tr))
            {
                trScalesToStop.Add(tr);
                currentChangedScale.Remove(tr);
            }
        }

        private static List<Transform> trScalesToStop = new List<Transform>();
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
                if(!trScalesToStop.Contains(tr))
                    trScalesToStop.Add(tr);

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
                if (trScalesToStop.Contains(tr) && timer != 0)
                {
                    trScalesToStop.Remove(tr);

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


        public static void UStopChangeScale(this RectTransform tr)
        {
            if (currentRectChangedScale.Keys.Contains(tr))
            {
                rectTrScalesToStop.Add(tr);
                currentRectChangedScale.Remove(tr);
            }
        }

        private static List<RectTransform> rectTrScalesToStop = new List<RectTransform>();
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
                if (!rectTrScalesToStop.Contains(tr))
                    rectTrScalesToStop.Add(tr);

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
                if (rectTrScalesToStop.Contains(tr) && timer != 0)
                {
                    rectTrScalesToStop.Remove(tr);

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

        public static void UStopBounceScale(this Transform tr)
        {
            if (currentBouncesScales.Keys.Contains(tr))
            {
                trBouceScaleToStop.Add(tr);
                currentBouncesScales.Remove(tr);
            }
        }

        private static List<Transform> trBouceScaleToStop = new List<Transform>();
        private static Dictionary<Transform, Task> currentBouncesScales = new();
        public static void UBounceScale(this Transform tr, float duration1, Vector3 bounceSize, float duration2, Vector3 endSize, 
            CurveType curve = CurveType.None, bool loop = false, bool unscaledTime = false)
        {
            if (currentBouncesScales.Keys.Contains(tr))
            {
                trBouceScaleToStop.Add(tr);

                currentBouncesScales[tr] = UBounceAsync(tr, duration1, bounceSize, duration2, endSize, curve, loop, unscaledTime);
            }
            else
            {
                currentBouncesScales.Add(tr, UBounceAsync(tr, duration1, bounceSize, duration2, endSize, curve, loop, unscaledTime));
            }
        }

        private static async Task UBounceAsync(this Transform tr, float duration1, Vector3 bounceSize, float duration2, Vector3 endSize, CurveType curve, bool loop, bool unscaledTime)
        {
            bool firstIteration = true;
            
            while (loop || firstIteration)
            {
                float timer = 0;
                Vector3 originalScale = tr.localScale;

                while (timer < duration1)
                {
                    if (!Application.isPlaying) return;
                    if (trBouceScaleToStop.Contains(tr) && timer != 0)
                    {
                        trBouceScaleToStop.Remove(tr);

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
                    if (trBouceScaleToStop.Contains(tr) && timer != 0)
                    {
                        trBouceScaleToStop.Remove(tr);

                        return;
                    }

                    timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                    tr.localScale = Vector3.Lerp(bounceSize, endSize, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration2));

                    await Task.Yield();
                }

                if (!Application.isPlaying) return;
                tr.localScale = endSize;

                firstIteration = false;
            }
            
            currentBouncesScales.Remove(tr);
        }


        public static void UStopBounceScale(this RectTransform tr)
        {
            if (currentBouncedScalesRectTr.Keys.Contains(tr))
            {
                rectTrBouceScaleToStop.Add(tr);
                currentBouncedScalesRectTr.Remove(tr);
            }
        }

        private static List<RectTransform> rectTrBouceScaleToStop = new List<RectTransform>();
        private static Dictionary<RectTransform, Task> currentBouncedScalesRectTr = new();
        public static void UBounce(this RectTransform tr, float duration1, Vector3 bounceSize, float duration2, Vector3 endSize, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if (currentBouncedScalesRectTr.Keys.Contains(tr))
            {
                rectTrBouceScaleToStop.Add(tr);

                currentBouncedScalesRectTr[tr] = UBounceAsync(tr, duration1, bounceSize, duration2, endSize, curve, unscaledTime);
            }
            else
            {
                currentBouncedScalesRectTr.Add(tr, UBounceAsync(tr, duration1, bounceSize, duration2, endSize, curve, unscaledTime));
            }
        }

        private static async Task UBounceAsync(this RectTransform tr, float duration1, Vector3 bounceSize, float duration2, Vector3 endSize, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Vector3 originalScale = tr.localScale;

            while (timer < duration1)
            {
                if (!Application.isPlaying) return;
                if (rectTrBouceScaleToStop.Contains(tr) && timer != 0)
                {
                    rectTrBouceScaleToStop.Remove(tr);

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
                if (rectTrBouceScaleToStop.Contains(tr) && timer != 0)
                {
                    rectTrBouceScaleToStop.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                tr.localScale = Vector3.Lerp(bounceSize, endSize, UtilitiesCurves.AdaptToWantedCurve(curve, timer / duration2));

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.localScale = endSize;
            currentBouncedScalesRectTr.Remove(tr);
        }

        #endregion


        #region Squish Effect

         public static void UStopSquish(this Transform tr)
        {
            if (currentSquishedTr.Keys.Contains(tr))
            {
                trSquishToStop.Add(tr);
                currentSquishedTr.Remove(tr);
            }
        }

        private static List<Transform> trSquishToStop = new List<Transform>();
        private static Dictionary<Transform, Task> currentSquishedTr = new();
        public static void USquishEffect(this Transform tr, float duration, float strength, bool is2D = false, CurveType curve = CurveType.None, bool unscaledTime = false)
        {
            if(duration == 0)
            {
                return;
            }

            if (currentSquishedTr.Keys.Contains(tr))
            {
                if(!trSquishToStop.Contains(tr))
                    trSquishToStop.Add(tr);

                currentSquishedTr[tr] = USquishEffectCoroutine(tr, duration, strength, is2D, curve, unscaledTime);
            }
            else
            {
                currentSquishedTr.Add(tr, USquishEffectCoroutine(tr, duration, strength, is2D, curve, unscaledTime));
            }
        }

        private static async Task USquishEffectCoroutine(Transform tr, float duration, float strength, bool is2D, CurveType curve, bool unscaledTime)
        {
            float timer = 0;
            Vector3 originalScale = tr.localScale;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (trSquishToStop.Contains(tr) && timer != 0)
                {
                    trSquishToStop.Remove(tr);
                    tr.localScale = originalScale;
                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                Vector3 finalScale = originalScale;

                if (is2D)
                {
                    finalScale.x = Mathf.Lerp(originalScale.x, originalScale.x + strength, UtilitiesCurves.AdaptToWantedCurve(curve, Mathf.Sin((timer / duration) * 3f)));
                    finalScale.y = Mathf.Lerp(originalScale.y, originalScale.y + strength, UtilitiesCurves.AdaptToWantedCurve(curve, Mathf.Sin((timer / duration) * 3f - 1)));
                    finalScale.z = originalScale.z;
                }
                else
                {
                    finalScale.x = Mathf.Lerp(originalScale.x, originalScale.x + strength, UtilitiesCurves.AdaptToWantedCurve(curve, Mathf.Sin((timer / duration) * 3f)));
                    finalScale.y = Mathf.Lerp(originalScale.y, originalScale.y + strength, UtilitiesCurves.AdaptToWantedCurve(curve, Mathf.Sin((timer / duration) * 3f - 1)));
                    finalScale.z = Mathf.Lerp(originalScale.z, originalScale.z + strength, UtilitiesCurves.AdaptToWantedCurve(curve, Mathf.Sin((timer / duration) * 3f)));
                }
                
                tr.localScale = finalScale;

                await Task.Yield();
            }

            tr.localScale = originalScale;
            
            if (!Application.isPlaying) return;
            currentSquishedTr.Remove(tr);
        }


        #endregion


        /// ---------------- LOCAL VERSIONS --------------------


        #region Shake Local

        public static void UStopLocalShakePosition(this Transform tr)
        {
            if (currentLocalShakePositions.Keys.Contains(tr))
            {
                trShakePositionsToStopLocal.Add(tr);
                currentLocalShakePositions.Remove(tr);
            }
        }

        private static List<Transform> trShakePositionsToStopLocal = new List<Transform>();
        private static Dictionary<Transform, Task> currentLocalShakePositions = new();
        public static void UShakeLocalPosition(this Transform tr, float duration, float intensity, float vibrato = 0.05f, ShakeLockType lockedAxis = ShakeLockType.None, bool unscaledTime = false)
        {
            if (currentLocalShakePositions.Keys.Contains(tr))
            {
                trShakePositionsToStopLocal.Add(tr);

                currentLocalShakePositions[tr] = UShakeLocalPositionAsync(tr, duration, intensity, vibrato, lockedAxis, unscaledTime);
            }
            else
            {
                currentLocalShakePositions.Add(tr, UShakeLocalPositionAsync(tr, duration, intensity, vibrato, lockedAxis, unscaledTime));
            }
        }

        private static async Task UShakeLocalPositionAsync(Transform tr, float duration, float intensity, float vibrato, ShakeLockType lockedAxis, bool unscaledTime)
        {
            float timer = 0;
            float startIntensity = intensity;
            Vector3 originalPos = tr.localPosition;

            float stepTimer = 0;
            Vector3 previousPos = tr.localPosition;
            Vector3 currentWantedPos = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (trShakePositionsToStopLocal.Contains(tr) && timer != 0)
                {
                    trShakePositionsToStopLocal.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                stepTimer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                if (stepTimer > vibrato)
                {
                    stepTimer = 0;
                    previousPos = currentWantedPos;
                    currentWantedPos = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));
                }

                intensity = Mathf.Lerp(startIntensity, 0, timer / duration);
                tr.localPosition = Vector3.Lerp(previousPos, currentWantedPos, stepTimer / vibrato);

                if (lockedAxis == ShakeLockType.X || lockedAxis == ShakeLockType.XY || lockedAxis == ShakeLockType.XZ)
                    tr.localPosition = new Vector3(originalPos.x, tr.localPosition.y, tr.localPosition.z);

                if (lockedAxis == ShakeLockType.Y || lockedAxis == ShakeLockType.XY || lockedAxis == ShakeLockType.YZ)
                    tr.localPosition = new Vector3(tr.localPosition.x, originalPos.y, tr.localPosition.z);

                if (lockedAxis == ShakeLockType.Z || lockedAxis == ShakeLockType.YZ || lockedAxis == ShakeLockType.XZ)
                    tr.localPosition = new Vector3(tr.localPosition.x, tr.localPosition.y, originalPos.z);

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.localPosition = originalPos;
            currentLocalShakePositions.Remove(tr);
        }



        public static void UStopLocalShakePosition(this RectTransform tr)
        {
            if (currentRectLocalShakePositions.Keys.Contains(tr))
            {
                rectTrShakePositionsToStopLocal.Add(tr);
                currentRectLocalShakePositions.Remove(tr);
            }
        }

        private static List<RectTransform> rectTrShakePositionsToStopLocal = new List<RectTransform>();
        private static Dictionary<RectTransform, Task> currentRectLocalShakePositions = new();
        public static void UShakeLocalPosition(this RectTransform tr, float duration, float intensity, float vibrato = 0.05f, ShakeLockType lockedAxis = ShakeLockType.None, bool unscaledTime = false)
        {
            if (currentRectLocalShakePositions.Keys.Contains(tr))
            {
                rectTrShakePositionsToStopLocal.Add(tr);

                currentRectLocalShakePositions[tr] = UShakeLocalPositionAsync(tr, duration, intensity, vibrato, lockedAxis, unscaledTime);
            }
            else
            {
                currentRectLocalShakePositions.Add(tr, UShakeLocalPositionAsync(tr, duration, intensity, vibrato, lockedAxis, unscaledTime));
            }
        }

        private static async Task UShakeLocalPositionAsync(RectTransform tr, float duration, float intensity, float vibrato, ShakeLockType lockedAxis, bool unscaledTime)
        {
            float timer = 0;
            float startIntensity = intensity;
            Vector3 originalPos = tr.localPosition;

            float stepTimer = 0;
            Vector3 previousPos = tr.localPosition;
            Vector3 currentWantedPos = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (rectTrShakePositionsToStopLocal.Contains(tr) && timer != 0)
                {
                    rectTrShakePositionsToStopLocal.Remove(tr);

                    return;
                }

                timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                stepTimer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                if (stepTimer > vibrato)
                {
                    stepTimer = 0;
                    previousPos = currentWantedPos;
                    currentWantedPos = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));
                }

                intensity = Mathf.Lerp(startIntensity, 0, timer / duration);
                tr.localPosition = Vector3.Lerp(previousPos, currentWantedPos, stepTimer / vibrato);

                if (lockedAxis == ShakeLockType.X || lockedAxis == ShakeLockType.XY || lockedAxis == ShakeLockType.XZ)
                    tr.localPosition = new Vector3(originalPos.x, tr.localPosition.y, tr.localPosition.z);

                if (lockedAxis == ShakeLockType.Y || lockedAxis == ShakeLockType.XY || lockedAxis == ShakeLockType.YZ)
                    tr.localPosition = new Vector3(tr.localPosition.x, originalPos.y, tr.localPosition.z);

                if (lockedAxis == ShakeLockType.Z || lockedAxis == ShakeLockType.YZ || lockedAxis == ShakeLockType.XZ)
                    tr.localPosition = new Vector3(tr.localPosition.x, tr.localPosition.y, originalPos.z);

                await Task.Yield();
            }

            if (!Application.isPlaying) return;
            tr.localPosition = originalPos;
            currentRectLocalShakePositions.Remove(tr);
        }

        #endregion


        #region Position Local

        public static void UStopChangePositionLocal(this Transform tr)
        {
            if (currentChangedPosLocal.Keys.Contains(tr))
            {
                trPositionsToStopLocal.Add(tr);
                currentChangedPosLocal.Remove(tr);
            }
        }

        private static List<Transform> trPositionsToStopLocal = new List<Transform>();
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
                trPositionsToStopLocal.Add(tr);

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
                if (trPositionsToStopLocal.Contains(tr) && timer != 0)
                {
                    trPositionsToStopLocal.Remove(tr);

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



        public static void UStopChangePositionLocal(this RectTransform tr)
        {
            if (currentRectChangedPosLocal.Keys.Contains(tr))
            {
                rectTrPositionsToStopLocal.Add(tr);
                currentRectChangedPosLocal.Remove(tr);
            }
        }

        private static List<RectTransform> rectTrPositionsToStopLocal = new List<RectTransform>();
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
                rectTrPositionsToStopLocal.Add(tr);

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
                if (rectTrPositionsToStopLocal.Contains(tr) && timer != 0)
                {
                    rectTrPositionsToStopLocal.Remove(tr);

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

        public static void UStopChangeLocalRotation(this Transform tr)
        {
            if (currentChangedRotLocal.Keys.Contains(tr))
            {
                trRotationsToStopLocal.Add(tr);
                currentChangedRotLocal.Remove(tr);
            }
        }

        private static List<Transform> trRotationsToStopLocal = new List<Transform>();
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
                trRotationsToStopLocal.Add(tr);

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
                if (trRotationsToStopLocal.Contains(tr) && timer != 0)
                {
                    trRotationsToStopLocal.Remove(tr);

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


        public static void UStopChangeLocalRotation(this RectTransform tr)
        {
            if (currentRectChangedRotLocal.Keys.Contains(tr))
            {
                rectTrRotationsToStopLocal.Add(tr);
                currentRectChangedRotLocal.Remove(tr);
            }
        }

        private static List<RectTransform> rectTrRotationsToStopLocal = new List<RectTransform>();
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
                rectTrRotationsToStopLocal.Add(tr);

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
                if (rectTrRotationsToStopLocal.Contains(tr) && timer != 0)
                {
                    rectTrRotationsToStopLocal.Remove(tr);

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
