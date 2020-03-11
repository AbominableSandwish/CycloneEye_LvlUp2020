using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PerlinNoiseSystem : MonoBehaviour
{
    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale;

    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;
    private float startTime;

    void Start()
    {
        
        rend = GetComponent<Renderer>();

        // Set up the texture and a Color array to hold pixels during processing.
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex;
        startTime = Time.time;
        
    }

    public Vector2 offsetPosition;

    void CalcNoise()
    {
        Color[] newPix = new Color[noiseTex.width * noiseTex.height];
        float[] samples = new float[noiseTex.width * noiseTex.height];
        int max = 1;
        for (int i = 0; i < max; i++)
        {
            int y = 0;
            while (y < noiseTex.height)
            {
                int x = 0;
                while (x < noiseTex.width)
                {
                    float xCoord =(float)x / noiseTex.width * scale;
                    float yCoord =(float)y / noiseTex.height * scale;
                    samples[(int)y * noiseTex.width + (int)x] = samples[(int)y * noiseTex.width + (int)x]+ Random.Range(0, 2);
                    if (i == max-1)
                    {
                        newPix[(int)y * noiseTex.width + (int)x] = new Color(samples[(int)y * noiseTex.width + (int)x]/ max, samples[(int)y * noiseTex.width + (int)x]/ max, samples[(int)y * noiseTex.width + (int)x]/ max);
                    }
                    x++;
                }

                y++;
               
            }

        }
        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(newPix);
        noiseTex.Apply();
        
    }

    private float lastCalc;
    private float TimetoCalc = 0.1f;

    void FixedUpdate()
    {
        if (Time.time >= lastCalc + TimetoCalc)
        {
            lastCalc = Time.time;
            // Set up the texture and a Color array to hold pixels during processing.
            pix = new Color[noiseTex.width * noiseTex.height];
            CalcNoise();
            
        }
    }

}
