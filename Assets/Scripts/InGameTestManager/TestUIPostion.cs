using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUIPostion : MonoBehaviour
{
    public RectTransform uiToTestRectTransform;
    public Transform uiToTestTransform;
    public RectTransform canvasTransform;
    public RectTransform panelLayoutTransform;

    protected Canvas canvas; // Este canvas sirva para llevar al frente de todo siempre lo que drageamos
    protected CanvasScaler canvasScaler;

    private void Start()
    {
        // Dos maneras de buscar el Canvas, la segunda es mejor cuando se tiene diferentes Canvas
        if (canvas == null)
        {
            //canvas = transform.root.GetComponent<Canvas>();
            Transform trsCanvas = transform.parent;
            while (trsCanvas != null)
            {
                canvas = trsCanvas.GetComponent<Canvas>();
                if (canvas != null)
                {
                    canvasScaler = canvas.GetComponent<CanvasScaler>();
                    break;
                }
                trsCanvas = trsCanvas.parent;
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TestPosition();
        }
    }

    public void TestPosition()
    {
        //Debug.Log("uiToTestRectTransform.rect.anchoredPosition " + uiToTestRectTransform.anchoredPosition); // Posicion segun los Anchors
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        Debug.Log("canvasTransform.localPosition " + canvasTransform.localPosition);
        Debug.Log("panelLayoutTransform.localPosition " + panelLayoutTransform.localPosition);
        Debug.Log("uiToTestRectTransform.localPosition " + uiToTestRectTransform.localPosition); // Posicion dentro de la Pantalla/Parent
        Debug.Log("Screen Width " + screenWidth);
        Debug.Log("Screen Height " + screenHeight);
        Debug.Log("Half Screen Width " + screenWidth / 2 );
        Debug.Log("Half Screen Height " + screenHeight / 2);

        float canvasResoX = canvasScaler.referenceResolution.x;
        Vector2 mousePosition = Input.mousePosition / canvas.scaleFactor;

        // 960 - 560 - 390 = 10
        // 540 - 415 + 125 = 250

        // 960 - (-1 * -560) - (-1 * -390) = 
        // 540 - (-1 * -415) - (-1 * 125) = 

        float halfScreenWidth = screenWidth / 2 / canvas.scaleFactor;
        float halfScreenHeight = screenHeight / 2 / canvas.scaleFactor;
        float finalPositionX = halfScreenWidth - (-1 * panelLayoutTransform.localPosition.x) - (-1 * uiToTestRectTransform.localPosition.x);
        float finalPositionY = halfScreenHeight - (-1 * panelLayoutTransform.localPosition.y) - (-1 * uiToTestRectTransform.localPosition.y);
        Debug.Log("finalPositionX " + finalPositionX);
        Debug.Log("finalPositionY " + finalPositionY);


        //Debug.Log("uiToTestRectTransform.pivot " + uiToTestRectTransform.pivot);
        //Debug.Log("ribbonRect.rect.xMax " + uiToTestRectTransform.rect.xMax);
        //Debug.Log("ribbonRect.rect.xMin " + uiToTestRectTransform.rect.xMin);
        //Debug.Log("ribbonRect.rect.yMax " + uiToTestRectTransform.rect.yMax);
        //Debug.Log("ribbonRect.rect.yMin " + uiToTestRectTransform.rect.yMin);
        //Debug.Log("ribbonRect.rect.x " + uiToTestRectTransform.rect.x); 
        //Debug.Log("ribbonRect.rect.y " + uiToTestRectTransform.rect.y);
        //Debug.Log("ribbonRect Width " + uiToTestRectTransform.rect.width);
        //Debug.Log("ribbonRect Height " + uiToTestRectTransform.rect.height);

        //float rectWidth = (uiToTestRectTransform.anchorMax.x - uiToTestRectTransform.anchorMin.x) * Screen.width;
        //float rectHeight = (uiToTestRectTransform.anchorMax.y - uiToTestRectTransform.anchorMin.y) * Screen.height;
        //Vector2 position = new Vector2(uiToTestRectTransform.anchorMin.x * Screen.width, uiToTestRectTransform.anchorMin.y * Screen.height);
        //Debug.Log("position " + position);

        //Debug.Log("uiToTestTransform.position " + uiToTestTransform.position);
        //Debug.Log("uiToTestTransform.localPosition " + uiToTestTransform.localPosition);
        //Debug.Log("ribbonRect.rect.size " + ribbonRect.rect.size);
        //float distanceRibbonFromAnchorX = ribbonRect.anchoredPosition.x;
        //float distanceRibbonFromAnchorY = ribbonRect.anchoredPosition.y * -1;
        //float ribbonPixelSizeX = ribbonRect.rect.size.x; // ESTO ESTA EN PIXELES
        //float diference = screenWidth - ribbonPixelSizeX;
        //float ribbonRectWidth = uiToTestRectTransform.rect.width;
        //float ribbonRectHeight = uiToTestRectTransform.rect.height;
        //Debug.Log("ribbonRect Width " + ribbonRectWidth);
        //Debug.Log("ribbonRect Height " + ribbonRectHeight);
        //Debug.Log("ribbonRect Half Width " + ribbonRectWidth / 2);
        //Debug.Log("ribbonRect Half Height " + ribbonRectHeight / 2);
        //Debug.Log("ribbonRect.rect.xMax " + ribbonRect.rect.xMax);
        //Debug.Log("ribbonRect.rect.xMin " + ribbonRect.rect.xMin);
        //Debug.Log("ribbonRect.rect.yMax " + ribbonRect.rect.yMax);
        //Debug.Log("ribbonRect.rect.yMin " + ribbonRect.rect.yMin);
        //Debug.Log("ribbonRect.rect.x " + ribbonRect.rect.x); // 
        //Debug.Log("ribbonRect.rect.y " + ribbonRect.rect.y);// 
        //Debug.Log("ribbonRect.rect.center " + ribbonRect.rect.center);
        //Debug.Log("ribbonRect.rect.max " + ribbonRect.rect.max); // 
        //Debug.Log("ribbonRect.rect.min " + ribbonRect.rect.min);// 
        //Debug.Log("ribbonRect.rect.position " + ribbonRect.rect.position);
        //Debug.Log("ribbonRect.rect.size " + ribbonRect.rect.size); // Width and Heigth of the card
        //Debug.Log("ribbonRect.rect.anchoredPosition " + ribbonRect.anchoredPosition); // Posicion segun los Anchors
        //Debug.Log("ribbonRect.rect.localPosition " + ribbonRect.localPosition); // Posicion dentro de la Pantalla/Parent
        //Vector2 localPoint;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRect, ribbonRect.anchoredPosition, Camera.main, out localPoint);
        //Debug.Log("localPoint " + localPoint);

    }
}
