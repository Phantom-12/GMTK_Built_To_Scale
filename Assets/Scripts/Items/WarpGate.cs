using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpGate : MonoBehaviour
{
    [SerializeField]
    WarpGate warpGateTarget;

    private bool canUse;

    private void Start()
    {
        canUse = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canUse)
        {
            other.transform.position = warpGateTarget.transform.position;
            warpGateTarget.SetCanUse(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetCanUse(true);
        }
    }

    public void SetCanUse(bool enable)
    {
        canUse = enable;
    }
}
