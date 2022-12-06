using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsColorScript : MonoBehaviour
{
    public Texture2D legsTexture;

    public Color skinColor;
    public Color shortsColor;
    public Color shoesColor;
    private float skinColorH;
    private float shortsColorH;
    private float shoesColorH;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        legsTexture = sr.sprite.texture;

        float discard;
        Color.RGBToHSV(skinColor, out skinColorH, out discard, out discard);
        Color.RGBToHSV(shortsColor, out shortsColorH, out discard, out discard);
        Color.RGBToHSV(shoesColor, out shoesColorH, out discard, out discard);

        Texture2D newTex = ReplaceLegsColor(legsTexture);

        string tempName = sr.sprite.name;
        //sr.sprite = Sprite.Create(newTex, sr.sprite.rect, new Vector2(0, 1));
        sr.sprite.name = tempName;

        sr.material.mainTexture = newTex;
    }

    Texture2D ReplaceLegsColor(Texture2D originalTexture)
    {
        Texture2D newTexture = new Texture2D(originalTexture.width, originalTexture.height);
        newTexture.filterMode = FilterMode.Point;
        newTexture.wrapMode = TextureWrapMode.Clamp;

        for (int y = 0; y < newTexture.height; y++)
        {
            for(int x = 0; x < newTexture.width; x++)
            {
                float H, S, V;
                Color newColor;
                Color oldColor = originalTexture.GetPixel(x, y);
                Color.RGBToHSV(oldColor, out H, out S, out V);
                H *= 255;
                Debug.Log("PEDROLOG: H = " + H + ", S = " + S + ", V = " + V);
                if (H == 60)    //Hue is yellow
                {
                    newColor = Color.HSVToRGB(skinColorH, S, V);
                    //Debug.Log("PEDROLOG: RGB when H = 60: " + newColor);
                    newTexture.SetPixel(x, y, newColor);
                } else if (H == 0)  //Hue is red
                {
                    newColor = Color.HSVToRGB(shortsColorH, S, V);
                    //Debug.Log("PEDROLOG: RGB when H = 0: " + newColor);
                    newTexture.SetPixel(x, y, newColor);
                } else if (H == 240)    //Hue is blue
                {
                    newColor = Color.HSVToRGB(shoesColorH, S, V);
                    Debug.Log("PEDROLOG: RGB when H = 240: " + newColor);
                    newTexture.SetPixel(x, y, newColor);
                } else
                {
                    //Debug.Log("PEDROLOG: Keeping old color.");
                    newTexture.SetPixel(x, y, oldColor);
                }
            }
        }

        newTexture.name = (originalTexture.name + "_" + transform.parent.name);
        newTexture.Apply();    

        return newTexture;
    }
}
