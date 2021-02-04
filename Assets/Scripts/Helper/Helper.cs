using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Events;

public static class Helper
{
    public static float GetPercent(float current, float maximum)
    {
        float percent = (current / maximum) * 100;

        return percent;
    }

    public static Vector3 GetMouseWorldPosition(Camera cam)
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, cam);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public static Vector3 GetCameraTopBorderYWorldPostion()
    {
       Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
       return screenBounds;
    }

    public static Vector3 GetCameraCenterWorldPositionWithZoffset()
    {
        Vector3 cameraCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, -Camera.main.transform.position.z));
        return cameraCenter;
    }

    public static TextMesh CreateWorldText(string text, Color col, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
    {
        GameObject go = new GameObject("World_Text", typeof(TextMesh));
        Transform trans = go.transform;
        trans.SetParent(parent, false);
        trans.localPosition = localPosition;
        TextMesh textMesh = go.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = col;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public static IEnumerator MoveFromTo(Transform objectToMove, Vector2 a, Vector2 b, float speed)
    {
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time

            objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        objectToMove.position = b;
    }

    public static bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public static bool IsMouseOverUIWithIgnores()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultsList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);

        for (int i = 0; i < raycastResultsList.Count; i++)
        {
            if (raycastResultsList[i].gameObject.GetComponent<MouseUIClickThrough>() != null)
            {
                raycastResultsList.RemoveAt(i);
                i--;
            }
        }

        return raycastResultsList.Count > 0;
    }

    private static DateTime UnixTimeStampToDateTimeSeconds(long unixTimeStamp)
    {
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
        return dtDateTime;
    }

    public static DateTime UnixTimeStampToDateTimeMiliseconds(long unixTimeStamp)
    {
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp);
        return dtDateTime;
    }

    public static long DateTimeToUnixTimeStampSeconds(DateTime dateTime)
    {
        long unixTimestamp = (Int32)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        return unixTimestamp;
    }

    public static Texture2D duplicateTexture(Texture2D source)
    {
        // Use RenderTexture. Put the source Texture2D into RenderTexture with 
        // Graphics.Blit then use Texture2D.ReadPixels to read the image from RenderTexture into the new Texture2D.
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear); // Default RGBA32

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    public static Texture2D duplicateTextureBadWay(Texture2D source)
    {
        //2.Use Texture2D.GetRawTextureData() + Texture2D.LoadRawTextureData():
        //You can't use GetPixels32() because the Texture2D is not readable. You were so close about using GetRawTextureData().
        //You failed when you used Texture2D.LoadImage() to load from GetRawTextureData().
        //Texture2D.LoadImage() is only used to load PNG / JPG array bytes not Texture2D array byte.
        //If you read with Texture2D.GetRawTextureData(), you must write with Texture2D.LoadRawTextureData() not Texture2D.LoadImage().
        //There will be no error with the code above in the Editor but there should be an error in standalone build. 
        //Besides, it should still work even with the error in the standalone build. I think that error is more like a warning.
        byte[] pix = source.GetRawTextureData();
        Texture2D readableText = new Texture2D(source.width, source.height, source.format, false);
        readableText.LoadRawTextureData(pix);
        readableText.Apply();
        return readableText;
    }

    public static Sprite GetSpriteFromByteArray(Byte[] bytes)
    {
        // create a Texture2D object that is used to stream data into Texture2D
        // No impota la medida que le asignemos a la textura ya que cuando ejecutamos el Load Image toma la medida real
        Texture2D texture = new Texture2D(10, 10);
        texture.LoadImage(bytes); // stream data into Texture2D
                                  // Create a Sprite, to Texture2D object basis
        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        return sp;
    }

    public static byte[] EncodeSpriteToByteArray(Sprite sp)
    {
        // EL SPRITE TIENE QUE TENER HABILITADO LO SIGUIENTE
        // /Advance/Read/Write Enable = true
        // /Default/Compression = None
        //public Image pruebaSprite;
        //byte[] spByte = EncodeSpriteToByteArray(pruebaSprite.sprite);


        // convert Texture
        Texture2D temp = sp.texture;
        // conversion to bytes
        byte[] photoByte = temp.EncodeToPNG();

        return photoByte;
    }

    public static void WriteTextureToPlayerPrefs(string tag, Texture2D tex)
    {
        // if texture is png otherwise you can use tex.EncodeToJPG().
        byte[] texByte = tex.EncodeToPNG();

        // convert byte array to base64 string
        string base64Tex = Convert.ToBase64String(texByte);

        // write string to playerpref
        PlayerPrefs.SetString(tag, base64Tex);
        PlayerPrefs.Save();
    }

    public static Texture2D ReadTextureFromPlayerPrefs(string tag)
    {
        // load string from playerpref
        string base64Tex = PlayerPrefs.GetString(tag, null);

        if (!string.IsNullOrEmpty(base64Tex))
        {
            // convert it to byte array
            byte[] texByte = System.Convert.FromBase64String(base64Tex);
            Texture2D tex = new Texture2D(2, 2);

            //load texture from byte array
            if (tex.LoadImage(texByte))
            {
                return tex;
            }
        }

        return null;
    }

    private static void SaveWithBinaryFormatter(string path, object objToSerialize)
    {
        BinaryFormatter bf = new BinaryFormatter();
        //(Application.persistentDataPath + "/playerInfo.dat"
        FileStream file = File.Create(path);
        bf.Serialize(file, objToSerialize);
        file.Close();
    }

    private static void LoadWithBianaryFormatter(string path)
    {
        //(Application.persistentDataPath + "/playerInfo.dat"
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);

            Dictionary<CardDataRT, int> QuantityOfEachCard = (Dictionary<CardDataRT, int>)bf.Deserialize(file);
        }
    }

    public static Vector3 KeepRectInsideScreen(RectTransform rect, Vector3 newPos, CanvasScaler canvasScaler)
    {
        float minX = (rect.rect.size.x / 2);
        float maxX = (canvasScaler.referenceResolution.x - minX);
        float minY = (rect.rect.size.y * 0.5f);
        float maxY = (canvasScaler.referenceResolution.y - minY);
        Vector3 posToReturn = new Vector3(Mathf.Clamp(newPos.x, minX, maxX), Mathf.Clamp(newPos.y, minY, maxY));
        //newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        //newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        return posToReturn;
    }

    public static bool IsEmptyString(string stringToCheck)
    {
        if (stringToCheck == "" || stringToCheck == string.Empty || stringToCheck == null) return true;
        return false;
    }
}

public static class HelperUI
{
    public static InputFiledDisplay CreateInputTextDisplay(GameObject inputPrefab, RectTransform parent)
    {
        GameObject inputAux = GameObject.Instantiate(inputPrefab, parent);
        InputFiledDisplay inputFiledDisplay = inputAux.GetComponent<InputFiledDisplay>();
        return inputFiledDisplay;
    }
}