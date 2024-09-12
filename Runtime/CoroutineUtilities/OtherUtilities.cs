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



        public static void ULerpFloat(Action<float> currentVariable, float duration, float endValue)
        {
            ULerpFloatAction(currentVariable, duration, duration);
        }
        

        private static async Task ULerpFloatAction(Action<float> currentVariable, float duration, float endValue)
        {
            float timer = 0;
            float originalValue = 0;

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
