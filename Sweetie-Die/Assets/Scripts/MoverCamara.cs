using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBob : MonoBehaviour
{
    public float bobbingSpeed = 0.01f;
    public float bobbingAmount = 0.03f;
    public float midpoint = 0.5f;
    public float transitionTime = 0.5f;

    private float timer = 0.0f;
    private float startY;

    void Start()
    {
        startY = transform.localPosition.y;
    }

    void Update()
    {
        float waveslice = 0.0f;
        float vertical = Input.GetAxis("Vertical");

        Vector3 cSharpConversion = transform.localPosition;

        if (Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }
        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;

            float targetY = midpoint + translateChange;
            float newY = Mathf.Lerp(cSharpConversion.y, targetY, transitionTime);
            cSharpConversion.y = newY;
        }
        else
        {
            cSharpConversion.y = Mathf.Lerp(cSharpConversion.y, startY, transitionTime);
        }

        transform.localPosition = cSharpConversion;
    }
}