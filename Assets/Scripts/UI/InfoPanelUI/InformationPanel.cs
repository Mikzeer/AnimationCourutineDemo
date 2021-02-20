using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InformationPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoTextMesh = default;
    [SerializeField] private RectTransform infoPanelRect = default;
    protected Canvas canvas; // Este canvas sirva para llevar al frente de todo siempre lo que drageamos
    protected CanvasScaler canvasScaler;
    protected RectTransform canvasTransform;

    void Awake()
    {
        infoPanelRect = GetComponent<RectTransform>();

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
                    canvasTransform = canvas.GetComponent<RectTransform>();
                    return;
                }
            }

            while (trsCanvas != null)
            {
                canvas = trsCanvas.GetComponent<Canvas>();
                if (canvas != null)
                {
                    canvasScaler = canvas.GetComponent<CanvasScaler>();
                    canvasTransform = canvas.GetComponent<RectTransform>();
                    break;
                }
                trsCanvas = trsCanvas.parent;
            }
        }
    }

    public void SetInformationText(string infoText, RectTransform uiMouseOverTest)
    {
        // ESTA VERSION SIRVE PARA CUANDO ESTAN LOS ANCHORS JUNTOS

        // PRIMERO HAY QUE ACTIVARLO SINO NOS TIRA ERROR AL OBTENER LOS RENDER VALUES
        SetActive(true);

        // 1 - INSTANCIAMOS EL OBJETO Y SETEAMOS TODAS SUS PROPIEDADES PARA DARLE EL TAMANO CORRECTO FINAL
        // EL OBJETIVO DE ESTO ES QUE EL OBJETO TENGA EL TAMANO REAL QUE SE VA A MOSTRAR PARA OBTENER SUS LIMITES
        infoTextMesh.SetText(infoText);
        // FORZAMOS EL UPDATEO YA QUE SI NO EL CALCULO RECIEN DEBERIA HACERSE EN EL "SIGUIENTE FRAME" CUANDO TENGA EL TAMAÑO REAL
        infoTextMesh.ForceMeshUpdate();

        Vector2 textSize = infoTextMesh.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);

        infoPanelRect.sizeDelta = textSize + paddingSize;
        infoTextMesh.ForceMeshUpdate();

        // 4 - NECESITAMOS LA POSICION DEL OBJETO QUE ESTAMOS SOBRE.... uiMouseOverTest
        Vector2 anchoredPos = uiMouseOverTest.anchoredPosition;

        float anchorMinXUIMouseOver = uiMouseOverTest.anchorMin.x;
        float anchorManXUIMouseOver = uiMouseOverTest.anchorMax.x;
        float anchorMinYUIMouseOver = uiMouseOverTest.anchorMin.y;
        float anchorManYUIMouseOver = uiMouseOverTest.anchorMax.y;
        // convertir esos anchor viewport to screen point
        Vector3 origin = Camera.main.ViewportToScreenPoint(new Vector3(anchorMinXUIMouseOver, anchorMinYUIMouseOver, 0));
        //Vector3 extent = Camera.main.ViewportToScreenPoint(new Vector3(anchorManXUIMouseOver, anchorManYUIMouseOver, 0));

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
        //float posYRepositionFromUIObject = diferenceBetweenAnchorAndUIPositionY + (mouseOverTotalHeight / 2);


        // SEGUN EL LADO DE LA PANTALLA QUE ESTEMOS LO INSTANCIAMOS A LA IZQUIERDA O LA DERECHA
        float finalPosX = diferenceBetweenAnchorAndUIPositionX + (mouseOverTotalWidth / 2) + (infoPanelRect.rect.size.x / 2);
        if (diferenceBetweenAnchorAndUIPositionX > (canvasScaler.referenceResolution.x / 2))
        {
            finalPosX = diferenceBetweenAnchorAndUIPositionX - (mouseOverTotalWidth / 2) - (infoPanelRect.rect.size.x / 2);
        }

        // centrado al objeto que estamos sobre en la altura
        float finalPosY = diferenceBetweenAnchorAndUIPositionY;

        // OBTENEMOS EL TAMANO ACTUAL DEL RECT DEL CANVAS
        float CanvasWidth = canvasTransform.rect.width;
        float CanvasHeight = canvasTransform.rect.height;


        // 6 - OBTENGO EL TAMANO MIN/MAX DE POSICIONAMIENTO EN LA PANTALLA
        // ESTO SE OBTIENE SEGUN EL TAMANO DEL OBJETO QUE QUEREMOS POSICIONAR Y LA RESOLUCION DEL SCALER
        float minX = (infoPanelRect.rect.size.x / 2);
        /*float maxX = (canvasScaler.referenceResolution.x - minX);*/ // ESTO SIEMPRE VA A TRABAJAR CON UNA RESOLUCION DE REFERENCIA... LO QUE NECESITAMOS ES LA ACTUAL RESOLUTION
        float maxX = (CanvasWidth - minX);

        float minY = (infoPanelRect.rect.size.y * 0.5f);
        //float maxY = (canvasScaler.referenceResolution.y - minY);
        float maxY = (CanvasHeight - minY);
        // DE ESTA MANERA GARANTIZAMOS QUE EL OBJETO NO SE VAYA DEL MAXIMO DE LA PANTALLA
        Vector3 posToReturn = new Vector3(Mathf.Clamp(finalPosX, minX, maxX), Mathf.Clamp(finalPosY, minY, maxY));
        infoPanelRect.anchoredPosition = posToReturn;
    }

    public void SetInformationText(string infoText, RectTransform uiMouseOverTest, Vector2 anchoredPos)
    {
        // ESTA VERSION SIRVE PARA CUANDO ESTAN LOS ANCHORS JUNTOS
        // PRIMERO HAY QUE ACTIVARLO SINO NOS TIRA ERROR AL OBTENER LOS RENDER VALUES
        SetActive(true);

        // 1 - INSTANCIAMOS EL OBJETO Y SETEAMOS TODAS SUS PROPIEDADES PARA DARLE EL TAMANO CORRECTO FINAL
        // EL OBJETIVO DE ESTO ES QUE EL OBJETO TENGA EL TAMANO REAL QUE SE VA A MOSTRAR PARA OBTENER SUS LIMITES
        infoTextMesh.SetText(infoText);
        // FORZAMOS EL UPDATEO YA QUE SI NO EL CALCULO RECIEN DEBERIA HACERSE EN EL "SIGUIENTE FRAME" CUANDO TENGA EL TAMAÑO REAL
        infoTextMesh.ForceMeshUpdate();

        Vector2 textSize = infoTextMesh.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);

        infoPanelRect.sizeDelta = textSize + paddingSize;
        infoTextMesh.ForceMeshUpdate();

        // 5 - NECESITAMOS EL TAMANO DEL OBJETO SOBRE EL CUAL TENEMOS EL MOUSE OVER
        float mouseOverTotalWidth = uiMouseOverTest.rect.size.x;

        // SEGUN EL LADO DE LA PANTALLA QUE ESTEMOS LO INSTANCIAMOS A LA IZQUIERDA O LA DERECHA
        float finalPosX = anchoredPos.x + (mouseOverTotalWidth / 2) + (infoPanelRect.rect.size.x / 2);
        if (anchoredPos.x > (canvasTransform.rect.width / 2))
        {
            finalPosX = anchoredPos.x - (mouseOverTotalWidth / 2) - (infoPanelRect.rect.size.x / 2);
        }

        // centrado al objeto que estamos sobre en la altura
        float finalPosY = anchoredPos.y;

        // OBTENEMOS EL TAMANO ACTUAL DEL RECT DEL CANVAS
        float CanvasWidth = canvasTransform.rect.width;
        float CanvasHeight = canvasTransform.rect.height;

        // 6 - OBTENGO EL TAMANO MIN/MAX DE POSICIONAMIENTO EN LA PANTALLA
        // ESTO SE OBTIENE SEGUN EL TAMANO DEL OBJETO QUE QUEREMOS POSICIONAR Y LA RESOLUCION DEL SCALER
        float minX = (infoPanelRect.rect.size.x / 2);        
        float maxX = (CanvasWidth - minX);
        float minY = (infoPanelRect.rect.size.y * 0.5f);
        float maxY = (CanvasHeight - minY);
        // DE ESTA MANERA GARANTIZAMOS QUE EL OBJETO NO SE VAYA DEL MAXIMO DE LA PANTALLA
        Vector3 posToReturn = new Vector3(Mathf.Clamp(finalPosX, minX, maxX), Mathf.Clamp(finalPosY, minY, maxY));
        infoPanelRect.anchoredPosition = posToReturn;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}