using UnityEngine;

namespace Utilities
{
    public enum CurveType
    {
        None,
        EaseInSin,
        EaseOutSin, 
        EaseInOutSin,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack
    }

    public static class UtilitiesCurves 
    {
        public static float AdaptToWantedCurve(CurveType curveType, float progress)
        {
            switch (curveType)
            {
                case CurveType.None:
                    return progress;


                case CurveType.EaseInSin:
                    return 1 - Mathf.Cos(Mathf.PI * progress / 2);

                case CurveType.EaseOutSin:
                    return Mathf.Sin(Mathf.PI * progress / 2);

                case CurveType.EaseInOutSin:
                    return (1 - Mathf.Cos(Mathf.PI * progress)) * 0.5f;


                case CurveType.EaseInCubic:
                    return Mathf.Pow(progress, 3);

                case CurveType.EaseOutCubic:
                    return 1 -  Mathf.Pow(1 - progress, 3);

                case CurveType.EaseInOutCubic:
                    if(progress < 0.5)
                        return 4 * Mathf.Pow(progress, 3);
                    
                    else
                        return 1 -  Mathf.Pow(-progress * 2 + 2, 3) / 2;


                case CurveType.EaseInBack:
                    return Mathf.Pow(progress, 3) - progress * Mathf.Sin(progress * Mathf.PI);

                case CurveType.EaseOutBack:
                    return 1 - Mathf.Pow(1 - progress, 3) - (1 - progress) * Mathf.Sin((1 - progress) * Mathf.PI);

                case CurveType.EaseInOutBack:
                    if (progress < 0.5)
                        return 0.5f * (Mathf.Pow(progress, 3) - progress * Mathf.Sin(progress * Mathf.PI));

                    else
                        return 1 - 0.5f * (Mathf.Pow(1 - progress, 3) - (1 - progress) * Mathf.Sin((1 - progress) * Mathf.PI));

            }

            return progress;
        }
    }
}
