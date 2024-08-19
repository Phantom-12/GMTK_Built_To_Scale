using System.Collections.Generic;
using System.Linq;

namespace Tools
{
    /// <summary>
    /// 数值范围数组工具类
    /// </summary>
    public static class NumberRange
    {
        /// <summary>
        /// 获取指定范围内指定步长的Float数值数组
        /// <para>p_start:起始值</para>
        /// <para>p_end:终点值</para>
        /// <para>p_step:步长值</para>
        /// <para>[ContainsEnd]:是否包括终点值,默认为false</para>
        /// <para>返回值:Float[]</para>
        /// </summary>
        public static float[] FloatRange(float p_start, float p_end, float p_step, bool ContainsEnd = false)
        {
            if (!ContainsEnd) return DoFloatRange(p_start, p_end, p_step).ToArray();
            else
            {
                List<float> result = DoFloatRange(p_start, p_end, p_step).ToList();
                result.Add(p_end);
                return result.ToArray();
            }
        }

        static IEnumerable<float> DoFloatRange(float p_start, float p_end, float p_step)
        {
            for (float i = p_start; i <= p_end; i += p_step)
            {
                yield return i;
            }
        }
    }
}
