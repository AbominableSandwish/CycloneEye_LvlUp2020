using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTrace : MonoBehaviour
{
    private SpriteRenderer renderer;

    public void SetTrace(Sprite sprite)
    {
        this.renderer.sprite = sprite;
    }

    public void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        renderer.color = renderer.color - new Color(0.0f, 0.0f, 0.0f, Time.deltaTime*2);
        if(renderer.color.a <= 0.0f)
            Destroy(this.gameObject);
    }
}
