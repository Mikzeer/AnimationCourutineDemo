using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections.Generic;

public class CardCollectionFirebase : MonoBehaviour
{
    private const string cardsTable = "Cards";

    public async Task<List<CardDataRT>> GetAllCardCollectionLibrary()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        List<CardDataRT> allCardList = new List<CardDataRT>();

        await FirebaseDatabase.DefaultInstance.GetReference(cardsTable).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.Log("NoChild");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children)
                {
                    CardDataRT card = JsonUtility.FromJson<CardDataRT>(child.GetRawJsonValue());
                    allCardList.Add(card);
                }
            }
        });

        return allCardList;
    }
}