using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace Utilities
{
    public static class MaterialUtilities 
    {

        private static List<Material> materialsToStop = new List<Material>();

        #region Lerp Material Float

        private static Dictionary<Material, Task> currentLerpedMaterials = new();
        public static void ULerpMaterialFloat(this Material material, float duration, float endValue, string parameterName)
        {
            if (currentLerpedMaterials.Keys.Contains(material))
            {
                materialsToStop.Add(material);

                currentLerpedMaterials[material] = ULerpMaterialFloatAsync(material, duration, endValue, parameterName);
            }
            else
            {
                currentLerpedMaterials.Add(material, ULerpMaterialFloatAsync(material, duration, endValue, parameterName));
            }
        }

        private static async Task ULerpMaterialFloatAsync(Material material, float duration, float endValue, string parameterName)
        {
            float timer = 0;
            float originalValue = material.GetFloat(parameterName);

            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (materialsToStop.Contains(material) && timer != 0)
                {
                    materialsToStop.Remove(material);

                    return;
                }


                timer += Time.deltaTime;

                material.SetFloat(parameterName, Mathf.Lerp(originalValue, endValue, timer / duration));

                await Task.Yield();
            }

            material.SetFloat(parameterName, endValue);

            await Task.Yield();

            currentLerpedMaterials.Remove(material);
        }

        #endregion


        #region Lerp Material Color

        private static Dictionary<Material, Task> currentLerpedMaterialsColors = new();
        public static void ULerpMaterialColor(this Material material, float duration, Color endValue, string parameterName)
        {
            if (currentLerpedMaterialsColors.Keys.Contains(material))
            {
                materialsToStop.Add(material);

                currentLerpedMaterialsColors[material] = ULerpMaterialColorAsync(material, duration, endValue, parameterName);
            }
            else
            {
                currentLerpedMaterialsColors.Add(material, ULerpMaterialColorAsync(material, duration, endValue, parameterName));
            }
        }

        private static async Task ULerpMaterialColorAsync(Material material, float duration, Color endValue, string parameterName)
        {
            float timer = 0;
            Color originalValue = material.GetColor(parameterName);


            while (timer < duration)
            {
                if (!Application.isPlaying) return;
                if (materialsToStop.Contains(material) && timer != 0)
                {
                    materialsToStop.Remove(material);

                    return;
                }


                timer += Time.deltaTime;

                material.SetColor(parameterName, Color.Lerp(originalValue, endValue, timer / duration));

                await Task.Yield();
            }

            material.SetColor(parameterName, endValue);

            await Task.Yield();

            currentLerpedMaterialsColors.Remove(material);
        }

        #endregion
    }
}
