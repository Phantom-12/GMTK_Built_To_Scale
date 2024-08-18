using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragClamp : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI限制拖拽位置")]
    public RectTransform container;
    RectTransform rt;
    public RectTransform[] ScaleRectTransforms;

    // 位置偏移量
    //Vector3 offset = Vector3.zero;
    // 最小、最大X、Y坐标
    float minX, maxX;
    //float minY, maxY;

    void Start()
    {
        foreach (RectTransform t in ScaleRectTransforms)
        {
            if (t.name == "1")
            {
                t.gameObject.SetActive(ResolutionScalerManager.Instance.GetResolutionRatioEnable(t.name));
            }
            if (t.name != "1") 
            {
                t.gameObject.SetActive(!ResolutionScalerManager.Instance.GetResolutionRatioEnable(t.name));
            }
        }
        rt = GetComponent<RectTransform>();
        rt.position = new Vector3(ScaleRectTransforms[0].position.x, rt.position.y, rt.position.z);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.enterEventCamera, out Vector3 globalMousePos))
        {
            // 计算偏移量
            //offset = rt.position - globalMousePos;
            // 设置拖拽范围
            SetDragRange();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 将屏幕空间上的点转换为位于给定RectTransform平面上的世界空间中的位置
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos))
        {
            rt.position = new Vector3(DragRangeLimit(globalMousePos).x, rt.position.y, rt.position.z);
            print(container.localScale.x);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float distance = Mathf.Infinity;
        Vector3 targetPosition = Vector3.zero;
        string targetRadio = string.Empty;
        foreach (RectTransform t in ScaleRectTransforms)
        {
            if(t.name == "1")
            {
                if (t.gameObject.activeSelf)
                {
                    if (distance > Mathf.Abs(rt.position.x - t.position.x))
                    {
                        distance = Mathf.Abs(rt.position.x - t.position.x);
                        targetPosition = t.position;
                        targetRadio = t.name;
                    }
                }
            }
            if (t.name != "1")
            {
                if (!t.gameObject.activeSelf)
                {
                    if (distance > Mathf.Abs(rt.position.x - t.position.x))
                    {
                        distance = Mathf.Abs(rt.position.x - t.position.x);
                        targetPosition = t.position;
                        targetRadio = t.name;
                    }
                }
            }
        }
        rt.position = new Vector3(targetPosition.x, rt.position.y, rt.position.z);
        GameData.Instance.SetResolutionRatio(int.Parse(targetRadio));
    }
    //通过计算忽略Scale和pivot对UI的影响
    // 设置最大、最小坐标
    void SetDragRange()
    {
        if (container)
        {
            // 最小x坐标 = 容器当前x坐标 - 容器轴心距离左边界的距离 + UI轴心距离左边界的距离
            minX = container.position.x
            - container.pivot.x * container.rect.width
            + rt.rect.width * rt.localScale.x * rt.pivot.x;

            // 最大x坐标 = 容器当前x坐标 + 容器轴心距离右边界的距离 - UI轴心距离右边界的距离
            maxX = container.position.x
            + (1 - container.pivot.x) * container.rect.width
            - rt.rect.width * rt.localScale.x * (1 - rt.pivot.x);

            // 最小y坐标 = 容器当前y坐标 - 容器轴心距离底边的距离 + UI轴心距离底边的距离
            /*minY = container.position.y
            - container.pivot.y * container.rect.height
            + rt.rect.height * rt.localScale.y * rt.pivot.y;

            // 最大y坐标 = 容器当前x坐标 + 容器轴心距离顶边的距离 - UI轴心距离顶边的距离
            maxY = container.position.y
            + (1 - container.pivot.y) * container.rect.height
            - rt.rect.height * rt.localScale.y * (1 - rt.pivot.y);*/
        }
        else
        {
            minX = rt.rect.width * rt.pivot.x;
            maxX = Screen.width - rt.rect.width * (1 - rt.pivot.x);
            //minY = rt.rect.height * rt.pivot.y;
            //maxY = Screen.height - rt.rect.height * (1 - rt.pivot.y);
        }
        minX -= (maxX - minX) * (container.localScale.x - 1f) /2;
        maxX += (maxX - minX) * (container.localScale.x - 1f) /2;
    }


    // 限制坐标范围
    Vector3 DragRangeLimit(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        print(minX);
        print(maxX);
        //pos.y = Mathf.Clamp(pos.y, minY, maxY);
        return pos;
    }
}
