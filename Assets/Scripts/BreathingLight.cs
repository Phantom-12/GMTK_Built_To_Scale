using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BreathingLight : MonoBehaviour
{
    private float startIntensity;
    private float endIntensity;
    private bool toEnd = true;
    private bool toStart = true;
    private bool isGrowing = true;
    Bloom bloom;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float minIntensity;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Volume>().profile.TryGet(out bloom);
    }

    // Update is called once per frame
    void Update()
    {
        if (toEnd)
        {
            endIntensity = Random.Range((minIntensity + maxIntensity) / 2f, maxIntensity);
            toEnd = false;
        }
        if (toStart)
        {
            startIntensity = Random.Range(minIntensity, (minIntensity + maxIntensity) / 8f);
            toStart = false;
        }  
        if (isGrowing)
        {
            if(bloom.intensity.value < endIntensity)
            {
                bloom.intensity.value += 0.01f; 
            }
            else
            {
                isGrowing = false;
                toEnd = true;
            }
        }
        else if (!isGrowing)
        {
            if (bloom.intensity.value > startIntensity)
            {
                bloom.intensity.value -= 0.01f;
            }
            else
            {
                isGrowing = true;
                toStart = true;
            }
        }
    }
}
