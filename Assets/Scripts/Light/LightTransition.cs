using UnityEngine;

namespace Tools
{
    public class LightTransition : CommonLightTransition
    {
        [Header("必要组件")]
        [Tooltip("Light组件")]
        [SerializeField] private Light Light;

        private bool isLightLog;

        //初始化游戏对象相关组件
        protected sealed override bool InitComponents()
        {
            if (Light == null) return false;
            return true;
        }

        //光照过渡类型处理
        protected sealed override void TransitionTypeDeal()
        {
            if (_transitionBeginValueType == BeginValueType.Min)
            {
                Light.intensity = _intensityMinValue;
                index = 0;
                toMax = true;
            }
            else
            {
                Light.intensity = _intensityMaxValue;
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
                    Light.intensity = _changedValues[index++];
                }
                else
                {
                    if (index < 0)
                    {
                        lockUnLoopTransition = true;
                        return;
                    }
                    Light.intensity = _changedValues[index--];
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
                if (toMax) Light.intensity = _changedValues[index++];
                else Light.intensity = _changedValues[index--];
            }
        }

        //组件检测控制台提示方法
        protected sealed override void ComponentLog()
        {
            if (Light == null)
            {
                if (!isLightLog)
                {
                    Debug.LogWarning("The component \"<color=orange><b>Light</b></color>\" is null.");
                    isLightLog = true;
                }
            }
            else isLightLog = false;
        }
    }
}