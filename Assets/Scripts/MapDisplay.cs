using UnityEngine;

public class MapDisplay : MonoBehaviour
{

    public Renderer textureRenderer;
    
    public void DrawNoiseMap(Texture2D texture, Color[] colourMap, float[,] noiseMap, Vector3 objectSize, Gradient coloring) 
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        //Debug.Log("Create and evaluate all colours against provided gradient");
        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                colourMap[y * width + x] = coloring.Evaluate(noiseMap[x,y]);
            }
        }
        texture.wrapMode = TextureWrapMode.Clamp;
        

        //Debug.Log("colourMap contains a total entries: " + colourMap.Length);
        //Debug.Log("First entry is " + colourMap[0].ToString());
        
        texture.SetPixels(colourMap);
        //Debug.Log("Pixels set");

        texture.Apply();
        //Debug.Log("Texture applied");

        textureRenderer.sharedMaterial.mainTexture = texture;
        
        textureRenderer.transform.localScale = new Vector3(objectSize.x, objectSize.y, objectSize.z);
    }
}
