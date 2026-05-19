using UnityEngine;
using TMPro;

public class ZamanaKarsiYoneticisi : MonoBehaviour
{
    public static ZamanaKarsiYoneticisi ornek;

    public const string PrefsEnYuksekSkor = "ZamanaKarsi_EnYuksekSkor";

    private const int BaslangicSure = 10;
    private const int MinSure = 5;
    private const int SoruBasinaAzalma = 10;

    private GameObject sayacPaneli;
    private GameObject kaybetmePaneli;
    private TextMeshProUGUI sayacYazi;
    private TextMeshProUGUI skorYazi;
    private TextMeshProUGUI kaybetmeYazi;

    private float kalanSure;
    private int dogruCevapSayisi;
    private bool aktifMi;
    private bool sureBittiMi;

    public static int EnYuksekSkorOku() => PlayerPrefs.GetInt(PrefsEnYuksekSkor, 0);

    private void Awake()
    {
        ornek = this;
    }

    public void Baslat()
    {
        dogruCevapSayisi = 0;
        sureBittiMi = false;
        aktifMi = true;
        KaybetmeGizle();
        SayacOlustur();
        YeniSoruSayaciniBaslat();
        SesYoneticisi.SaatTikAc(true);
    }

    public void Durdur()
    {
        aktifMi = false;
        SesYoneticisi.SaatTikAc(false);

        if (sayacPaneli != null)
            sayacPaneli.SetActive(false);

        KaybetmeGizle();
    }

    public void OturumdanCik(bool kaybetti)
    {
        if (!aktifMi && !sureBittiMi && dogruCevapSayisi == 0)
        {
            Durdur();
            return;
        }

        SkorKaydet();

        if (kaybetti)
        {
            if (!sureBittiMi)
                SureBitti();
            return;
        }

        Durdur();
    }

    public void DogruCevap()
    {
        if (!aktifMi || sureBittiMi) return;

        dogruCevapSayisi++;
        YeniSoruSayaciniBaslat();
    }

    public bool AktifMi => aktifMi;

    private void SkorKaydet()
    {
        int enIyi = EnYuksekSkorOku();
        if (dogruCevapSayisi > enIyi)
        {
            PlayerPrefs.SetInt(PrefsEnYuksekSkor, dogruCevapSayisi);
            PlayerPrefs.Save();
        }
    }

    private int MevcutSureLimit()
    {
        int azalma = dogruCevapSayisi / SoruBasinaAzalma;
        return Mathf.Max(MinSure, BaslangicSure - azalma);
    }

    private void YeniSoruSayaciniBaslat()
    {
        kalanSure = MevcutSureLimit();
        GuncelleYazi();
    }

    private void Update()
    {
        if (!aktifMi || sureBittiMi) return;
        if (OyunYoneticisi.ornek == null || !OyunYoneticisi.ornek.oyunAktifMi) return;
        if (AyarlarYoneticisi.AyarlarAcikMi) return;

        kalanSure -= Time.deltaTime;
        if (kalanSure <= 0f)
        {
            kalanSure = 0f;
            GuncelleYazi();
            OturumdanCik(true);
        }
        else
        {
            GuncelleYazi();
        }
    }

    private void SureBitti()
    {
        sureBittiMi = true;
        aktifMi = false;
        SesYoneticisi.SaatTikAc(false);

        if (sayacPaneli != null)
            sayacPaneli.SetActive(false);

        if (OyunYoneticisi.ornek != null)
            OyunYoneticisi.ornek.OyunuDurdur();

        KaybetmeGoster();
    }

    private void KaybetmeGeriDon()
    {
        Durdur();

        AnaMenuYoneticisi anaMenu = FindFirstObjectByType<AnaMenuYoneticisi>();
        if (anaMenu != null)
            anaMenu.ModSecimMenuyeDon(false);
    }

    private void GuncelleYazi()
    {
        if (sayacYazi != null)
            sayacYazi.text = Mathf.CeilToInt(kalanSure).ToString();

        if (skorYazi != null)
            skorYazi.text = "Solved: " + dogruCevapSayisi;
    }

    private void KaybetmeGoster()
    {
        KaybetmePaneliOlustur();
        if (kaybetmeYazi == null) return;

        int enIyi = EnYuksekSkorOku();
        kaybetmeYazi.text =
            "YOU LOSE\n\n" +
            "Questions solved: " + dogruCevapSayisi + "\n" +
            "Best: " + enIyi;

        kaybetmePaneli.SetActive(true);
        kaybetmePaneli.transform.SetAsLastSibling();
    }

