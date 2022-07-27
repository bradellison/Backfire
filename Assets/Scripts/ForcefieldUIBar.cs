using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForcefieldUIBar : MonoBehaviour
{

    public GameObject leftSide;
    public GameObject rightSide;

    Vector3 leftSideVector;
    Vector3 rightSideVector;

    float maxDistance;

    public float forcefieldCounter;
    public float forcefieldTarget;

    PlayerController player;

    RectTransform rectTransform;

    public Color forcefieldInactiveColor;
    public Color forcefieldActiveColor;

    bool isForcefieldActive;

    void Start()
    {
        isForcefieldActive = false;
        leftSideVector = leftSide.transform.localPosition;
        rightSideVector = rightSide.transform.localPosition;
        maxDistance = rightSideVector.x - leftSideVector.x;
        rectTransform = this.GetComponent<RectTransform>();
    }

    void DrawLine() {
        float barLength = forcefieldCounter / forcefieldTarget * maxDistance;
        transform.localPosition = new Vector3(leftSideVector.x + (barLength / 2), transform.localPosition.y, transform.localPosition.z);
        transform.localScale = new Vector3(barLength / 100, transform.localScale.y, transform.localScale.z);
    }

    void UpdateColor() { 
        Color newColor;
        if(isForcefieldActive) {
            newColor = forcefieldActiveColor;
        } else {
            newColor = forcefieldInactiveColor;
        }
        this.gameObject.GetComponent<Image>().color = newColor;
        leftSide.GetComponent<Image>().color = newColor;
        rightSide.GetComponent<Image>().color = newColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        } else {
            forcefieldCounter = player.forcefieldCounter;
            forcefieldTarget = player.forcefieldTarget;

            if(isForcefieldActive != player.isForcefieldActive) {
                isForcefieldActive = player.isForcefieldActive;
                UpdateColor();
            }

            DrawLine();
        }
    }
}
