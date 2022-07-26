using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{

    public Renderer textureRenderer;
    
    public void DrawNoiseMap(float[,] noiseMap, Vector3 objectSize, Gradient coloring) 
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colourMap = new Color[width * height];
        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                colourMap[y * width + x] = coloring.Evaluate(noiseMap[x,y]);
            }
        }
        texture.wrapMode = TextureWrapMode.Clamp;
        
        texture.SetPixels(colourMap);
        texture.Apply();

        textureRenderer.sharedMaterial.mainTexture = texture;
        
        textureRenderer.transform.localScale = new Vector3(objectSize.x, objectSize.y, objectSize.z);
    }
}
