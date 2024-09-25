using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class MobileAdapter : MonoBehaviour
{
    private PlayerInputHandler playerInputHandler;
    [SerializeField] private GameObject[] MobileKeys;
    private UIDragClamp uiDragClamp;
    // Start is called before the first frame update
    void Start()
    {
        playerInputHandler=GameObject.FindFirstObjectByType<PlayerInputHandler>();
        uiDragClamp = GameObject.Find("ResolutionSlider").GetComponent<UIDragClamp>();
        if (Application.isMobilePlatform)
        {
            foreach (var key in MobileKeys)
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

    Vector2 dir=new();
    public void OnLeftInput(bool pressed)
    {
        if(pressed)
            dir+=Vector2.left;
        else
            dir-=Vector2.left;
        playerInputHandler.OnMoveInput(dir);
    }

    public void OnRightInput(bool pressed)
    {
        if(pressed)
            dir+=Vector2.right;
        else
            dir-=Vector2.right;
        playerInputHandler.OnMoveInput(dir);
    }

    public void OnJumpInput(bool pressed)
    {
        playerInputHandler.OnJumpInput(pressed);
    }
}
