using MikzeerGame;
using PositionerDemo;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : SimpleClickeableUI
{
    public CardDisplay cardDisplay;

    public RectTransform panelRect;
    public RectTransform rootRect;
    public RectTransform panelsRibbonsRect;
    RectTransform cardRect;
    RectTransform ribbonRect;
    RectTransform parentRibbonRect;
    int screenWidth;
    int screenHeight;
    float cardRectWidth;
    float cardRectHeight;
    private void Start()
    {
        cardRect = cardDisplay.GetComponent<RectTransform>();
        ribbonRect = GetComponent<RectTransform>();
        parentRibbonRect = ribbonRect.parent.GetComponent<RectTransform>();
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        cardRectWidth = cardRect.rect.width;
        cardRectHeight = cardRect.rect.height;
    }

    protected override void OnSuccesfulPointerEnter()
    {
        //ShowParentRibbonInfo();
        float cardHalfSizeX = cardRect.rect.size.x / 2;
        float panelSizeX = rootRect.rect.size.x;
        float canvasResoX = canvasScaler.referenceResolution.x;
        float newPosX = canvasResoX - panelSizeX - (cardHalfSizeX * 1.3f);

        Vector2 mousePosition = Input.mousePosition / canvas.scaleFactor;
        Vector3 locPoint = new Vector3(newPosX, mousePosition.y, cardRect.localPosition.z);
        Vector3 clampedPostion = KeepFullyOnScreen(cardRect, locPoint, canvasScaler);
        cardRect.anchoredPosition = clampedPostion;
    }

    public void PPP()
    {
        //CardContainerPanel == panelsRibbonsRect
        // PRIMERO TENGO QUE CONSEGUIR LA POSICION REAL DEL ribbonRect
        // si el parentRibbonRect.Size.Y es mayor al Size del PanelsRibbonsRect
        Vector2 parentSize = panelsRibbonsRect.rect.size;

        // Calculate our corners relative to parent
        Vector2 componentMultiplyAnchorMin = new Vector2(ribbonRect.anchorMin.x * parentSize.x, ribbonRect.anchorMin.y * parentSize.y);
        Vector2 componentMultiplyAnchorMax = new Vector2(ribbonRect.anchorMax.x * parentSize.x, ribbonRect.anchorMax.y * parentSize.y);
        Vector2 ourMinCorner = componentMultiplyAnchorMin + ribbonRect.offsetMin;
        Vector2 ourMaxCorner = componentMultiplyAnchorMax + ribbonRect.offsetMax;
        //Debug.Log("parentSize" + parentSize);
        //Debug.Log("componentMultiplyAnchorMin" + componentMultiplyAnchorMin);
        //Debug.Log("componentMultiplyAnchorMax" + componentMultiplyAnchorMax);
        //Debug.Log("ourMinCorner" + ourMinCorner);
        //Debug.Log("ourMaxCorner" + ourMaxCorner);

        Vector2 mousePosition = Input.mousePosition / canvas.scaleFactor;
        Debug.Log(mousePosition);
        //Vector3[] localCorners = new Vector3[4];
        //Vector3[] worldCorners = new Vector3[4];

        //ribbonRect.GetWorldCorners(worldCorners);
        //ribbonRect.GetLocalCorners(localCorners);


        //for (int i = 0; i < 4; i++)
        //{
        //    Debug.Log("World Corners " + i + ": " + worldCorners[i]);
        //    Vector3 screenPos = Camera.main.WorldToScreenPoint(worldCorners[i]);
        //    Debug.Log("screenPos " + i + ": " + screenPos);
        //    //Debug.Log("Local Corners " + i + ": " + localCorners[i]);
        //}
    }

    public void ShowScreenInfo()
    {
        Debug.Log("Screen Width " + screenWidth);
        Debug.Log("Screen Height " + screenHeight);
        Debug.Log("Half Screen Width " + screenWidth / 2);
        Debug.Log("Half Screen Height " + screenHeight / 2);
    }

    public void ShowLimitScreenAndCardInfo()
    {
        float maxPoYForCard = (screenHeight / 2) - (cardRectHeight / 2);
        float minPoYForCard = -((screenHeight / 2) - (cardRectHeight / 2));
        float maxPoXForCard = (screenWidth / 2) - (cardRectWidth / 2);
        float minPoXForCard = -((screenWidth / 2) - (cardRectWidth / 2));
        Debug.Log("Max Pos X For Card " + maxPoXForCard);
        Debug.Log("Min Pos X For Card " + minPoXForCard);
        Debug.Log("Max Pos Y For Card " + maxPoYForCard);
        Debug.Log("Min Pos Y For Card " + minPoYForCard);
    }

    public void ShowRibbonInfo()
    {
        //Debug.Log("ribbonRect.rect.anchoredPosition " + ribbonRect.anchoredPosition); // Posicion segun los Anchors
        //Debug.Log("ribbonRect.rect.localPosition " + ribbonRect.localPosition); // Posicion dentro de la Pantalla/Parent
        //Debug.Log("ribbonRect.rect.size " + ribbonRect.rect.size);
        //float distanceRibbonFromAnchorX = ribbonRect.anchoredPosition.x;
        //float distanceRibbonFromAnchorY = ribbonRect.anchoredPosition.y * -1;
        //float ribbonPixelSizeX = ribbonRect.rect.size.x; // ESTO ESTA EN PIXELES
        //float diference = screenWidth - ribbonPixelSizeX;
        float ribbonRectWidth = ribbonRect.rect.width;
        float ribbonRectHeight = ribbonRect.rect.height;
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
        Debug.Log("ribbonRect.rect.anchoredPosition " + ribbonRect.anchoredPosition); // Posicion segun los Anchors
        Debug.Log("ribbonRect.rect.localPosition " + ribbonRect.localPosition); // Posicion dentro de la Pantalla/Parent

        Vector2 localPoint;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRect, ribbonRect.anchoredPosition, Camera.main, out localPoint);
        Debug.Log("localPoint " + localPoint);

    }

    public void ShowPanelInfo()
    {
        Debug.Log("panelRect.rect.localPosition " + panelRect.localPosition); // Posicion dentro de la Pantalla/Parent
        //Debug.Log("panelRect.rect.anchoredPosition " + panelRect.anchoredPosition);
        //panelsRibbonsRect
        Debug.Log("panelsRibbonsRect.rect.localPosition " + panelsRibbonsRect.localPosition); // Posicion dentro de la Pantalla/Parent
        //Debug.Log("panelsRibbonsRect.rect.anchoredPosition " + panelsRibbonsRect.anchoredPosition);
        //var screenPoint = Input.mousePosition;
        //Vector2 localPoint;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, panelRect.anchoredPosition, Camera.main, out localPoint);
        //Debug.Log("localPoint " + localPoint);
        //localPoint.x -= ribbonPixelSizeX / 2;

    }

    public void ShowParentRibbonInfo()
    {
        Debug.Log("parentRibbonRect.rect.anchoredPosition " + parentRibbonRect.anchoredPosition); // Posicion segun los Anchors
        Debug.Log("parentRibbonRect.rect.localPosition " + parentRibbonRect.localPosition); // Posicion dentro de la Pantalla/Parent
        Debug.Log("parentRibbonRect.rect.size " + parentRibbonRect.rect.size); // Width and Heigth of the card
        float parentRbLocalPosY = parentRibbonRect.localPosition.y;
        Vector3[] localCorners = new Vector3[4];
        Vector3[] worldCorners = new Vector3[4];

        parentRibbonRect.GetWorldCorners(worldCorners);
        parentRibbonRect.GetLocalCorners(localCorners);
        Debug.Log("Local Corners " + 1 + ": " + localCorners[0]);
        for (int i = 0; i < 4; i++)
        {
            //Debug.Log("World Corners " + i + ": " + worldCorners[i]);
            //Debug.Log("Local Corners " + i + ": " + localCorners[i]);
        }
    }

    private void ShowCardDisplayInfo()
    {        
        //Debug.Log("Card Rect Width " + cardRectWidth);
        //Debug.Log("Card Rect Height " + cardRectHeight);
        //Debug.Log("Card Rect Half Width " + cardRectWidth / 2);
        //Debug.Log("Card Rect Half Height " + cardRectHeight / 2);
        //Debug.Log("cardRect.rect.xMax " + cardRect.rect.xMax); 
        //Debug.Log("cardRect.rect.xMin " + cardRect.rect.xMin);
        //Debug.Log("cardRect.rect.yMax " + cardRect.rect.yMax);
        //Debug.Log("cardRect.rect.yMin " + cardRect.rect.yMin);
        //Debug.Log("cardRect.rect.x " + cardRect.rect.x); // 
        //Debug.Log("cardRect.rect.y " + cardRect.rect.y);// 
        //Debug.Log("cardRect.rect.center " + cardRect.rect.center);
        //Debug.Log("cardRect.rect.max " + cardRect.rect.max); // 
        //Debug.Log("cardRect.rect.min " + cardRect.rect.min);// 
        //Debug.Log("cardRect.rect.position " + cardRect.rect.position);
        //Debug.Log("cardRect.rect.size " + cardRect.rect.size); // Width and Heigth of the card
        //Debug.Log("cardRect.rect.anchoredPosition " + cardRect.anchoredPosition); // Posicion segun los Anchors
        //Debug.Log("cardRect.rect.localPosition " + cardRect.localPosition); // Posicion dentro de la Pantalla/Parent
    }

    Vector3 KeepFullyOnScreen(RectTransform rect, Vector3 newPos, CanvasScaler canvasScaler)
    {
        float minX = (rect.rect.size.x / 2);
        float maxX = (canvasScaler.referenceResolution.x - (rect.rect.size.x / 2));
        float minY = (rect.rect.size.y / 2);
        float maxY = (canvasScaler.referenceResolution.y - (rect.rect.size.y * 0.5f));
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        return newPos;
    }

}