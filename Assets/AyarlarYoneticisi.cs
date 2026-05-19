using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AyarlarYoneticisi : MonoBehaviour
{
    public static AyarlarYoneticisi ornek;

    private const string PrefsSes = "Ayarlar_Ses";
    private const string PrefsTamEkran = "Ayarlar_TamEkran";

    private GameObject ayarlarPaneli;
    private Slider sesSlider;
    private Toggle tamEkranToggle;
    private bool acikMi;

    public static bool AyarlarAcikMi => ornek != null && ornek.acikMi;

    private void Awake()
    {
        ornek = this;
        AyarlariUygula();
    }

    private void Start()
    {
        PanelOlustur();
        Kapat();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (acikMi) Kapat();
            else Ac();
        }
    }

    public void Ac()
    {
        if (ayarlarPaneli == null) return;

        acikMi = true;
        ayarlarPaneli.SetActive(true);
        ayarlarPaneli.transform.SetAsLastSibling();

        if (sesSlider != null)
            sesSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(PrefsSes, 1f));
        if (tamEkranToggle != null)
            tamEkranToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt(PrefsTamEkran, Screen.fullScreen ? 1 : 0) == 1);

        SesYoneticisi.SaatTikYenile();
    }

    public void Kapat()
    {
        acikMi = false;
        if (ayarlarPaneli != null)
            ayarlarPaneli.SetActive(false);

        SesYoneticisi.SaatTikYenile();
    }

    private void PanelOlustur()
    {
        GameObject canvasObj = GameObject.Find("BackGround");
        if (canvasObj == null) return;

        ayarlarPaneli = OyunUIYardimci.TamEkranPanel(canvasObj.transform, "AyarlarPaneli", 0.65f);

        OyunUIYardimci.BaslikOlustur(
            ayarlarPaneli.transform,
            "SETTINGS",
            new Vector2(0f, 160f),
            44f,
            new Color(1f, 0.95f, 0.4f));

        GameObject kutu = new GameObject("AyarlarKutusu", typeof(RectTransform));
        kutu.transform.SetParent(ayarlarPaneli.transform, false);
        RectTransform kutuRt = kutu.GetComponent<RectTransform>();
        kutuRt.anchorMin = new Vector2(0.5f, 0.5f);
        kutuRt.anchorMax = new Vector2(0.5f, 0.5f);
        kutuRt.anchoredPosition = new Vector2(0f, 10f);
        kutuRt.sizeDelta = new Vector2(420f, 200f);

        Image kutuImg = kutu.AddComponent<Image>();
        kutuImg.color = new Color(0.12f, 0.14f, 0.22f, 0.92f);

        OyunUIYardimci.EtiketOlustur(kutu.transform, "Volume", new Vector2(-120f, 50f));
        sesSlider = SliderOlustur(kutu.transform, new Vector2(40f, 50f), SesDegisti);

        OyunUIYardimci.EtiketOlustur(kutu.transform, "Fullscreen", new Vector2(-120f, -20f));
        tamEkranToggle = ToggleOlustur(kutu.transform, new Vector2(40f, -20f), TamEkranDegisti);

        Sprite backSprite = OyunUIYardimci.BackButonSpriteBul();
        Vector2 backBoyut = OyunUIYardimci.SpriteButonBoyutu(backSprite, 0.85f);

        OyunUIYardimci.ButonOlustur(
            ayarlarPaneli.transform,
            "",
            new Vector2(0f, -170f),
            backBoyut,
            Kapat,
            backSprite,
            metinGoster: false,
            tamBoyutKullan: true);
    }

    private Slider SliderOlustur(Transform parent, Vector2 pozisyon, UnityEngine.Events.UnityAction<float> degisim)
    {
        GameObject sliderObj = new GameObject("SesSlider", typeof(RectTransform));
        sliderObj.transform.SetParent(parent, false);

        RectTransform rt = sliderObj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pozisyon;
        rt.sizeDelta = new Vector2(220f, 30f);

        GameObject arkaObj = new GameObject("ArkaPlan", typeof(RectTransform));
        arkaObj.transform.SetParent(sliderObj.transform, false);
        RectTransform arkaRt = arkaObj.GetComponent<RectTransform>();
        arkaRt.anchorMin = Vector2.zero;
        arkaRt.anchorMax = Vector2.one;
        arkaRt.offsetMin = Vector2.zero;
        arkaRt.offsetMax = Vector2.zero;
        Image arkaImg = arkaObj.AddComponent<Image>();
        arkaImg.color = new Color(0.3f, 0.3f, 0.35f);

        GameObject dolumObj = new GameObject("Dolum", typeof(RectTransform));
        dolumObj.transform.SetParent(sliderObj.transform, false);
        RectTransform dolumRt = dolumObj.GetComponent<RectTransform>();
        dolumRt.anchorMin = new Vector2(0f, 0.25f);
        dolumRt.anchorMax = new Vector2(0f, 0.75f);
        dolumRt.pivot = new Vector2(0f, 0.5f);
        dolumRt.sizeDelta = new Vector2(10f, 0f);
        Image dolumImg = dolumObj.AddComponent<Image>();
        dolumImg.color = new Color(0.35f, 0.75f, 1f);

        GameObject tutamacObj = new GameObject("Tutamac", typeof(RectTransform));
        tutamacObj.transform.SetParent(sliderObj.transform, false);
        RectTransform tutamacRt = tutamacObj.GetComponent<RectTransform>();
        tutamacRt.sizeDelta = new Vector2(24f, 24f);
        Image tutamacImg = tutamacObj.AddComponent<Image>();
        tutamacImg.color = Color.white;

        Slider slider = sliderObj.AddComponent<Slider>();
        slider.fillRect = dolumRt;
        slider.handleRect = tutamacRt;
        slider.targetGraphic = tutamacImg;
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = PlayerPrefs.GetFloat(PrefsSes, 1f);
        slider.onValueChanged.AddListener(degisim);

        return slider;
    }

    private Toggle ToggleOlustur(Transform parent, Vector2 pozisyon, UnityEngine.Events.UnityAction<bool> degisim)
    {
        GameObject toggleObj = new GameObject("TamEkranToggle", typeof(RectTransform));
        toggleObj.transform.SetParent(parent, false);

        RectTransform rt = toggleObj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pozisyon;
        rt.sizeDelta = new Vector2(40f, 40f);

        GameObject arkaObj = new GameObject("ArkaPlan", typeof(RectTransform));
        arkaObj.transform.SetParent(toggleObj.transform, false);
        RectTransform arkaRt = arkaObj.GetComponent<RectTransform>();
        arkaRt.anchorMin = Vector2.zero;
        arkaRt.anchorMax = Vector2.one;
        arkaRt.offsetMin = Vector2.zero;
        arkaRt.offsetMax = Vector2.zero;
        Image arkaImg = arkaObj.AddComponent<Image>();
        arkaImg.color = new Color(0.3f, 0.3f, 0.35f);

        GameObject isaretObj = new GameObject("Isaret", typeof(RectTransform));
        isaretObj.transform.SetParent(toggleObj.transform, false);
        RectTransform isaretRt = isaretObj.GetComponent<RectTransform>();
        isaretRt.anchorMin = new Vector2(0.15f, 0.15f);
        isaretRt.anchorMax = new Vector2(0.85f, 0.85f);
        isaretRt.offsetMin = Vector2.zero;
        isaretRt.offsetMax = Vector2.zero;
        Image isaretImg = isaretObj.AddComponent<Image>();
        isaretImg.color = new Color(0.35f, 0.85f, 0.45f);

        Toggle toggle = toggleObj.AddComponent<Toggle>();
        toggle.targetGraphic = arkaImg;
        toggle.graphic = isaretImg;
        toggle.isOn = PlayerPrefs.GetInt(PrefsTamEkran, Screen.fullScreen ? 1 : 0) == 1;
        toggle.onValueChanged.AddListener(degisim);

        return toggle;
    }

    private void SesDegisti(float deger)
    {
        SesYoneticisi.SesSeviyesiniUygula(deger);
        PlayerPrefs.SetFloat(PrefsSes, deger);
        PlayerPrefs.Save();
    }

    private void TamEkranDegisti(bool acik)
    {
        Screen.fullScreen = acik;
        PlayerPrefs.SetInt(PrefsTamEkran, acik ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void AyarlariUygula()
    {
        SesYoneticisi.SesSeviyesiniUygula(PlayerPrefs.GetFloat(PrefsSes, 1f));
        bool tamEkran = PlayerPrefs.GetInt(PrefsTamEkran, Screen.fullScreen ? 1 : 0) == 1;
        Screen.fullScreen = tamEkran;
    }
}
