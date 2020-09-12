using UnityEngine;
namespace PositionerDemo
{
    public static class KimbokoUtils
    {
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
    }

}

