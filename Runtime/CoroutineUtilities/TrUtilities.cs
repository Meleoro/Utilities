using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public static class TrUtilities
{
    private static Dictionary<Transform, Task> currentShakePositions = new();
    public static void ShakePosition(this Transform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
    {
        if (currentShakePositions.Keys.Contains(tr))
        {
            currentShakePositions[tr].Dispose();
            currentShakePositions[tr] = ShakePositionAsync(tr, duration, intensity, lockX, lockY, lockZ);
        }
        else
        {
            currentShakePositions.Add(tr, ShakePositionAsync(tr, duration, intensity, lockX, lockY, lockZ));
        }
    }

    private static async Task ShakePositionAsync(Transform tr, float duration, float intensity, bool lockX = false, bool lockY = false, bool lockZ = false)
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

            await Task.Yield();
        }

        tr.position = originalPos;
    }
}
