using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTrace : MonoBehaviour
{
    public enum ColorPlayer
    {
        GREEN,
        BLUE,
        RED,
        MAGENTA
    }

    [SerializeField] ColorPlayer color;
    [SerializeField] Sprite green;
    [SerializeField] Sprite blue;
    [SerializeField] Sprite red;
    [SerializeField] Sprite magenta;

    private SpriteRenderer renderer;

    public SpriteRenderer source;

    public void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        switch (color)
        {
            case ColorPlayer.GREEN:
                renderer.sprite = green;
                break;
            case ColorPlayer.BLUE:
                renderer.sprite = blue;
                break;
            case ColorPlayer.RED:
                renderer.sprite = red;
                break;
            case ColorPlayer.MAGENTA:
                renderer.sprite = magenta;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        renderer.sprite = source.sprite;
        renderer.color = renderer.color - new Color(0.0f, 0.0f, 0.0f, Time.deltaTime*2);
        if(renderer.color.a <= 0.0f)
            Destroy(this.gameObject);
    }
}
