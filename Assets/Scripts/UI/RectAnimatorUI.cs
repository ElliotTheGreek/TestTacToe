using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this class should pulse the attached RectTransform's scale slightly up and down around 1.0f while the object is active
public class RectAnimatorUI : MonoBehaviour
{
    public RectTransform rectTransform;
    public float pulseSpeed = 5.0f;
    public float pulseAmount = 0.1f;

    void Update()
    {
        if (rectTransform == null) return;
        if (gameObject.activeSelf == false) return;

        float scale = 1.0f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        rectTransform.localScale = new Vector3(scale, scale, 1.0f);
    }
}
