using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICardDragAndDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler
{
    public Action OnDrop; 
    int numberTime = 0;
    GraphicRaycaster m_Raycaster;
    Vector3 startPosition;
    Image image; // LA IMAGEN EN MINIATURA
    Transform parentTransform; // EL PANEL DE CARDS
    Transform rootTransform; // EL CANVAS

    private void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        parentTransform = transform.parent.GetComponent<Transform>();
        rootTransform = transform.root;

        //Debug.Log(parentTransform.name);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        //worldPosition.z = 0;
        //startPosition = worldPosition;
        transform.SetParent(rootTransform);
        image = GetComponent<Image>();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Estoy Siendo Drageada");
        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        //worldPosition.z = 0;
        //transform.position = worldPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Estoy Siendo Clickeada " + eventData.position);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPosition.z = 0;
        startPosition = worldPosition;
        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        //Debug.Log("Estoy Siendo Clickeada World Position " + worldPosition);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("Me dejaron de clickear, number Time " + numberTime);
        numberTime = 0;


        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(eventData, results);


        if (results.Count == 0)
        {
            // ESTO SIGNIFICA QUE NO CHOCO CONTRA NINGUN UI
            // ENTONCES ES QUE LA TIRAMOS EN LA PANTALLA PARA PODER COMENZAR A USARLA
            transform.position = startPosition;
            transform.SetParent(parentTransform);

            OnDrop?.Invoke();
        }
        else
        {
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {

                //Debug.Log("Hit " + result.gameObject.name);
                //if (result.gameObject.CompareTag("Card"))
                //{
                //    Debug.Log("Hit " + result.gameObject.name);
                //    cardSelectionSystem.SelectCard(result.gameObject.transform);
                //    return;
                //}
            }
        }

        if (image != null)
        {
            image.raycastTarget = true;
        }

    }

    public void AddEventListenerToExecution(Action OnDrop)
    {
        this.OnDrop = OnDrop;
    }

    public void RemoveEventListeners()
    {
        OnDrop = null;

    }
}
