using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CardUIPointerHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform cardRectTansform;
    private Transform playerHandTransform;
    private GraphicRaycaster m_Raycaster;
    private CanvasGroup canvasGroup;
    int siblingIndex;

    public void Awake()
    {
        cardRectTansform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        playerHandTransform = transform.parent.GetComponent<Transform>();
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        cardRectTansform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        // eventData.delta = the amount the mouse move since the last frame
        cardRectTansform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(eventData, results);

        if (results.Count == 0)
        {
            // ESTO SIGNIFICA QUE NO CHOCO CONTRA NINGUN UI
            //Debug.Log("NO CHOCO CONTRA NINGUNA UI");
            cardRectTansform.SetParent(playerHandTransform);
        }
        else
        {
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("DropeableArea"))
                {
                    Debug.Log("Hit " + result.gameObject.name);
                    return;
                }

                // tengo 5 cartas C1 0  C2 1  C3 2  C4 3  C5 4
                // saco 1
                // ahora tengo 4 en la mano C1 0  C2 1  C3 2  C4 3  
                // cuando vuelvo a dropear esa carta en la mano
                // el index pasa de 4 a 5, entonces primero tengo que capturar el sibling index de la carta
                // carta en index 3 va a pasar a estar en el index 3, y la nueva carta va a pasar a estar en el new sibling mas 1

                if (result.gameObject.CompareTag("Card"))
                {
                    int newSiblingIndex = result.gameObject.transform.GetSiblingIndex();

                    cardRectTansform.SetParent(playerHandTransform);

                    //Debug.Log("siblingIndex" + siblingIndex);
                    Debug.Log("newSiblingIndex " + newSiblingIndex);

                    //result.gameObject.transform.SetSiblingIndex(siblingIndex);
                    //cardRectTansform.SetSiblingIndex(newSiblingIndex);

                    //cardRectTansform.SetSiblingIndex(newSiblingIndex + 1);
                    //result.gameObject.transform.SetSiblingIndex(newSiblingIndex);

                    canvasGroup.blocksRaycasts = true;
                    return;
                }
            }


        }
        //cardRectTansform.SetParent(playerHandTransform);
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //cardRectTansform.SetAsLastSibling();
        Debug.Log("OnPointerDown INDEX " + cardRectTansform.GetSiblingIndex());
        siblingIndex = cardRectTansform.GetSiblingIndex();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter, HACER MAS GRANDE LA CARTA Y PONERLA PRIMERO");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit, VOLVER A LA CARTA SU TAMANO Y POSICION ORIGINAL");
    }

}