    private void KaybetmeGizle()
    {
        if (kaybetmePaneli != null)
            kaybetmePaneli.SetActive(false);
    }

    private void KaybetmePaneliOlustur()
    {
        if (kaybetmePaneli != null) return;

        GameObject canvasObj = GameObject.Find("BackGround");
        if (canvasObj == null) return;

        kaybetmePaneli = OyunUIYardimci.TamEkranPanel(canvasObj.transform, "ZamanaKarsiKaybetme", 0.72f);

        kaybetmeYazi = OyunUIYardimci.BaslikOlustur(
            kaybetmePaneli.transform,
            "",
            new Vector2(0f, 50f),
            40f,
            new Color(1f, 0.35f, 0.35f));

        RectTransform yaziRt = kaybetmeYazi.rectTransform;
        yaziRt.sizeDelta = new Vector2(520f, 200f);
        kaybetmeYazi.alignment = TextAlignmentOptions.Center;

        Sprite backSprite = OyunUIYardimci.BackButonSpriteBul();
        Vector2 backBoyut = OyunUIYardimci.SpriteButonBoyutu(backSprite, 0.78f);

        OyunUIYardimci.ButonOlustur(
            kaybetmePaneli.transform,
            "",
            new Vector2(0f, -120f),
            backBoyut,
            KaybetmeGeriDon,
            backSprite,
            metinGoster: false,
            tamBoyutKullan: true);

        kaybetmePaneli.SetActive(false);
    }

    private void SayacOlustur()
    {
        GameObject canvasObj = GameObject.Find("BackGround");
        if (canvasObj == null) return;

        if (sayacPaneli == null)
        {
            sayacPaneli = new GameObject("ZamanSayaci", typeof(RectTransform));
            sayacPaneli.transform.SetParent(canvasObj.transform, false);

            RectTransform panelRt = sayacPaneli.GetComponent<RectTransform>();
            panelRt.anchorMin = new Vector2(1f, 1f);
            panelRt.anchorMax = new Vector2(1f, 1f);
            panelRt.pivot = new Vector2(1f, 1f);
            panelRt.anchoredPosition = new Vector2(-36f, -36f);
            panelRt.sizeDelta = new Vector2(140f, 100f);

            GameObject skorObj = new GameObject("Skor", typeof(RectTransform));
            skorObj.transform.SetParent(sayacPaneli.transform, false);
            RectTransform skorRt = skorObj.GetComponent<RectTransform>();
            skorRt.anchorMin = new Vector2(0.5f, 1f);
            skorRt.anchorMax = new Vector2(0.5f, 1f);
            skorRt.pivot = new Vector2(0.5f, 1f);
            skorRt.anchoredPosition = new Vector2(0f, 0f);
            skorRt.sizeDelta = new Vector2(140f, 28f);

            skorYazi = skorObj.AddComponent<TextMeshProUGUI>();
            skorYazi.fontSize = 20f;
            skorYazi.alignment = TextAlignmentOptions.TopRight;
            skorYazi.color = new Color(1f, 0.92f, 0.55f);
            TMP_FontAsset font = OyunUIYardimci.FontBul();
            if (font != null) skorYazi.font = font;

            GameObject sureObj = new GameObject("Sure", typeof(RectTransform));
            sureObj.transform.SetParent(sayacPaneli.transform, false);
            RectTransform sureRt = sureObj.GetComponent<RectTransform>();
            sureRt.anchorMin = new Vector2(0.5f, 0f);
            sureRt.anchorMax = new Vector2(0.5f, 0f);
            sureRt.pivot = new Vector2(0.5f, 0f);
            sureRt.anchoredPosition = new Vector2(0f, 0f);
            sureRt.sizeDelta = new Vector2(140f, 64f);

            sayacYazi = sureObj.AddComponent<TextMeshProUGUI>();
            sayacYazi.fontSize = 52f;
            sayacYazi.fontStyle = FontStyles.Bold;
            sayacYazi.alignment = TextAlignmentOptions.BottomRight;
            sayacYazi.color = Color.white;
            if (font != null) sayacYazi.font = font;
        }

        KaybetmePaneliOlustur();
        sayacPaneli.SetActive(true);
        sayacPaneli.transform.SetAsLastSibling();
    }
}
