using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    private TrailRenderer trail;

    public Gradient green;
    public Gradient red;
    public Gradient blue;
    public Gradient magmenta;
    // Start is called before the first frame update
    void Start()
    {
        trail = GetComponentInChildren<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setEmitting(bool emitting)
    {
        trail.emitting = emitting;
    }

    public enum trailColor
    {
        GREEN,
        RED,
        BLUE,
        MAGMENTA
    }

    public void setColorTrail(trailColor color)
    {
        switch (color)
        {
            case trailColor.GREEN:
                trail.colorGradient = green;
                break;
            case trailColor.RED:
                trail.colorGradient = red;
                break;
            case trailColor.BLUE:
                trail.colorGradient = blue;
                break;
            case trailColor.MAGMENTA:
                trail.colorGradient = magmenta;
                break;

        }
    }
}
