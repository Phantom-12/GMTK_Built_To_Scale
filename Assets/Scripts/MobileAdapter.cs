using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileAdapter : MonoBehaviour
{
    [SerializeField] private GameObject[] MobileKeys;
    private UIDragClamp uiDragClamp;
    // Start is called before the first frame update
    void Start()
    {
        uiDragClamp = GameObject.Find("ResolutionSlider").GetComponent<UIDragClamp>();
        if (Application.isMobilePlatform)
        {
            foreach(var key in MobileKeys)
            {
                key.SetActive(true);
            }
        }
    }
    public void ScaleUp()
    {
        uiDragClamp.ScaleUp();
    }
    public void ScaleDown()
    {
        uiDragClamp.ScaleDown();
    }

}
