using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Screen Reference")]
    [SerializeField] private GameObject ScreenContent;

    [Header("Pack Prefab")]
    [SerializeField] private GameObject PackPrefab;

    [Header("Rect Placement")]
    [SerializeField] private Transform PacksParent;
    [SerializeField] private Transform InitialPackSpot;
    [SerializeField] private float PosXRange = 4f;
    [SerializeField] private float PosYRange = 8f;
    [SerializeField] private float RotationRange = 10f;
    [SerializeField] private PackOpeningArea OpeningArea;
    private float packPlacementOffset = -0.01f;

    [Header("Money And Price")]
    [SerializeField] private Text MoneyText;
    [SerializeField] private GameObject MoneyHUD;
    [SerializeField] private int PackPrice;
    [SerializeField] private int StartingAmountOfMoney = 1000;
    public int PacksCreated { get; set; } // ESTO ES UN INT PARA LOS PACKS COMPRADOS PERO NO ABIERTOS
    private int money;
    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            MoneyText.text = money.ToString();
        }
    }

    #region SINGLETON

    [SerializeField] protected bool dontDestroy;

    private static ShopManager instance;
    public static ShopManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ShopManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<ShopManager>();
                }
            }
            return instance;
        }
    }

    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this as ShopManager;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        HideScreen();

        if (PlayerPrefs.HasKey("UnopenedPacks"))
        {
            Debug.Log("UnopenedPacks: " + PlayerPrefs.GetInt("UnopenedPacks"));
            StartCoroutine(GivePacks(PlayerPrefs.GetInt("UnopenedPacks"), true));
        }
        LoadMoneyFromPlayerPrefs();
    }

    public IEnumerator GivePacks(int NumberOfPacks, bool instant = false)
    {
        for (int i = 0; i < NumberOfPacks; i++)
        {
            GameObject newPack = Instantiate(PackPrefab, PacksParent);
            Vector3 localPositionForNewPack = new Vector3(Random.Range(-PosXRange, PosXRange), Random.Range(-PosYRange, PosYRange), PacksCreated * packPlacementOffset);
            newPack.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(-RotationRange, RotationRange));
            PacksCreated++;

            // make this pack appear on top of all the previous packs using PacksCreated;
            //newPack.GetComponentInChildren<Canvas>().sortingOrder = PacksCreated;
            newPack.GetComponent<CardPackUI>().SetCardPackOpeningArea(OpeningArea);
            if (instant)
                newPack.transform.localPosition = localPositionForNewPack;
            else
            {
                newPack.transform.position = InitialPackSpot.position;
                newPack.transform.DOLocalMove(localPositionForNewPack, 0.5f);
                yield return new WaitForSeconds(0.5f);
            }
        }
        yield break;
    }

    public void LoadMoneyFromPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("Money"))
            Money = PlayerPrefs.GetInt("Money");
        else
            Money = StartingAmountOfMoney;  // default value of dust to give to player
    }

    public void SaveMoneyToPlayerPrefs()
    {
        PlayerPrefs.SetInt("Money", money);
    }

    private void LoadWithBianaryFormatter()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfoMoney.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfoMoney.dat", FileMode.Open);

            Money = (int)bf.Deserialize(file);
        }
        else
        {
            Money = StartingAmountOfMoney;
        }
    }

    private void SaveWithBinaryFormatter()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfoMoney.dat");
        bf.Serialize(file, Money);
        file.Close();
    }

    void OnApplicationQuit()
    {
        SaveMoneyToPlayerPrefs();

        PlayerPrefs.SetInt("UnopenedPacks", PacksCreated);
    }

    public void BuyPack()
    {
        if (money >= PackPrice)
        {
            Money -= PackPrice;
            StartCoroutine(GivePacks(1));
        }
    }

    public void ShowScreen()
    {
        ScreenContent.SetActive(true);
        MoneyHUD.SetActive(true);
    }

    public void HideScreen()
    {
        ScreenContent.SetActive(false);
        MoneyHUD.SetActive(false);
    }
}
