using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    
    public static Vector2 GetCamWorldSize()
    {
        Camera mainCamera = Camera.main;
        if (!mainCamera) {Debug.Log("No camera found in CamWorldSize class"); return new Vector2();}

        Vector2 camWorldSize;
        camWorldSize.y = mainCamera.orthographicSize;
        camWorldSize.x = camWorldSize.y * mainCamera.aspect;
        return camWorldSize;
    }

}
