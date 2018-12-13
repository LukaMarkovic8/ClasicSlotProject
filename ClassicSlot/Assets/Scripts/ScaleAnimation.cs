using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    //values for the amimation
    [SerializeField]
    private float durationTime = 1.5f;
    [SerializeField]
    private float scale = 3.0f;

    private float time = 0f;
    private bool isAnimating = false;

    void Update()
    {
        //checking if to start animation
        if (isAnimating)
        {   //
            if (time < durationTime / 1.5)
            {
                //increasing scale
                gameObject.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * scale, time);
                time += Time.deltaTime;
            }
            else if (time < durationTime)
            {
                //decreasing scale
                gameObject.transform.localScale = Vector3.Lerp(Vector3.one * scale, Vector3.one, time);
                time += Time.deltaTime;
            }
            else
            {
                //Resetting
                gameObject.transform.localScale = Vector3.one;
                time = 0f;
                isAnimating = false;
            }
        }
    }

    public void StartAnimation()
    {
        isAnimating = true;
    }

    public float DurationTime()
    {
        return durationTime;
    }
}
