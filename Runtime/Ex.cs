using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ex 
{
    /// <summary>
    /// RETURN THE VECTOR 2 FROM THE DIRECTION OF A GIVEN ANGLE
    /// </summary>
    public static Vector2 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }


    /// <summary>
    /// RETURN THE ANGLE FROM A GIVEN DIRECTION
    /// </summary>
    public static float GetAngleFromVector(this Vector2 dir)
    {
        dir = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle += 360;

        return angle;
    }


    /// <summary>
    /// ROTATES A VECTOR FROM A CERTAIN ANGLE
    /// </summary>
    public static Vector2 RotateDirection(this Vector2 originalDirection, float addedAngle)
    {
        float currentAngle = GetAngleFromVector(originalDirection);

        currentAngle += addedAngle;

        return GetVectorFromAngle(currentAngle);
    }
}
