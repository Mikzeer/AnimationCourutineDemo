using DG.Tweening;
using PositionerDemo;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    #region VARIABLES
    [Header("Screen Reference")]
    [SerializeField] private GameObject ScreenContent;
    [Header("Pack Prefab")]
    [SerializeField] private GameObject PackPrefab;
    [Header("Rect Placement")]
    [SerializeField] private Transform PacksParent;
    [SerializeField] private Transform InitialPackSpot;
    [SerializeField] private float RotationRange = 10f;
    [SerializeField] private PackOpeningArea OpeningArea;
    private float packPlacementOffset = -0.01f;
    private float PosXRange = 4f;
    private float PosYRange = 8f;
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
    [Header("Manager")]
    [SerializeField] private GameMenuManager gameMenuManager;

    [SerializeField] private Button btnBuy;
    private bool debugOn = false;
    #endregion

    private void Start()
    {
        HideScreen();
    }

    private void OnEnable()
    {
        btnBuy.onClick.AddListener(BuyPackDB);
    }

    private void OnDisable()
    {
        btnBuy.onClick.RemoveAllListeners();
    }

    public void LoadUserResourcesFromFirebase(UserResources userResources)
    {
        if (userResources != null)
        {
            Money = userResources.Gold;
            StartCoroutine(GivePacks(userResources.UnopendPacks, true));
            if (debugOn) Debug.Log("USER RESOURCES LOADED FROM DB ONLINE");
        }               
    }

    public void LoadPriceDataFromFirebase(GamePricesData priceData)
    {
        if (priceData != null)
        {
            PackPrice = priceData.NormalPackPrices;
            if(debugOn) Debug.Log("PRICE DATA LOADED FROM DB ONLINE");
        }
    }

    public IEnumerator GivePacks(int NumberOfPacks, bool instant = false)
    {
        for (int i = 0; i < NumberOfPacks; i++)
        {
            GameObject newPack = Instantiate(PackPrefab, PacksParent);

            Vector3 CardSize = newPack.GetComponent<Image>().sprite.rect.max;

            float halfCardWidht = CardSize.x / 2;
            float halfCardHeight = CardSize.y / 2;

            PosXRange -= halfCardWidht;
            PosYRange -= halfCardHeight;

            Vector3 localPositionForNewPack = new Vector3(Random.Range(-PosXRange, PosXRange), Random.Range(-PosYRange, PosYRange), 0);
            newPack.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(-RotationRange, RotationRange));
            PacksCreated++;

            // make this pack appear on top of all the previous packs using PacksCreated;
            newPack.GetComponent<CardPackUINEW>().SetCardPackOpeningArea(OpeningArea);
            if (instant)
                newPack.transform.localPosition = PacksParent.position;
            else
            {
                newPack.transform.DOLocalMove(localPositionForNewPack, 0.5f);
                yield return new WaitForSeconds(0.5f);
            }
        }
        yield break;
    }

    public async void BuyPackDB()
    {
        RectTransform rect = PacksParent.GetComponent<RectTransform>();
        PosXRange = rect.rect.size.x / 2;
        PosYRange = rect.rect.size.y / 2;

        UserResourcesManager userResourcesManager = new UserResourcesManager();
        bool hasEnoughMoney = await userResourcesManager.CanUserBuyAPackANormalPack(gameMenuManager.GetUser());

        if (hasEnoughMoney == true)
        {
            userResourcesManager.BuyPackDB(gameMenuManager.GetUser());
            Money -= PackPrice;
            StartCoroutine(GivePacks(1));
        }
        else
        {
            Debug.Log("NOT ENOUGH MONEY ");
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