using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardPackUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler, IInitializePotentialDragHandler
{
    private Canvas canvas;
    private RectTransform cardRectTansform;
    private Transform playerStartParent;
    private GraphicRaycaster m_Raycaster;
    private CanvasGroup canvasGroup;
    bool isDragin = false;
    public static bool isACardDraging = false;
    int indexBeforeEnter = 0;
    bool isPointerOverCard = false;
    bool isComingFromDrag = false;
    Vector2 startAnchorPosition;

    public Image GlowImage;
    public Color32 GlowColor;
    private bool allowedToOpen = false;


    PackOpeningArea packOpeningArea;

    public void Awake()
    {
        cardRectTansform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (transform.parent != null)
        {
            playerStartParent = transform.parent.GetComponent<Transform>();
        }
        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        // Dos maneras de buscar el Canvas, la segunda es mejor cuando se tiene diferentes Canvas
        //canvas = transform.root.GetComponent<Canvas>();
        if (canvas == null)
        {
            Transform trsCanvas = transform.parent;
            while (trsCanvas != null)
            {
                canvas = trsCanvas.GetComponent<Canvas>();
                if (canvas != null)
                {
                    break;
                }

                trsCanvas = trsCanvas.parent;
            }
        }
    }

    public void SetCardPackOpeningArea(PackOpeningArea packOpeningArea)
    {
        this.packOpeningArea = packOpeningArea;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isACardDraging) return;

        if (isDragin == false)
        {
            isPointerOverCard = true;
            indexBeforeEnter = cardRectTansform.GetSiblingIndex();
            startAnchorPosition = cardRectTansform.anchoredPosition;
            playerStartParent = transform.parent;
            //Vector2 offset = new Vector2(80, 0);
            //cardRectTansform.anchoredPosition += offset;
        }

        if (allowedToOpen)
            GlowImage.DOColor(GlowColor, 0.5f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (allowedToOpen)
        {
            // prevent from opening again
            allowedToOpen = false;
            packOpeningArea.OnCardPackOpen(cardRectTansform);
        }

        //siblingIndex = cardRectTansform.GetSiblingIndex();
        //siblingIndex = indexBeforeEnter;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (packOpeningArea.AllowedToDragAPack == false)
        {
            return;
        }

        if (isACardDraging)
        {
            return;
        }

        cardRectTansform.SetParent(canvas.transform);
        cardRectTansform.SetAsLastSibling();
        isACardDraging = true;
        isDragin = true;
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        // eventData.delta = the amount the mouse move since the last frame
        cardRectTansform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //canvasGroup.blocksRaycasts = false;
        isACardDraging = false;

        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(eventData, results);

        if (results.Count == 0)
        {
            Debug.Log("NO CHOCO CONTRA NINGUNA UI");
            OnCancelDrag();
            return;
        }
        else
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("OpenPackArea"))
                {
                    Debug.Log("Hit " + result.gameObject.name);
                    OnSuccesfullDrop();
                    isDragin = false;
                    return;
                }
            }
        }
        OnCancelDrag();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // turn the glow Off
        GlowImage.DOColor(Color.clear, 0.5f);

        if (isACardDraging)
        {
            return;
        }

        if (isComingFromDrag)
        {
            isComingFromDrag = false;
            isPointerOverCard = false;
            return;
        }

        if (isDragin == false)
        {
            //Debug.Log("ISDRAGIN IS FALSE");
            isPointerOverCard = false;
            cardRectTansform.SetParent(playerStartParent);
            cardRectTansform.SetSiblingIndex(indexBeforeEnter);
        }
    }

    public void OnCancelDrag()
    {
        transform.DOLocalMove(startAnchorPosition, 1f).OnComplete(() =>
        {
            packOpeningArea.AllowedToDragAPack = true;
            cardRectTansform.SetParent(playerStartParent);
            cardRectTansform.SetSiblingIndex(indexBeforeEnter);
            canvasGroup.blocksRaycasts = true;
            isDragin = false;
        });
    }

    public void OnSuccesfullDrop()
    {
        // snap the pack to the center of the pack opening area
        transform.DOMove(packOpeningArea.transform.position, 0.5f).OnComplete(() =>
        {
            // enable opening on click
            AllowToOpenThisPack();
        });
    }

    public void AllowToOpenThisPack()
    {
        allowedToOpen = true;
        packOpeningArea.AllowedToDragAPack = false;
        // Disable back button so that player can not exit the pack opening screen while he has not opened a pack
        packOpeningArea.BackButton.interactable = false;
    }

}
