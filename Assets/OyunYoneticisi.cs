using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class OyunYoneticisi : MonoBehaviour
{
    public static OyunYoneticisi ornek;

    [Header("UI")]
    public TextMeshProUGUI canliYaziUI;
    public TextMeshProUGUI hedefYaziUI;

    [Header("Sprites")]
    public Sprite[] rakamSpriteleri;
    public Sprite artiSprite;
    public Sprite eksiSprite;
    public Sprite carpiSprite;
    public Sprite boluSprite;

    [Header("Menu Button Sprites")]
    public Sprite playMenuSprite;
    public Sprite quitMenuSprite;
    public Sprite backMenuSprite;
    public Sprite settingsMenuSprite;
    public Sprite classicModeSprite;
    public Sprite customModeSprite;
    public Sprite againstTimeSprite;
    public Sprite easySprite;
    public Sprite mediumSprite;
    public Sprite hardSprite;
    public Sprite gameLogoSprite;

    [Header("Game State")]
    public string suAnkiIslem = "";
    public int anlikHedefSayi;
    public OyunModu aktifMod = OyunModu.Klasik;
    public ZorlukSeviyesi aktifZorluk = ZorlukSeviyesi.Orta;

    public List<SayiButonu> secilenButonlar = new List<SayiButonu>();

    public bool oyunAktifMi = false;

    private List<SayiButonu> solSutun = new List<SayiButonu>();
    private List<SayiButonu> ortaSutun = new List<SayiButonu>();
    private List<SayiButonu> sagSutun = new List<SayiButonu>();

    private void Awake()
    {
        ornek = this;
        ButonlariListele();

        MenuSpriteYukle();

        if (boluSprite == null)
            boluSprite = SpriteBul("mathgamenumbas 1_12");
    }

    private void MenuSpriteYukle()
    {
        if (playMenuSprite == null) playMenuSprite = SpriteBul("mathgamebuttons 2_5");
        if (settingsMenuSprite == null) settingsMenuSprite = SpriteBul("mathgamebuttons 2_6");
        if (againstTimeSprite == null) againstTimeSprite = SpriteBul("mathgamebuttons 2_7");
        if (hardSprite == null) hardSprite = SpriteBul("mathgamebuttons 2_8");
        if (quitMenuSprite == null) quitMenuSprite = SpriteBul("mathgamebuttons 2_14");
        if (classicModeSprite == null) classicModeSprite = SpriteBul("mathgamebuttons 2_15");
        if (easySprite == null) easySprite = SpriteBul("mathgamebuttons 2_16");
        if (backMenuSprite == null) backMenuSprite = SpriteBul("mathgamebuttons 2_23");
        if (customModeSprite == null) customModeSprite = SpriteBul("mathgamebuttons 2_24");
        if (mediumSprite == null) mediumSprite = SpriteBul("mathgamebuttons 2_25");
        if (gameLogoSprite == null) gameLogoSprite = SpriteBul("Image_0");
    }

    private void Update()
    {
        if (!oyunAktifMi || AyarlarYoneticisi.AyarlarAcikMi) return;

        if (Input.GetMouseButtonUp(0))
        {
            if (secilenButonlar.Count == 3)
                CevabiKontrolEt();

            suAnkiIslem = "";
            canliYaziUI.text = "";
            foreach (SayiButonu buton in secilenButonlar) buton.secildiMi = false;
            secilenButonlar.Clear();
        }
    }

    private void ButonlariListele()
    {
        solSutun.Clear();
        ortaSutun.Clear();
        sagSutun.Clear();

        SayiButonu[] tumButonlar = Object.FindObjectsByType<SayiButonu>(FindObjectsSortMode.None);
        foreach (SayiButonu buton in tumButonlar)
        {
            if (buton.sutunNo == 0) solSutun.Add(buton);
            else if (buton.sutunNo == 1) ortaSutun.Add(buton);
            else if (buton.sutunNo == 2) sagSutun.Add(buton);
        }
    }

    private void OyunIzgarasiniDuzenle()
    {
        float genislik = 248f * OyunUIYardimci.OyunGridButonOlcegi;
        float yukseklik = 127f * OyunUIYardimci.OyunGridButonOlcegi;
        float kolonMesafe = genislik + 52f;
        float satirMesafe = yukseklik + 24f;

        float[] kolonX = { -kolonMesafe, 0f, kolonMesafe };
        float[] satirY = { satirMesafe, 0f, -satirMesafe };

        SutunIzgarasiniDuzenle(solSutun, kolonX[0], satirY);
        SutunIzgarasiniDuzenle(ortaSutun, kolonX[1], satirY);
        SutunIzgarasiniDuzenle(sagSutun, kolonX[2], satirY);
    }

    private void OyunYazilariniDuzenle()
    {
        float yukseklik = 127f * OyunUIYardimci.OyunGridButonOlcegi;
        float satirMesafe = yukseklik + 24f;
        float gridUstKenar = satirMesafe + yukseklik * 0.5f + 48f;
        const float yaziAraligi = 58f;
        const float hedefEkstraYukari = 36f;

        if (canliYaziUI != null)
        {
            RectTransform canliRt = canliYaziUI.rectTransform;
            canliRt.anchoredPosition = new Vector2(canliRt.anchoredPosition.x, gridUstKenar);
        }

        if (hedefYaziUI != null)
        {
            RectTransform hedefRt = hedefYaziUI.rectTransform;
            hedefRt.anchoredPosition = new Vector2(
                hedefRt.anchoredPosition.x,
                gridUstKenar + yaziAraligi + hedefEkstraYukari);
        }
    }

    private static void SutunIzgarasiniDuzenle(List<SayiButonu> sutun, float x, float[] satirY)
    {
        if (sutun == null || sutun.Count == 0) return;

        sutun.Sort((a, b) =>
        {
            float ay = a.GetComponent<RectTransform>().anchoredPosition.y;
            float by = b.GetComponent<RectTransform>().anchoredPosition.y;
            return by.CompareTo(ay);
        });

        for (int i = 0; i < sutun.Count; i++)
        {
            int satir = Mathf.Min(i, satirY.Length - 1);
            RectTransform rt = sutun[i].GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(x, satirY[satir]);
        }
    }

    public void OyunuBaslatKlasik()
    {
        aktifMod = OyunModu.Klasik;
        aktifZorluk = ZorlukSeviyesi.Orta;
        OyunuBaslat();
    }

    public void OyunuBaslatOzel(ZorlukSeviyesi zorluk)
    {
        aktifMod = OyunModu.Ozel;
        aktifZorluk = zorluk;
        OyunuBaslat();
    }

    public void OyunuBaslatZamanaKarsi()
    {
        aktifMod = OyunModu.ZamanaKarsi;
        aktifZorluk = ZorlukSeviyesi.Orta;
        OyunuBaslat();
    }

    public void OyunuBaslat()
    {
        if (aktifMod != OyunModu.ZamanaKarsi && ZamanaKarsiYoneticisi.ornek != null)
            ZamanaKarsiYoneticisi.ornek.Durdur();

        oyunAktifMi = true;
        suAnkiIslem = "";
        canliYaziUI.text = "";
        secilenButonlar.Clear();
        YeniBolumOlustur();
        OyunIzgarasiniDuzenle();
        OyunYazilariniDuzenle();

        if (aktifMod == OyunModu.ZamanaKarsi && ZamanaKarsiYoneticisi.ornek != null)
            ZamanaKarsiYoneticisi.ornek.Baslat();
    }

    public void OyunuDurdur()
    {
        oyunAktifMi = false;
        suAnkiIslem = "";
        if (canliYaziUI != null) canliYaziUI.text = "";
        if (hedefYaziUI != null) hedefYaziUI.text = "";
        secilenButonlar.Clear();
    }

    public void YeniBolumOlustur()
    {
        if (!oyunAktifMi) return;

        ZorlukAyarlariAl(out int minSayi, out int maxSayi, out List<string> islemHavuzu);

        for (int deneme = 0; deneme < 40; deneme++)
        {
            BenzersizSayilariAta(solSutun, minSayi, maxSayi);
            BenzersizSayilariAta(sagSutun, minSayi, maxSayi);
            BenzersizIslemleriAta(ortaSutun, islemHavuzu);

            SayiButonu rastgeleSol = solSutun[Random.Range(0, solSutun.Count)];
            SayiButonu rastgeleOrta = ortaSutun[Random.Range(0, ortaSutun.Count)];
            SayiButonu rastgeleSag = sagSutun[Random.Range(0, sagSutun.Count)];

            int sayi1 = RakamOku(rastgeleSol);
            int sayi2 = RakamOku(rastgeleSag);
            string islem = IslemOku(rastgeleOrta);

            if (!GecerliHedefMi(sayi1, islem, sayi2))
                continue;

            anlikHedefSayi = Hesapla(sayi1, islem, sayi2);
            hedefYaziUI.text = anlikHedefSayi.ToString();
            return;
        }

        BenzersizSayilariAta(solSutun, minSayi, maxSayi);
        BenzersizSayilariAta(sagSutun, minSayi, maxSayi);
        BenzersizIslemleriAta(ortaSutun, islemHavuzu);
        anlikHedefSayi = minSayi + minSayi;
        hedefYaziUI.text = anlikHedefSayi.ToString();
    }

    private void ZorlukAyarlariAl(out int minSayi, out int maxSayi, out List<string> islemHavuzu)
    {
        if (aktifMod == OyunModu.Klasik || aktifMod == OyunModu.ZamanaKarsi)
        {
            minSayi = 1;
            maxSayi = 9;
            islemHavuzu = new List<string> { "+", "-", "x" };
            return;
        }

        switch (aktifZorluk)
        {
            case ZorlukSeviyesi.Kolay:
                minSayi = 1;
                maxSayi = 5;
                islemHavuzu = new List<string> { "+", "-" };
                break;
            case ZorlukSeviyesi.Zor:
                minSayi = 1;
                maxSayi = 9;
                islemHavuzu = new List<string> { "+", "-", "x", "/" };
                break;
            default:
                minSayi = 1;
                maxSayi = 9;
                islemHavuzu = new List<string> { "+", "-", "x" };
                break;
        }
    }

    private bool GecerliHedefMi(int sayi1, string islem, int sayi2)
    {
        if (!IslemGecerliMi(islem)) return false;
        if (islem == "/" && (sayi2 == 0 || sayi1 % sayi2 != 0)) return false;

        int hedef = Hesapla(sayi1, islem, sayi2);
        if (aktifMod == OyunModu.Ozel && aktifZorluk == ZorlukSeviyesi.Kolay && hedef < 0)
            return false;

        return true;
    }

    public Sprite SpriteGetir(string deger)
    {
        if (deger == "+") return artiSprite;
        if (deger == "-") return eksiSprite;
        if (deger == "x") return carpiSprite;
        if (deger == "/") return boluSprite;

        int rakam = int.Parse(deger);
        return rakamSpriteleri[rakam];
    }

    private void CevabiKontrolEt()
    {
        if (!SecimdenDegerleriAl(out int sayi1, out string islem, out int sayi2))
            return;

        if (CozumMu(sayi1, islem, sayi2))
        {
            if (aktifMod == OyunModu.ZamanaKarsi && ZamanaKarsiYoneticisi.ornek != null)
                ZamanaKarsiYoneticisi.ornek.DogruCevap();

            YeniBolumOlustur();
            return;
        }

        int sonuc = Hesapla(sayi1, islem, sayi2);
        Debug.Log("Wrong. You got " + sonuc + ", target was " + anlikHedefSayi + ".");
    }

    private bool SecimdenDegerleriAl(out int sayi1, out string islem, out int sayi2)
    {
        sayi1 = 0;
        sayi2 = 0;
        islem = "";

        if (secilenButonlar.Count != 3) return false;

        foreach (SayiButonu b in secilenButonlar)
        {
            if (b.sutunNo == 0) sayi1 = RakamOku(b);
            else if (b.sutunNo == 1) islem = IslemOku(b);
            else if (b.sutunNo == 2) sayi2 = RakamOku(b);
        }

        return IslemGecerliMi(islem);
    }

    private static int RakamOku(SayiButonu buton) => int.Parse(buton.karakter.Trim());

    private static string IslemOku(SayiButonu buton)
    {
        string k = buton.karakter.Trim().ToLowerInvariant();
        if (k == "*" || k == "\u00d7") return "x";
        if (k == "\u00f7") return "/";
        return k;
    }

    private bool IslemGecerliMi(string islem)
    {
        if (islem == "+" || islem == "-") return true;
        if (islem == "x") return CarpiAktifMi();
        if (islem == "/") return BoluAktifMi();
        return false;
    }

    private bool CarpiAktifMi()
    {
        if (aktifMod == OyunModu.Klasik || aktifMod == OyunModu.ZamanaKarsi) return true;
        return aktifZorluk == ZorlukSeviyesi.Orta || aktifZorluk == ZorlukSeviyesi.Zor;
    }

    private bool BoluAktifMi()
    {
        if (aktifMod == OyunModu.Klasik || aktifMod == OyunModu.ZamanaKarsi) return false;
        return aktifZorluk == ZorlukSeviyesi.Zor;
    }

    private static int Hesapla(int sayi1, string islem, int sayi2)
    {
        if (islem == "+") return sayi1 + sayi2;
        if (islem == "-") return sayi1 - sayi2;
        if (islem == "x") return sayi1 * sayi2;
        if (islem == "/" && sayi2 != 0) return sayi1 / sayi2;
        return int.MinValue;
    }

    private bool CozumMu(int sayi1, string islem, int sayi2) =>
        IslemGecerliMi(islem) && Hesapla(sayi1, islem, sayi2) == anlikHedefSayi;

    private void BenzersizSayilariAta(List<SayiButonu> sutun, int minSayi, int maxSayi)
    {
        List<int> havuz = new List<int>();
        for (int i = minSayi; i <= maxSayi; i++) havuz.Add(i);

        for (int i = 0; i < sutun.Count; i++)
        {
            if (havuz.Count == 0)
            {
                for (int n = minSayi; n <= maxSayi; n++) havuz.Add(n);
            }

            int index = Random.Range(0, havuz.Count);
            sutun[i].DegeriAyarla(havuz[index].ToString());
            havuz.RemoveAt(index);
        }
    }

    private void BenzersizIslemleriAta(List<SayiButonu> sutun, List<string> islemHavuzu)
    {
        List<string> havuz = new List<string>(islemHavuzu);

        for (int i = 0; i < sutun.Count; i++)
        {
            if (havuz.Count == 0)
                havuz = new List<string>(islemHavuzu);

            int index = Random.Range(0, havuz.Count);
            sutun[i].DegeriAyarla(havuz[index]);

            if (havuz.Count > 1)
                havuz.RemoveAt(index);
        }
    }

    private static Sprite SpriteBul(string spriteAdi)
    {
        Sprite[] tumSpriteler = Resources.FindObjectsOfTypeAll<Sprite>();
        foreach (Sprite s in tumSpriteler)
        {
            if (s.name == spriteAdi) return s;
        }
        return null;
    }

    public void ButonEtkilesimi(SayiButonu basilanButon)
    {
        if (!oyunAktifMi || AyarlarYoneticisi.AyarlarAcikMi) return;

        if (basilanButon.secildiMi && secilenButonlar.Count >= 2)
        {
            if (secilenButonlar[secilenButonlar.Count - 2] == basilanButon)
            {
                SayiButonu iptalEdilecek = secilenButonlar[secilenButonlar.Count - 1];
                iptalEdilecek.secildiMi = false;
                secilenButonlar.RemoveAt(secilenButonlar.Count - 1);
                YaziyiGuncelle();
                return;
            }
        }

        if (!basilanButon.secildiMi)
        {
            for (int i = secilenButonlar.Count - 1; i >= 0; i--)
            {
                if (secilenButonlar[i].sutunNo == basilanButon.sutunNo)
                {
                    secilenButonlar[i].secildiMi = false;
                    secilenButonlar.RemoveAt(i);
                    break;
                }
            }

            basilanButon.secildiMi = true;
            secilenButonlar.Add(basilanButon);
            YaziyiGuncelle();
        }
    }

    private void YaziyiGuncelle()
    {
        suAnkiIslem = "";
        foreach (SayiButonu buton in secilenButonlar) suAnkiIslem += buton.karakter;
        canliYaziUI.text = suAnkiIslem;
    }
}
