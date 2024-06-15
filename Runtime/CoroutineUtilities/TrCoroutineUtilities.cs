using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrCoroutineUtilities : MonoBehaviour
{
    public static TrCoroutineUtilities Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
            Instance = this;

    }



    // Global Transform / RectTransform

    #region Shake Object Pos

    private Dictionary<Transform, Coroutine> currentShakePositions = new();
    public void ShakePosition(Transform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
    {
        if (currentShakePositions.Keys.Contains(tr))
        {
            StopCoroutine(currentShakePositions[tr]);
            currentShakePositions[tr] = StartCoroutine(ShakePositionCoroutine(tr, duration, intensity, lockX, lockY, lockZ));
        }
        else
        {
            currentShakePositions.Add(tr, StartCoroutine(ShakePositionCoroutine(tr, duration, intensity, lockX, lockY, lockZ)));
        }
    }

    private IEnumerator ShakePositionCoroutine(Transform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
    {
        float timer = 0;
        float startIntensity = intensity;
        Vector3 originalPos = tr.position;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            intensity = Mathf.Lerp(startIntensity, 0, timer / duration);
            tr.position = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));

            if (lockX) tr.position = new Vector3(originalPos.x, tr.position.y, tr.position.z);
            if (lockY) tr.position = new Vector3(tr.position.x, originalPos.y, tr.position.z);
            if (lockZ) tr.position = new Vector3(tr.position.x, tr.position.y, originalPos.z);

            yield return null;
        }

        tr.position = originalPos;
    }

    private Dictionary<RectTransform, Coroutine> currentShakePositionsRect = new();
    public void ShakePosition(RectTransform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
    {
        if (currentShakePositionsRect.Keys.Contains(tr))
        {
            StopCoroutine(currentShakePositionsRect[tr]);
            currentShakePositionsRect[tr] = StartCoroutine(ShakePositionCoroutine(tr, duration, intensity, lockX, lockY, lockZ));
        }
        else
        {
            currentShakePositionsRect.Add(tr, StartCoroutine(ShakePositionCoroutine(tr, duration, intensity, lockX, lockY, lockZ)));
        }
    }

    private IEnumerator ShakePositionCoroutine(RectTransform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
    {
        float timer = 0;
        float startIntensity = intensity;
        Vector3 originalPos = tr.position;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            intensity = Mathf.Lerp(startIntensity, 0, timer / duration);
            tr.position = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));

            if (lockX) tr.position = new Vector3(originalPos.x, tr.position.y, tr.position.z);
            if (lockY) tr.position = new Vector3(tr.position.x, originalPos.y, tr.position.z);
            if (lockZ) tr.position = new Vector3(tr.position.x, tr.position.y, originalPos.z);

            yield return null;
        }

        tr.position = originalPos;
    }

    #endregion

    #region Change Tr Pos

    private Dictionary<Transform, Coroutine> currentChangedPos = new();
    public void ChangePosition(Transform tr, float duration, Vector3 newPos, AnimationCurve progressCurve = null)
    {
        if (currentChangedPos.Keys.Contains(tr))
        {
            StopCoroutine(currentChangedPos[tr]);
            currentChangedPos[tr] = StartCoroutine(ChangePositionCoroutine(tr, duration, newPos, progressCurve));
        }
        else
        {
            currentChangedPos.Add(tr, StartCoroutine(ChangePositionCoroutine(tr, duration, newPos, progressCurve)));
        }
    }

    private IEnumerator ChangePositionCoroutine(Transform tr, float duration, Vector3 newPos, AnimationCurve progressCurve = null)
    {
        float timer = 0;
        Vector3 savePos = tr.position;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (progressCurve == null) tr.position = Vector3.Lerp(savePos, newPos, timer / duration);
            else tr.position = Vector3.Lerp(savePos, newPos, progressCurve.Evaluate(timer / duration));

            yield return null;
        }

        tr.position = newPos;
    }

    private Dictionary<RectTransform, Coroutine> currentChangedPosRect = new();
    public void ChangePosition(RectTransform tr, float duration, Vector3 newPos, AnimationCurve progressCurve = null)
    {
        if (currentChangedPosRect.Keys.Contains(tr))
        {
            StopCoroutine(currentChangedPosRect[tr]);
            currentChangedPosRect[tr] = StartCoroutine(ChangePositionCoroutine(tr, duration, newPos, progressCurve));
        }
        else
        {
            currentChangedPosRect.Add(tr, StartCoroutine(ChangePositionCoroutine(tr, duration, newPos, progressCurve)));
        }
    }

    private IEnumerator ChangePositionCoroutine(RectTransform tr, float duration, Vector3 newPos, AnimationCurve progressCurve = null)
    {
        float timer = 0;
        Vector3 savePos = tr.position;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (progressCurve == null) tr.position = Vector3.Lerp(savePos, newPos, timer / duration);
            else tr.position = Vector3.Lerp(savePos, newPos, progressCurve.Evaluate(timer / duration));

            yield return null;
        }

        tr.position = newPos;
    }

    #endregion

    #region Change Tr Rot

    private Dictionary<Transform, Coroutine> currentChangeRot = new();
    public void ChangeRotation(Transform tr, float duration, Quaternion newRot, AnimationCurve progressCurve = null)
    {
        if (currentChangeRot.Keys.Contains(tr))
        {
            StopCoroutine(currentChangeRot[tr]);
            currentChangeRot[tr] = StartCoroutine(ChangeRotationCoroutine(tr, duration, newRot, progressCurve));
        }
        else
        {
            currentChangeRot.Add(tr, StartCoroutine(ChangeRotationCoroutine(tr, duration, newRot, progressCurve)));
        }
    }

    private IEnumerator ChangeRotationCoroutine(Transform tr, float duration, Quaternion newRot, AnimationCurve progressCurve = null)
    {
        float timer = 0;
        Quaternion saveRot = tr.rotation;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (progressCurve == null) tr.rotation = Quaternion.Lerp(saveRot, newRot, timer / duration);
            else tr.rotation = Quaternion.Lerp(saveRot, newRot, progressCurve.Evaluate(timer / duration));

            yield return null;
        }

        tr.rotation = newRot;
    }

    private Dictionary<RectTransform, Coroutine> currentChangeRotRect = new();
    public void ChangeRotation(RectTransform tr, float duration, Quaternion newRot, AnimationCurve progressCurve = null)
    {
        if (currentChangeRotRect.Keys.Contains(tr))
        {
            StopCoroutine(currentChangeRotRect[tr]);
            currentChangeRotRect[tr] = StartCoroutine(ChangeRotationCoroutine(tr, duration, newRot, progressCurve));
        }
        else
        {
            currentChangeRotRect.Add(tr, StartCoroutine(ChangeRotationCoroutine(tr, duration, newRot, progressCurve)));
        }
    }

    private IEnumerator ChangeRotationCoroutine(RectTransform tr, float duration, Quaternion newRot, AnimationCurve progressCurve = null)
    {
        float timer = 0;
        Quaternion saveRot = tr.rotation;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (progressCurve == null) tr.rotation = Quaternion.Lerp(saveRot, newRot, timer / duration);
            else tr.rotation = Quaternion.Lerp(saveRot, newRot, progressCurve.Evaluate(timer / duration));

            yield return null;
        }

        tr.rotation = newRot;
    }

    #endregion

    #region Change Tr Scale

    private Dictionary<Transform, Coroutine> currentChangeScale = new();
    public void ChangeScale(Transform tr, float duration, Vector3 newScale, AnimationCurve progressCurve = null)
    {
        if (currentChangeScale.Keys.Contains(tr))
        {
            StopCoroutine(currentChangeScale[tr]);
            currentChangeScale[tr] = StartCoroutine(ChangeScaleCoroutine(tr, duration, newScale, progressCurve));
        }
        else
        {
            currentChangeScale.Add(tr, StartCoroutine(ChangeScaleCoroutine(tr, duration, newScale, progressCurve)));
        }
    }

    private IEnumerator ChangeScaleCoroutine(Transform tr, float duration, Vector3 newScale, AnimationCurve progressCurve = null)
    {
        float timer = 0;
        Vector3 saveScale = tr.localScale;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (progressCurve == null) tr.localScale = Vector3.Lerp(saveScale, newScale, timer / duration);
            else tr.localScale = Vector3.Lerp(saveScale, newScale, progressCurve.Evaluate(timer / duration));

            yield return null;
        }

        tr.localScale = newScale;
    }

    private Dictionary<RectTransform, Coroutine> currentChangeScaleRect = new();
    public void ChangeScale(RectTransform tr, float duration, Vector3 newScale, AnimationCurve progressCurve = null)
    {
        if (currentChangeScaleRect.Keys.Contains(tr))
        {
            StopCoroutine(currentChangeScaleRect[tr]);
            currentChangeScaleRect[tr] = StartCoroutine(ChangeScaleCoroutine(tr, duration, newScale, progressCurve));
        }
        else
        {
            currentChangeScaleRect.Add(tr, StartCoroutine(ChangeScaleCoroutine(tr, duration, newScale, progressCurve)));
        }
    }

    private IEnumerator ChangeScaleCoroutine(RectTransform tr, float duration, Vector3 newScale, AnimationCurve progressCurve = null)
    {
        float timer = 0;
        Vector3 saveScale = tr.localScale;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (progressCurve == null) tr.localScale = Vector3.Lerp(saveScale, newScale, timer / duration);
            else tr.localScale = Vector3.Lerp(saveScale, newScale, progressCurve.Evaluate(timer / duration));

            yield return null;
        }

        tr.localScale = newScale;
    }

    #endregion



    // Local Transform / RectTransform

    #region Shake Object Pos Local

    private Dictionary<Transform, Coroutine> currentShakePositionsLocal = new();
    public void ShakeLocalPosition(Transform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
    {
        if (currentShakePositionsLocal.Keys.Contains(tr))
        {
            StopCoroutine(currentShakePositionsLocal[tr]);
            currentShakePositionsLocal[tr] = StartCoroutine(ShakeLocalPositionCoroutine(tr, duration, intensity, lockX, lockY, lockZ));
        }
        else
        {
            currentShakePositionsLocal.Add(tr, StartCoroutine(ShakeLocalPositionCoroutine(tr, duration, intensity, lockX, lockY, lockZ)));
        }
    }

    private IEnumerator ShakeLocalPositionCoroutine(Transform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
    {
        float timer = 0;
        float startIntensity = intensity;
        Vector3 originalPos = tr.localPosition;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            intensity = Mathf.Lerp(startIntensity, 0, timer / duration);
            tr.localPosition = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));

            if (lockX) tr.localPosition = new Vector3(originalPos.x, tr.localPosition.y, tr.localPosition.z);
            if (lockY) tr.localPosition = new Vector3(tr.localPosition.x, originalPos.y, tr.localPosition.z);
            if (lockZ) tr.localPosition = new Vector3(tr.localPosition.x, tr.localPosition.y, originalPos.z);

            yield return null;
        }

        tr.localPosition = originalPos;
    }

    private Dictionary<RectTransform, Coroutine> currentShakePositionsLocalRect = new();
    public void ShakeLocalPosition(RectTransform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
    {
        if (currentShakePositionsLocalRect.Keys.Contains(tr))
        {
            StopCoroutine(currentShakePositionsLocalRect[tr]);
            currentShakePositionsLocalRect[tr] = StartCoroutine(ShakeLocalPositionCoroutine(tr, duration, intensity, lockX, lockY, lockZ));
        }
        else
        {
            currentShakePositionsLocalRect.Add(tr, StartCoroutine(ShakeLocalPositionCoroutine(tr, duration, intensity, lockX, lockY, lockZ)));
        }
    }

    private IEnumerator ShakeLocalPositionCoroutine(RectTransform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
    {
        float timer = 0;
        float startIntensity = intensity;
        Vector3 originalPos = tr.localPosition;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            intensity = Mathf.Lerp(startIntensity, 0, timer / duration);
            tr.localPosition = originalPos + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));

            if (lockX) tr.localPosition = new Vector3(originalPos.x, tr.localPosition.y, tr.localPosition.z);
            if (lockY) tr.localPosition = new Vector3(tr.localPosition.x, originalPos.y, tr.localPosition.z);
            if (lockZ) tr.localPosition = new Vector3(tr.localPosition.x, tr.localPosition.y, originalPos.z);

            yield return null;
        }

        tr.localPosition = originalPos;
    }

    #endregion

    #region Change Tr Pos Local

    private Dictionary<Transform, Coroutine> currentChangedPosLocal = new();
    public void ChangeLocalPosition(Transform tr, float duration, Vector3 newPos, AnimationCurve progressCurve = null)
    {
        if (currentChangedPosLocal.Keys.Contains(tr))
        {
            StopCoroutine(currentChangedPosLocal[tr]);
            currentChangedPosLocal[tr] = StartCoroutine(ChangeLocalPositionCoroutine(tr, duration, newPos, progressCurve));
        }
        else
        {
            currentChangedPosLocal.Add(tr, StartCoroutine(ChangeLocalPositionCoroutine(tr, duration, newPos, progressCurve)));
        }
    }

    private IEnumerator ChangeLocalPositionCoroutine(Transform tr, float duration, Vector3 newPos, AnimationCurve progressCurve = null)
    {
        float timer = 0;
        Vector3 savePos = tr.localPosition;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (progressCurve == null) tr.localPosition = Vector3.Lerp(savePos, newPos, timer / duration);
            else tr.localPosition = Vector3.Lerp(savePos, newPos, progressCurve.Evaluate(timer / duration));

            yield return null;
        }

        tr.localPosition = newPos;
    }

    private Dictionary<RectTransform, Coroutine> currentChangedPosLocalRect = new();
    public void ChangeLocalPosition(RectTransform tr, float duration, Vector3 newPos, AnimationCurve progressCurve = null)
    {
        if (currentChangedPosLocalRect.Keys.Contains(tr))
        {
            StopCoroutine(currentChangedPosLocalRect[tr]);
            currentChangedPosLocalRect[tr] = StartCoroutine(ChangeLocalPositionCoroutine(tr, duration, newPos, progressCurve));
        }
        else
        {
            currentChangedPosLocalRect.Add(tr, StartCoroutine(ChangeLocalPositionCoroutine(tr, duration, newPos, progressCurve)));
        }
    }

    private IEnumerator ChangeLocalPositionCoroutine(RectTransform tr, float duration, Vector3 newPos, AnimationCurve progressCurve = null)
    {
        float timer = 0;
        Vector3 savePos = tr.localPosition;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (progressCurve == null) tr.localPosition = Vector3.Lerp(savePos, newPos, timer / duration);
            else tr.localPosition = Vector3.Lerp(savePos, newPos, progressCurve.Evaluate(timer / duration));

            yield return null;
        }

        tr.localPosition = newPos;
    }

    #endregion

    #region Change Tr Rot Local

    private Dictionary<Transform, Coroutine> currentChangeRotLocal = new();
    public void ChangeLocalRotation(Transform tr, float duration, Quaternion newRot, AnimationCurve progressCurve = null)
    {
        if (currentChangeRotLocal.Keys.Contains(tr))
        {
            StopCoroutine(currentChangeRotLocal[tr]);
            currentChangeRotLocal[tr] = StartCoroutine(ChangeLocalRotationCoroutine(tr, duration, newRot, progressCurve));
        }
        else
        {
            currentChangeRotLocal.Add(tr, StartCoroutine(ChangeLocalRotationCoroutine(tr, duration, newRot, progressCurve)));
        }
    }

    private IEnumerator ChangeLocalRotationCoroutine(Transform tr, float duration, Quaternion newRot, AnimationCurve progressCurve = null)
    {
        float timer = 0;
        Quaternion saveRot = tr.localRotation;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (progressCurve == null) tr.localRotation = Quaternion.Lerp(saveRot, newRot, timer / duration);
            else tr.localRotation = Quaternion.Lerp(saveRot, newRot, progressCurve.Evaluate(timer / duration));

            yield return null;
        }

        tr.localRotation = newRot;
    }

    private Dictionary<RectTransform, Coroutine> currentChangeRotLocalRect = new();
    public void ChangeLocalRotation(RectTransform tr, float duration, Quaternion newRot, AnimationCurve progressCurve = null)
    {
        if (currentChangeRotLocalRect.Keys.Contains(tr))
        {
            StopCoroutine(currentChangeRotLocalRect[tr]);
            currentChangeRotLocalRect[tr] = StartCoroutine(ChangeLocalRotationCoroutine(tr, duration, newRot, progressCurve));
        }
        else
        {
            currentChangeRotLocalRect.Add(tr, StartCoroutine(ChangeLocalRotationCoroutine(tr, duration, newRot, progressCurve)));
        }
    }

    private IEnumerator ChangeLocalRotationCoroutine(RectTransform tr, float duration, Quaternion newRot, AnimationCurve progressCurve = null)
    {
        float timer = 0;
        Quaternion saveRot = tr.localRotation;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (progressCurve == null) tr.localRotation = Quaternion.Lerp(saveRot, newRot, timer / duration);
            else tr.localRotation = Quaternion.Lerp(saveRot, newRot, progressCurve.Evaluate(timer / duration));

            yield return null;
        }

        tr.localRotation = newRot;
    }

    #endregion
}
