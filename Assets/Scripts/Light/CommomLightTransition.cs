using System.Collections;
using UnityEngine;

namespace Tools
{
    public class CommonLightTransition : MonoBehaviour
    {
        [Header("必要属性")]
        [Tooltip("光照强度变化平均值,默认值为0.1(值>0)")]
        [SerializeField] private float IntensityStepValue = 0.1f;
        [Tooltip("光照强度最小值,默认值为1(值属于[0,IntensityMaxValue))")]
        [SerializeField] private float IntensityMinValue = 1;
        [Tooltip("光照强度最大值,默认值为2(值>IntensityMinValue)")]
        [SerializeField] private float IntensityMaxValue = 2;
        [Tooltip("过渡时间间隔,默认值为0.05,单位s(值>0)")]
        [SerializeField] private float TransitionTimeSpan = 0.05f;
        [Tooltip("是否开启光照过渡循环,默认值为true")]
        [SerializeField] private bool Loop = true;
        [Tooltip("光照过渡循环方式,需要勾选Loop该选项才有效,BeginToEnd是每次循环从光照过渡起始值开始,Pendulum是从光照过渡起始值至光照过渡最终值再过渡至光照过渡起始值(钟摆式过渡)")]
        [SerializeField] private LoopMode TheLoopMode;
        [Tooltip("光照过渡起始值类型,Min代表起始值从IntensityMinValue开始,Max同理")]
        [SerializeField] private BeginValueType TransitionBeginValueType;

        protected bool toMax;
        protected bool lockUnLoopTransition;
        protected int index;
        protected bool _isInit { get => isInit; }
        protected float[] _changedValues { get => changedValues; }
        protected BeginValueType _transitionBeginValueType { get => TransitionBeginValueType; }
        protected bool _loop { get => Loop; }
        protected LoopMode _loopMode { get => TheLoopMode; }
        protected float _intensityMinValue { get => IntensityMinValue; }
        protected float _intensityMaxValue { get => IntensityMaxValue; }

        private bool isInit, endTransition;
        private float[] changedValues;
        private IEnumerator coroutine;

        /// <summary>
        /// 启动过渡
        /// </summary>
        public void StartTransition()
        {
            if (isInit && endTransition)
            {
                StartCoroutine(coroutine);
                endTransition = false;
            }
        }

        /// <summary>
        /// 停止过渡
        /// </summary>
        public void StopTransition()
        {
            if (isInit && !endTransition)
            {
                StopCoroutine(coroutine);
                endTransition = true;
            }
        }

        /// <summary>
        /// 初始化游戏对象相关组件
        /// <para>返回值:初始化组件成功则返回true,否则返回false</para>
        /// </summary>
        protected virtual bool InitComponents() { return false; }

        /// <summary>
        /// 光照过渡类型处理
        /// </summary>
        protected virtual void TransitionTypeDeal() { }

        /// <summary>
        /// 非循环光照过渡
        /// </summary>
        protected virtual void UnLoopLightTransition() { }

        /// <summary>
        /// 循环光照过渡
        /// </summary>
        protected virtual void LoopLightTransition() { }

        /// <summary>
        /// 组件检测控制台提示方法
        /// <para>声明:该方法仅在Unity Editor模式下且当Inspector面板中组件属性发生更改时执行</para>
        /// </summary>
        protected virtual void ComponentLog() { }

        /// <summary>
        /// 光照过渡起始值类型
        /// </summary>
        protected enum BeginValueType
        {
            Min, Max
        }

        /// <summary>
        /// 循环光照过渡方式
        /// </summary>
        protected enum LoopMode
        {
            BeginToEnd, Pendulum
        }

        private void Start()
        {
            isInit = InitComponents() && InitParameters();
        }

        //初始化游戏对象相关参数
        private bool InitParameters()
        {
            if (IntensityStepValue <= 0 || IntensityMinValue < 0 || IntensityMaxValue <= 0) return false;
            if (IntensityMinValue >= IntensityMaxValue) return false;
            if (TransitionTimeSpan <= 0) return false;
            changedValues = NumberRange.FloatRange(IntensityMinValue, IntensityMaxValue, IntensityStepValue, true);
            if (changedValues == null && changedValues.Length == 0) return false;
            TransitionTypeDeal();
            coroutine = UnLoopTransition();
            if (Loop)
            {
                lockUnLoopTransition = true;
                coroutine = LoopTransition();
            }
            endTransition = true;
            return true;
        }

        private IEnumerator LoopTransition()
        {
            while (true)
            {
                LoopLightTransition();
                yield return new WaitForSeconds(TransitionTimeSpan);
            }
        }

        private IEnumerator UnLoopTransition()
        {
            while (!lockUnLoopTransition)
            {
                UnLoopLightTransition();
                yield return new WaitForSeconds(TransitionTimeSpan);
            }
            endTransition = true;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            ComponentLog();
        }
#endif
    }
}
