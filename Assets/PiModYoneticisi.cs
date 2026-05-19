using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PiModYoneticisi : MonoBehaviour
{
    public static PiModYoneticisi ornek;

    private const string PiTam =
        "3.1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679";

    private const int EzberSayfaKarakter = 44;
    private const int SatirSayfaKarakter = 16;
    private const float NumpadOlcegi = 0.5f;
    private const float NumpadAralik = 10f;
    private const float PiMenuButonAraligi = 36f;
    private const float PiMenuButonOlcegi = 0.72f;

    private GameObject kokPanel;
    private GameObject ezberPanel;
    private GameObject oyunPanel;
    private GameObject sonucPanel;

    private TextMeshProUGUI ezberYazi;
    private TextMeshProUGUI ezberSayfaYazi;
    private TextMeshProUGUI satirYazi;
    private TextMeshProUGUI satirSayfaYazi;
    private TextMeshProUGUI sonucYazi;

    private int sonrakiIndeks;
    private int ezberSayfaIndeksi;
    private int satirSayfaIndeksi;
    private bool oyunAktifMi;

    private void Awake()
    {
        ornek = this;
    }

    private void Start()
    {
        PanelleriOlustur();
        Kapat();
    }

    public void Baslat()
    {
        if (kokPanel == null)
            PanelleriOlustur();

        oyunAktifMi = false;
        sonrakiIndeks = 0;
        ezberSayfaIndeksi = 0;
        satirSayfaIndeksi = 0;
        EzberEkraniniGoster();
    }

    public void Kapat()
    {
        oyunAktifMi = false;
        if (kokPanel != null)
            kokPanel.SetActive(false);
    }

    public bool OyunAcikMi => kokPanel != null && kokPanel.activeSelf;

    private int EzberSayfaSayisi()
    {
        return Mathf.Max(1, Mathf.CeilToInt((float)PiTam.Length / EzberSayfaKarakter));
    }

    private int SatirMaksSayfaIndeksi()
    {
        if (sonrakiIndeks <= 0) return 0;
        return Mathf.Min(
            (PiTam.Length - 1) / SatirSayfaKarakter,
            Mathf.Max(0, (sonrakiIndeks - 1) / SatirSayfaKarakter));
    }

    private void EzberEkraniniGoster()
    {
        kokPanel.SetActive(true);
        kokPanel.transform.SetAsLastSibling();
        ezberPanel.SetActive(true);
        oyunPanel.SetActive(false);
        sonucPanel.SetActive(false);
        EzberSayfasiniGuncelle();
    }

    private void EzberSayfasiniGuncelle()
    {
        if (ezberYazi != null)
            ezberYazi.text = PiEzberSayfaMetni(ezberSayfaIndeksi);

        if (ezberSayfaYazi != null)
            ezberSayfaYazi.text = (ezberSayfaIndeksi + 1) + " / " + EzberSayfaSayisi();
    }

    private void EzberSayfaIleri()
    {
        if (ezberSayfaIndeksi >= EzberSayfaSayisi() - 1) return;
        ezberSayfaIndeksi++;
        EzberSayfasiniGuncelle();
    }

    private void EzberSayfaGeri()
    {
        if (ezberSayfaIndeksi <= 0) return;
        ezberSayfaIndeksi--;
        EzberSayfasiniGuncelle();
    }

    private void OyunEkraniniGoster()
    {
        oyunAktifMi = true;
        sonrakiIndeks = 0;
        satirSayfaIndeksi = 0;
        SonrakiRakamBasamaginaGec();
        ezberPanel.SetActive(false);
        oyunPanel.SetActive(true);
        sonucPanel.SetActive(false);
        SatirSayfasiniGuncelle();
    }

    private void SatirAktifSayfayaGit()
    {
        satirSayfaIndeksi = Mathf.Clamp(sonrakiIndeks / SatirSayfaKarakter, 0, SatirMaksSayfaIndeksi());
    }

    private void SatirSayfaIleri()
    {
        if (satirSayfaIndeksi >= SatirMaksSayfaIndeksi()) return;
        satirSayfaIndeksi++;
        SatirSayfasiniGuncelle();
    }

    private void SatirSayfaGeri()
    {
        if (satirSayfaIndeksi <= 0) return;
        satirSayfaIndeksi--;
        SatirSayfasiniGuncelle();
    }

    private void SonrakiRakamBasamaginaGec()
    {
        while (sonrakiIndeks < PiTam.Length && !char.IsDigit(PiTam[sonrakiIndeks]))
            sonrakiIndeks++;
    }

    private void SonucEkraniniGoster(int indeks)
    {
        oyunAktifMi = false;
        ezberPanel.SetActive(false);
        oyunPanel.SetActive(false);
        sonucPanel.SetActive(true);

        int dogruRakam = DogruRakamSayisi(indeks);
        int enIyi = EnIyiSkorKaydet(dogruRakam);
        bool tamamlandi = indeks >= PiTam.Length;

        if (sonucYazi != null)
        {
            sonucYazi.text =
                (tamamlandi ? "Amazing!\n\n" : "Wrong digit!\n\n") +
                "Digits correct: " + dogruRakam + "\n" +
                "Best: " + enIyi;
        }
    }

    private static int DogruRakamSayisi(int indeks)
    {
        int say = 0;
        int son = Mathf.Min(indeks, PiTam.Length);
        for (int i = 0; i < son; i++)
        {
            if (char.IsDigit(PiTam[i]))
                say++;
        }
        return say;
    }

    private static string PiEzberSayfaMetni(int sayfa)
    {
        int baslangic = sayfa * EzberSayfaKarakter;
        if (baslangic >= PiTam.Length) return "";

        int bitis = Mathf.Min(baslangic + EzberSayfaKarakter, PiTam.Length);
        StringBuilder sb = new StringBuilder();
        for (int i = baslangic; i < bitis; i++)
        {
            sb.Append(PiTam[i]);
            int sayfaIci = i - baslangic;
            if (sayfaIci > 0 && sayfaIci % 11 == 0)
                sb.Append('\n');
            else if (i < bitis - 1)
                sb.Append(' ');
        }
        return sb.ToString();
    }

    private void RakamBasildi(int rakam)
    {
        if (!oyunAktifMi) return;

        SonrakiRakamBasamaginaGec();
        if (sonrakiIndeks >= PiTam.Length)
        {
            SonucEkraniniGoster(sonrakiIndeks);
            return;
        }

        char beklenen = PiTam[sonrakiIndeks];
        if (rakam.ToString()[0] != beklenen)
        {
            SonucEkraniniGoster(sonrakiIndeks);
            return;
        }

        sonrakiIndeks++;
        SonrakiRakamBasamaginaGec();
        SatirAktifSayfayaGit();
        SatirSayfasiniGuncelle();

        if (sonrakiIndeks >= PiTam.Length)
            SonucEkraniniGoster(sonrakiIndeks);
    }

    private void SatirSayfasiniGuncelle()
    {
        if (satirYazi == null) return;

        int baslangic = satirSayfaIndeksi * SatirSayfaKarakter;
        int bitis = Mathf.Min(baslangic + SatirSayfaKarakter, PiTam.Length);
        StringBuilder sb = new StringBuilder();

        for (int i = baslangic; i < bitis; i++)
        {
            if (i > baslangic)
                sb.Append(' ');

            if (i < sonrakiIndeks)
                sb.Append(PiTam[i]);
            else if (i == sonrakiIndeks)
                sb.Append('_');
            else
                sb.Append('_');
        }

        satirYazi.text = sb.ToString();
        satirYazi.fontSize = 32f;

        if (satirSayfaYazi != null)
        {
            int toplamSayfa = SatirMaksSayfaIndeksi() + 1;
            satirSayfaYazi.text = (satirSayfaIndeksi + 1) + " / " + toplamSayfa;
        }
    }

    private void GeriDon()
    {
        Kapat();
        AnaMenuYoneticisi anaMenu = FindFirstObjectByType<AnaMenuYoneticisi>();
        if (anaMenu != null)
            anaMenu.ModSecimMenuyeDon(false);
    }

    private static int EnIyiSkorKaydet(int skor)
    {
        const string anahtar = "PiMod_EnYuksekSkor";
        int enIyi = PlayerPrefs.GetInt(anahtar, 0);
        if (skor > enIyi)
        {
            PlayerPrefs.SetInt(anahtar, skor);
            PlayerPrefs.Save();
            return skor;
        }
        return enIyi;
    }

    public static int EnIyiSkorOku() => PlayerPrefs.GetInt("PiMod_EnYuksekSkor", 0);

    private void PanelleriOlustur()
    {
        GameObject canvasObj = GameObject.Find("BackGround");
        if (canvasObj == null) return;

        kokPanel = OyunUIYardimci.TamEkranPanel(canvasObj.transform, "PiModPanel", 0.5f);
        EzberPaneliOlustur();
        OyunPaneliOlustur();
        SonucPaneliOlustur();
        kokPanel.SetActive(false);
    }

    private void EzberPaneliOlustur()
    {
        ezberPanel = new GameObject("PiEzber", typeof(RectTransform));
        ezberPanel.transform.SetParent(kokPanel.transform, false);
        RectTransform ezberRt = ezberPanel.GetComponent<RectTransform>();
        ezberRt.anchorMin = Vector2.zero;
        ezberRt.anchorMax = Vector2.one;
        ezberRt.offsetMin = Vector2.zero;
        ezberRt.offsetMax = Vector2.zero;

        OyunUIYardimci.BaslikOlustur(
            ezberPanel.transform,
            "PI MODE — MEMORIZE",
            new Vector2(0f, 220f),
            40f,
            new Color(1f, 0.95f, 0.4f));

        GameObject ezberKutu = new GameObject("EzberKutu", typeof(RectTransform));
        ezberKutu.transform.SetParent(ezberPanel.transform, false);
        RectTransform kutuRt = ezberKutu.GetComponent<RectTransform>();
        kutuRt.anchorMin = new Vector2(0.5f, 0.5f);
        kutuRt.anchorMax = new Vector2(0.5f, 0.5f);
        kutuRt.anchoredPosition = new Vector2(0f, 45f);
        kutuRt.sizeDelta = new Vector2(560f, 200f);
        ezberKutu.AddComponent<RectMask2D>();

        Image kutuImg = ezberKutu.AddComponent<Image>();
        kutuImg.color = new Color(0.1f, 0.14f, 0.1f, 0.85f);

        GameObject yaziObj = new GameObject("EzberYazi", typeof(RectTransform));
        yaziObj.transform.SetParent(ezberKutu.transform, false);
        RectTransform yaziRt = yaziObj.GetComponent<RectTransform>();
        yaziRt.anchorMin = Vector2.zero;
        yaziRt.anchorMax = Vector2.one;
        yaziRt.offsetMin = new Vector2(14f, 14f);
        yaziRt.offsetMax = new Vector2(-14f, -14f);

        ezberYazi = yaziObj.AddComponent<TextMeshProUGUI>();
        ezberYazi.fontSize = 30f;
        ezberYazi.alignment = TextAlignmentOptions.Center;
        ezberYazi.textWrappingMode = TextWrappingModes.Normal;
        ezberYazi.color = Color.white;
        TMP_FontAsset font = OyunUIYardimci.FontBul();
        if (font != null) ezberYazi.font = font;

        OkButonuEkle(ezberPanel.transform, new Vector2(-340f, 45f), "<", EzberSayfaGeri);
        OkButonuEkle(ezberPanel.transform, new Vector2(340f, 45f), ">", EzberSayfaIleri);

        ezberSayfaYazi = OyunUIYardimci.EtiketOlustur(ezberPanel.transform, "1 / 1", new Vector2(0f, -55f), 22f);
        ezberSayfaYazi.alignment = TextAlignmentOptions.Center;
        ezberSayfaYazi.color = new Color(1f, 0.92f, 0.55f);

        Sprite startSprite = OyunUIYardimci.PlayButonSpriteBul();
        Sprite backSprite = OyunUIYardimci.BackButonSpriteBul();
        Vector2 startBoyut = OyunUIYardimci.SpriteButonBoyutu(startSprite, PiMenuButonOlcegi);
        Vector2 backBoyut = OyunUIYardimci.SpriteButonBoyutu(backSprite, PiMenuButonOlcegi);
        float playY = -115f;
        float backY = playY - startBoyut.y * 0.5f - PiMenuButonAraligi - backBoyut.y * 0.5f;

        OyunUIYardimci.ButonOlustur(
            ezberPanel.transform,
            "",
            new Vector2(0f, playY),
            startBoyut,
            OyunEkraniniGoster,
            startSprite,
            metinGoster: false,
            tamBoyutKullan: true);

        OyunUIYardimci.ButonOlustur(
            ezberPanel.transform,
            "",
            new Vector2(0f, backY),
            backBoyut,
            GeriDon,
            backSprite,
            metinGoster: false,
            tamBoyutKullan: true);
    }

    private void OyunPaneliOlustur()
    {
        oyunPanel = new GameObject("PiOyun", typeof(RectTransform));
        oyunPanel.transform.SetParent(kokPanel.transform, false);
        RectTransform oyunRt = oyunPanel.GetComponent<RectTransform>();
        oyunRt.anchorMin = Vector2.zero;
        oyunRt.anchorMax = Vector2.one;
        oyunRt.offsetMin = Vector2.zero;
        oyunRt.offsetMax = Vector2.zero;

        OyunUIYardimci.BaslikOlustur(
            oyunPanel.transform,
            "TYPE PI",
            new Vector2(0f, 230f),
            38f,
            new Color(1f, 0.95f, 0.4f));

        GameObject satirKutu = new GameObject("SatirKutu", typeof(RectTransform));
        satirKutu.transform.SetParent(oyunPanel.transform, false);
        RectTransform satirRt = satirKutu.GetComponent<RectTransform>();
        satirRt.anchorMin = new Vector2(0.5f, 0.5f);
        satirRt.anchorMax = new Vector2(0.5f, 0.5f);
        satirRt.anchoredPosition = new Vector2(0f, 158f);
        satirRt.sizeDelta = new Vector2(560f, 72f);
        satirKutu.AddComponent<RectMask2D>();

        Image satirArka = satirKutu.AddComponent<Image>();
        satirArka.color = new Color(0.08f, 0.12f, 0.08f, 0.9f);

        GameObject satirObj = new GameObject("SatirYazi", typeof(RectTransform));
        satirObj.transform.SetParent(satirKutu.transform, false);
        RectTransform satirYaziRt = satirObj.GetComponent<RectTransform>();
        satirYaziRt.anchorMin = Vector2.zero;
        satirYaziRt.anchorMax = Vector2.one;
        satirYaziRt.offsetMin = new Vector2(10f, 6f);
        satirYaziRt.offsetMax = new Vector2(-10f, -6f);

        satirYazi = satirObj.AddComponent<TextMeshProUGUI>();
        satirYazi.fontSize = 32f;
        satirYazi.alignment = TextAlignmentOptions.Center;
        satirYazi.textWrappingMode = TextWrappingModes.NoWrap;
        satirYazi.overflowMode = TextOverflowModes.Overflow;
        satirYazi.color = new Color(0.75f, 1f, 0.75f);
        TMP_FontAsset font = OyunUIYardimci.FontBul();
        if (font != null) satirYazi.font = font;

        OkButonuEkle(oyunPanel.transform, new Vector2(-340f, 158f), "<", SatirSayfaGeri);
        OkButonuEkle(oyunPanel.transform, new Vector2(340f, 158f), ">", SatirSayfaIleri);

        satirSayfaYazi = OyunUIYardimci.EtiketOlustur(oyunPanel.transform, "1 / 1", new Vector2(0f, 118f), 20f);
        satirSayfaYazi.alignment = TextAlignmentOptions.Center;
        satirSayfaYazi.color = new Color(1f, 0.92f, 0.55f);

        NumpadOlustur(oyunPanel.transform);

        Sprite oyunBackSprite = OyunUIYardimci.BackButonSpriteBul();
        Vector2 oyunBackBoyut = OyunUIYardimci.SpriteButonBoyutu(oyunBackSprite, PiMenuButonOlcegi);
        float oyunBackY = -268f;
        OyunUIYardimci.ButonOlustur(
            oyunPanel.transform,
            "",
            new Vector2(0f, oyunBackY),
            oyunBackBoyut,
            GeriDon,
            oyunBackSprite,
            metinGoster: false,
            tamBoyutKullan: true);

        oyunPanel.SetActive(false);
    }

    private void NumpadOlustur(Transform parent)
    {
        GameObject numpad = new GameObject("Numpad", typeof(RectTransform));
        numpad.transform.SetParent(parent, false);
        RectTransform numpadRt = numpad.GetComponent<RectTransform>();
        numpadRt.anchorMin = new Vector2(0.5f, 0.5f);
        numpadRt.anchorMax = new Vector2(0.5f, 0.5f);
        numpadRt.anchoredPosition = new Vector2(0f, -35f);

        Sprite ornekSprite = OyunUIYardimci.RakamSpriteBul(5);
        Vector2 tusBoyutu = OyunUIYardimci.SpriteButonBoyutu(ornekSprite, NumpadOlcegi);
        float adimX = tusBoyutu.x + NumpadAralik;
        float adimY = tusBoyutu.y + NumpadAralik;

        int[,] duzen = {
            { 7, 8, 9 },
            { 4, 5, 6 },
            { 1, 2, 3 },
            { -1, 0, -1 }
        };

        float baslangicX = -adimX;
        float baslangicY = adimY * 1.5f;

        for (int satir = 0; satir < 4; satir++)
        {
            for (int sutun = 0; sutun < 3; sutun++)
            {
                int rakam = duzen[satir, sutun];
                if (rakam < 0) continue;

                int yakalananRakam = rakam;
                OyunUIYardimci.ButonOlustur(
                    numpad.transform,
                    "",
                    new Vector2(baslangicX + sutun * adimX, baslangicY - satir * adimY),
                    tusBoyutu,
                    () => RakamBasildi(yakalananRakam),
                    OyunUIYardimci.RakamSpriteBul(rakam),
                    metinGoster: false,
                    tamBoyutKullan: true);
            }
        }
    }

    private void SonucPaneliOlustur()
    {
        sonucPanel = new GameObject("PiSonuc", typeof(RectTransform));
        sonucPanel.transform.SetParent(kokPanel.transform, false);
        RectTransform sonucRt = sonucPanel.GetComponent<RectTransform>();
        sonucRt.anchorMin = Vector2.zero;
        sonucRt.anchorMax = Vector2.one;
        sonucRt.offsetMin = Vector2.zero;
        sonucRt.offsetMax = Vector2.zero;

        sonucYazi = OyunUIYardimci.BaslikOlustur(
            sonucPanel.transform,
            "",
            new Vector2(0f, 40f),
            36f,
            new Color(1f, 0.85f, 0.5f));

        RectTransform sonucYaziRt = sonucYazi.rectTransform;
        sonucYaziRt.sizeDelta = new Vector2(520f, 200f);
        sonucYazi.alignment = TextAlignmentOptions.Center;

        GeriButonuEkle(sonucPanel.transform, new Vector2(0f, -120f));
        sonucPanel.SetActive(false);
    }

    private void OkButonuEkle(Transform parent, Vector2 pozisyon, string sembol, UnityEngine.Events.UnityAction tikla)
    {
        OyunUIYardimci.ButonOlustur(
            parent,
            sembol,
            pozisyon,
            new Vector2(64f, 64f),
            tikla,
            OyunUIYardimci.MenuButonSpriteBul(),
            metinGoster: true,
            tamBoyutKullan: true);
    }

    private void GeriButonuEkle(Transform parent, Vector2 pozisyon)
    {
        Sprite backSprite = OyunUIYardimci.BackButonSpriteBul();
        Vector2 backBoyut = OyunUIYardimci.SpriteButonBoyutu(backSprite, PiMenuButonOlcegi);
        OyunUIYardimci.ButonOlustur(
            parent,
            "",
            pozisyon,
            backBoyut,
            GeriDon,
            backSprite,
            metinGoster: false,
            tamBoyutKullan: true);
    }
}
