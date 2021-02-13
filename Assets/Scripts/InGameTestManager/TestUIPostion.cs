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

    public Vector2 newAnchoredPosition;
    public Vector3 newLocalPosition;
    public bool isLocal;
    public string txtToSet;
    public InfoPanel infoPanel;
    public RectTransform uiMouseOverTest;
    private void Start()
    {
        // Dos maneras de buscar el Canvas, la segunda es mejor cuando se tiene diferentes Canvas
        if (canvas == null)
        {
            //canvas = transform.root.GetComponent<Canvas>();
            Transform trsCanvas = transform.parent;
            if (trsCanvas == null)
            {
                canvas = transform.GetComponent<Canvas>();
                if (canvas != null)
                {
                    canvasScaler = canvas.GetComponent<CanvasScaler>();
                    return;
                }
            }

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
            //TestPosition();
            ChangePosition();
            //SetText();
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

    public void ChangePosition()
    {
        // ESTA VERSION SIRVE PARA CUANDO ESTANA LOS ANCHORS JUNTOS


        // 1 - CONSEGUIMOS LA POSICION EN FORMATO VIEWPORT(0 TO 1) DEL MOUSE.
        Vector3 viewPortMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);


        // 2 - INSTANCIAMOS EL OBJETO Y SETEAMOS TODAS SUS PROPIEDADES PARA DARLE EL TAMANO CORRECTO FINAL
        // EL OBJETIVO DE ESTO ES QUE EL OBJETO TENGA EL TAMANO REAL QUE SE VA A MOSTRAR PARA OBTENER SUS LIMITES


        // 3 - SACAMOS LA DISTANCIA ENTRE SUS ANCHORS
        // CON ESTO SABEMOS LA DISTANCIA QUE DEBE HABER ENTRE SUS ANCHORS DESDE DONDE SE PONGA EL MINIMO DE CADA UNO
        float difX = uiToTestRectTransform.anchorMax.x - uiToTestRectTransform.anchorMin.x;
        float difY = uiToTestRectTransform.anchorMax.y - uiToTestRectTransform.anchorMin.y;


        // 4 - NECESITAMOS LA POSICION DEL OBJETO QUE ESTAMOS SOBRE.... uiMouseOverTest
        Vector2 anchoredPos = uiMouseOverTest.anchoredPosition;

        float anchorMinXUIMouseOver = uiMouseOverTest.anchorMin.x;
        float anchorManXUIMouseOver = uiMouseOverTest.anchorMax.x;
        float anchorMinYUIMouseOver = uiMouseOverTest.anchorMin.y;
        float anchorManYUIMouseOver = uiMouseOverTest.anchorMax.y;
        // convertir esos anchor viewport to screen point
        Vector3 origin = Camera.main.ViewportToScreenPoint(new Vector3(anchorMinXUIMouseOver, anchorMinYUIMouseOver, 0));
        Vector3 extent = Camera.main.ViewportToScreenPoint(new Vector3(anchorManXUIMouseOver, anchorManYUIMouseOver, 0));

        // DE ESTA PUTA MANERA HORRIBLE, OBTENEMOS LA POSICION EN SCREEN POINT DE LOS ANCHORS DE LA UI MOUSE OVER CORRECTAMENTE
        origin = origin / canvas.scaleFactor;

        // ESTAS SON LAS POSICION EN SCREEN POINT FINALES DEL OBJETO DE LA UI SOBRE EL CUAL TENEMOS EL MOUSE OVER PARA MOSTRAR LA INFORMACION
        float diferenceBetweenAnchorAndUIPositionX = (origin.x) - (-1 * anchoredPos.x);
        float diferenceBetweenAnchorAndUIPositionY = (origin.y) - (-1 * anchoredPos.y);


        // 5 - NECESITAMOS EL TAMANO DEL OBJETO SOBRE EL CUAL TENEMOS EL MOUSE OVER
        float mouseOverTotalWidth = uiMouseOverTest.rect.size.x;
        float mouseOverTotalHeight = uiMouseOverTest.rect.size.y;

        // ACA DEPENDE DE QUE LADO DE LA PANTALLA ESTE IZQUIERDA O DERECHA, VAMOS A POSICIONAR EL CARTEL DE INFORMACION DEL LADO CONTRARIO
        // POR ENDE, SI ESTAMOS DEL LADO IZQUIERDO DE LA PANTALLA NUESTRO CARTEL SE VA A POSICIONAR A LA DERECHA DEL OBJETO QUE ESTAMOS SOBRE
        // SI ESTAMOS DEL LADO DERECHO DE LA PANTALLA, NUESTRO CARTEL SE VA A POSICIONAR A LA IZQUIERDA DEL OBJETO QUE ESTAMOS SOBRE
        float posXRepositionFromUIObject = diferenceBetweenAnchorAndUIPositionX + (mouseOverTotalWidth / 2);
        float posYRepositionFromUIObject = diferenceBetweenAnchorAndUIPositionY + (mouseOverTotalHeight / 2);


        // SEGUN EL LADO DE LA PANTALLA QUE ESTEMOS LO INSTANCIAMOS A LA IZQUIERDA O LA DERECHA
        float finalPosX = diferenceBetweenAnchorAndUIPositionX + (mouseOverTotalWidth / 2) + (uiToTestRectTransform.rect.size.x / 2);
        if (diferenceBetweenAnchorAndUIPositionX > (canvasScaler.referenceResolution.x / 2))
        {
            finalPosX = diferenceBetweenAnchorAndUIPositionX - (mouseOverTotalWidth / 2) - (uiToTestRectTransform.rect.size.x / 2);
        }

        // centrado al objeto que estamos sobre en la altura
        float finalPosY = diferenceBetweenAnchorAndUIPositionY;



        // 6 - OBTENGO EL TAMANO MIN/MAX DE POSICIONAMIENTO EN LA PANTALLA
        // ESTO SE OBTIENE SEGUN EL TAMANO DEL OBJETO QUE QUEREMOS POSICIONAR Y LA RESOLUCION DEL SCALER
        float minX = (uiToTestRectTransform.rect.size.x / 2);
        float maxX = (canvasScaler.referenceResolution.x - minX);
        float minY = (uiToTestRectTransform.rect.size.y * 0.5f);
        float maxY = (canvasScaler.referenceResolution.y - minY);
        // DE ESTA MANERA GARANTIZAMOS QUE EL OBJETO NO SE VAYA DEL MAXIMO DE LA PANTALLA
        //Vector3 posToReturn = new Vector3(Mathf.Clamp(newAnchoredPosition.x, minX, maxX), Mathf.Clamp(newAnchoredPosition.y, minY, maxY));
        Vector3 posToReturn = new Vector3(Mathf.Clamp(finalPosX, minX, maxX), Mathf.Clamp(finalPosY, minY, maxY));
        uiToTestRectTransform.anchoredPosition = posToReturn;
    }

    public void SetText(Vector2 anchoredPos)
    {
        infoPanel.SetText(txtToSet, anchoredPos, canvasScaler);
    }
}