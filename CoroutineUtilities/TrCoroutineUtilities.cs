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
            Destroy(gameObject);
    }
    
    
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

            if(lockX) tr.position = new Vector3 (originalPos.x, tr.position.y, tr.position.z);
            if(lockY) tr.position = new Vector3 (tr.position.x, originalPos.y, tr.position.z);
            if(lockZ) tr.position = new Vector3 (tr.position.x, tr.position.y, originalPos.z);

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

            if(progressCurve == null) tr.position = Vector3.Lerp(savePos, newPos, timer / duration);
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
                
            if(progressCurve == null) tr.rotation = Quaternion.Lerp(saveRot, newRot, timer / duration);
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
                
            if(progressCurve == null) tr.localScale = Vector3.Lerp(saveScale, newScale, timer / duration);
            else tr.localScale = Vector3.Lerp(saveScale, newScale, progressCurve.Evaluate(timer / duration));

            yield return null;
        }

        tr.localScale = newScale;
    }

    #endregion
}
