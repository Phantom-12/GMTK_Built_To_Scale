using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Tools
{
    public class LightTransition2D : CommonLightTransition
    {
        [Header("必要组件(需要下载扩展包:Universal RP)")]
        [Tooltip("Light2D组件")]
        [SerializeField] private Light2D Light2D;

        private bool isLight2DLog;

        //初始化游戏对象相关组件
        protected sealed override bool InitComponents()
        {
            if (Light2D == null) return false;
            return true;
        }

        //光照过渡类型处理
        protected sealed override void TransitionTypeDeal()
        {
            if (_transitionBeginValueType == BeginValueType.Min)
            {
                Light2D.intensity = _intensityMinValue;
                index = 0;
                toMax = true;
            }
            else
            {
                Light2D.intensity = _intensityMaxValue;
                index = _changedValues.Length - 1;
                toMax = false;
            }
        }

        //非循环光照过渡
        protected sealed override void UnLoopLightTransition()
        {
            if (_isInit && !lockUnLoopTransition)
            {
                if (_transitionBeginValueType == BeginValueType.Min)
                {
                    if (index >= _changedValues.Length)
                    {
                        lockUnLoopTransition = true;
                        return;
                    }
                    Light2D.intensity = _changedValues[index++];
                }
                else
                {
                    if (index < 0)
                    {
                        lockUnLoopTransition = true;
                        return;
                    }
                    Light2D.intensity = _changedValues[index--];
                }
            }
        }

        //循环光照过渡
        protected sealed override void LoopLightTransition()
        {
            if (_isInit && _loop)
            {
                if (_loopMode == LoopMode.Pendulum)
                {
                    if (index <= 0) toMax = true;
                    else if (index >= _changedValues.Length - 1) toMax = false;
                }
                else if (index < 0 || index > _changedValues.Length - 1) TransitionTypeDeal();
                if (toMax) Light2D.intensity = _changedValues[index++];
                else Light2D.intensity = _changedValues[index--];
            }
        }

        //组件检测控制台提示方法
        protected sealed override void ComponentLog()
        {
            if (Light2D == null)
            {
                if (!isLight2DLog)
                {
                    Debug.LogWarning("The component \"<color=orange><b>Light2D</b></color>\" is null.");
                    isLight2DLog = true;
                }
            }
            else isLight2DLog = false;
        }
    }
}