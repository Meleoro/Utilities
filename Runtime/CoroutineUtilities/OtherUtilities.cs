using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Utilities
{
    public static class OtherUtilities 
    {
        #region Lerp Float

        public static async Task ULerpFloatAction(this float variable, Action<float> currentVariable, float duration, float endValue)
        {
            float timer = 0;
            float originalValue = variable;

            while (timer < duration)
            {
                if (!Application.isPlaying) return;

                timer += Time.deltaTime;

                currentVariable(Mathf.Lerp(originalValue, endValue, timer / duration));

                await Task.Yield();
            }

            await Task.Yield();
        }

        #endregion
    }
}
