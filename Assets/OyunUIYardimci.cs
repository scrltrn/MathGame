using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public static class OyunUIYardimci
{
    public const float OyunGridButonOlcegi = 0.54f;
    private static readonly Vector2 VarsayilanMenuButonBoyutu = new Vector2(248f, 127f);

    public static Vector2 SpriteButonBoyutu(Sprite sprite, float olcek = 1f)
    {
        if (sprite == null)
            return VarsayilanMenuButonBoyutu * olcek;

        Rect r = sprite.rect;
        return new Vector2(r.width * olcek, r.height * olcek);
    }

    public static Vector2 SpriteKutuyaSigdir(Vector2 kutu, Sprite sprite)
    {
        if (sprite == null) return kutu;

        float spriteEn = sprite.rect.width;
        float spriteBoy = sprite.rect.height;
        if (spriteEn <= 0f || spriteBoy <= 0f) return kutu;

        float olcek = Mathf.Min(kutu.x / spriteEn, kutu.y / spriteBoy);
        return new Vector2(spriteEn * olcek, spriteBoy * olcek);
    }

    public static TMP_FontAsset FontBul()
    {
        TMP_FontAsset[] fontlar = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
        foreach (TMP_FontAsset f in fontlar)
        {
            if (f.name.Contains("Milk Choco")) return f;
        }
        return null;
    }

    public static Sprite MenuButonSpriteBul()
    {
        return YoneticidenSprite(o => o.settingsMenuSprite, "mathgamebuttons 2_6", "mathgamebuttons 1_0");
    }

    public static Sprite PlayButonSpriteBul()
    {
        return YoneticidenSprite(o => o.playMenuSprite, "mathgamebuttons 2_5", "mathgamebuttons 1_5");
    }

    public static Sprite BackButonSpriteBul()
    {
        return YoneticidenSprite(o => o.backMenuSprite, "mathgamebuttons 2_23", "mathgamebuttons 1_17");
    }

    public static Sprite QuitButonSpriteBul()
    {
        return YoneticidenSprite(o => o.quitMenuSprite, "mathgamebuttons 2_14", "mathgamebuttons 1_11");
    }

    public static Sprite ClassicModeSpriteBul()
    {
        return YoneticidenSprite(o => o.classicModeSprite, "mathgamebuttons 2_15");
    }

    public static Sprite CustomModeSpriteBul()
    {
        return YoneticidenSprite(o => o.customModeSprite, "mathgamebuttons 2_24");
    }

    public static Sprite AgainstTimeSpriteBul()
    {
        return YoneticidenSprite(o => o.againstTimeSprite, "mathgamebuttons 2_7");
    }

    private static Sprite gameLogoOnbellek;

    public static Sprite GameLogoSpriteBul()
    {
        if (gameLogoOnbellek != null)
            return gameLogoOnbellek;

        if (OyunYoneticisi.ornek != null && OyunYoneticisi.ornek.gameLogoSprite != null)
            return gameLogoOnbellek = OyunYoneticisi.ornek.gameLogoSprite;

        gameLogoOnbellek = YoneticidenSprite(o => o.gameLogoSprite, "Image_0");
        if (gameLogoOnbellek != null)
            return gameLogoOnbellek;

        gameLogoOnbellek = Resources.Load<Sprite>("GameLogo");
        if (gameLogoOnbellek != null)
            return gameLogoOnbellek;

        Sprite[] parcalar = Resources.LoadAll<Sprite>("GameLogo");
        if (parcalar != null && parcalar.Length > 0)
        {
            foreach (Sprite s in parcalar)
            {
                if (s.name == "Image_0" || s.name.Contains("GameLogo"))
                {
                    gameLogoOnbellek = s;
                    return gameLogoOnbellek;
                }
            }

            gameLogoOnbellek = parcalar[0];
            return gameLogoOnbellek;
        }

        return null;
    }

    public static GameObject LogoOrtaUstOlustur(Transform parent, float usttenAsagi = 88f, Vector2 maxBoyut = default)
    {
        if (maxBoyut == Vector2.zero)
            maxBoyut = new Vector2(200f, 200f);

        Vector2 ortaUst = new Vector2(0.5f, 1f);
        GameObject logo = LogoOlustur(
            parent,
            new Vector2(0f, -usttenAsagi),
            maxBoyut,
            ortaUst,
            ortaUst,
            ortaUst);

        if (logo != null)
            logo.transform.SetAsLastSibling();

        return logo;
    }

    public static GameObject LogoSolUstOlustur(Transform parent, float kenarBoslugu = 22f, Vector2 maxBoyut = default)
    {
        if (maxBoyut == Vector2.zero)
            maxBoyut = new Vector2(155f, 155f);

        Vector2 solUst = new Vector2(0f, 1f);
        GameObject logo = LogoOlustur(
            parent,
            new Vector2(kenarBoslugu, -kenarBoslugu),
            maxBoyut,
            solUst,
            solUst,
            solUst);

        if (logo != null)
            logo.transform.SetAsLastSibling();

        return logo;
    }

    /// <summary>Mode select: sits in the dark top-left border of the background art.</summary>
    public static GameObject LogoSolUstKoseOlustur(Transform parent, Vector2 pozisyon = default, Vector2 maxBoyut = default)
    {
        if (pozisyon == Vector2.zero)
            pozisyon = new Vector2(68f, -86f);

        if (maxBoyut == Vector2.zero)
            maxBoyut = new Vector2(118f, 118f);

        Vector2 solUst = new Vector2(0f, 1f);
        GameObject logo = LogoOlustur(parent, pozisyon, maxBoyut, solUst, solUst, solUst);
        if (logo != null)
            logo.transform.SetAsLastSibling();

        return logo;
    }

    public static GameObject LogoOlustur(
        Transform parent,
        Vector2 anchoredPosition,
        Vector2 maxBoyut,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 pivot,
        Sprite sprite = null)
    {
        sprite ??= GameLogoSpriteBul();
        if (sprite == null)
        {
            Debug.LogWarning("Oyun logosu bulunamadi. Assets/Arts/Image.png veya Resources/GameLogo.png ekleyin.");
            return null;
        }

        GameObject obj = new GameObject("OyunLogosu", typeof(RectTransform));
        obj.transform.SetParent(parent, false);

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = pivot;
        rt.anchoredPosition = anchoredPosition;
        rt.sizeDelta = SpriteKutuyaSigdir(maxBoyut, sprite);

        Image img = obj.AddComponent<Image>();
        img.sprite = sprite;
        img.type = Image.Type.Simple;
        img.preserveAspect = true;
        img.color = Color.white;
        img.raycastTarget = false;

        return obj;
    }

    public static Sprite EasySpriteBul()
    {
        return YoneticidenSprite(o => o.easySprite, "mathgamebuttons 2_16");
    }

    public static Sprite MediumSpriteBul()
    {
        return YoneticidenSprite(o => o.mediumSprite, "mathgamebuttons 2_25");
    }

    public static Sprite HardSpriteBul()
    {
        return YoneticidenSprite(o => o.hardSprite, "mathgamebuttons 2_8");
    }

    public static Sprite RakamSpriteBul(int rakam)
    {
        rakam = Mathf.Clamp(rakam, 0, 9);
        if (OyunYoneticisi.ornek != null &&
            OyunYoneticisi.ornek.rakamSpriteleri != null &&
            rakam < OyunYoneticisi.ornek.rakamSpriteleri.Length &&
            OyunYoneticisi.ornek.rakamSpriteleri[rakam] != null)
        {
            return OyunYoneticisi.ornek.rakamSpriteleri[rakam];
        }

        return SpriteBul("mathgamebuttons 2_" + rakam);
    }

    private static Sprite YoneticidenSprite(System.Func<OyunYoneticisi, Sprite> alan, params string[] yedekAdlar)
    {
        if (OyunYoneticisi.ornek != null)
        {
            Sprite s = alan(OyunYoneticisi.ornek);
            if (s != null) return s;
        }

        foreach (string ad in yedekAdlar)
        {
            Sprite bulunan = SpriteBul(ad);
            if (bulunan != null) return bulunan;
        }

        return null;
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

    public static GameObject TamEkranPanel(Transform parent, string isim, float karartma = 0.55f)
    {
        GameObject panel = new GameObject(isim, typeof(RectTransform));
        panel.transform.SetParent(parent, false);

        RectTransform rt = panel.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        Image arkaPlan = panel.AddComponent<Image>();
        arkaPlan.color = new Color(0f, 0f, 0f, karartma);
        arkaPlan.raycastTarget = true;

        return panel;
    }

    public static TextMeshProUGUI BaslikOlustur(Transform parent, string metin, Vector2 pozisyon, float fontBoyutu, Color renk)
    {
        GameObject obj = new GameObject("Baslik", typeof(RectTransform));
        obj.transform.SetParent(parent, false);

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pozisyon;
        rt.sizeDelta = new Vector2(600f, 80f);

        TextMeshProUGUI yazi = obj.AddComponent<TextMeshProUGUI>();
        yazi.text = metin;
        yazi.fontSize = fontBoyutu;
        yazi.alignment = TextAlignmentOptions.Center;
        yazi.color = renk;

        TMP_FontAsset font = FontBul();
        if (font != null) yazi.font = font;

        return yazi;
    }

    public static Button ButonOlustur(
        Transform parent,
        string metin,
        Vector2 pozisyon,
        Vector2 boyut,
        UnityAction tiklama,
        Sprite sprite = null,
        bool metinGoster = true,
        bool tamBoyutKullan = false)
    {
        string butonAdi = string.IsNullOrEmpty(metin) ? "Buton" : metin;
        GameObject butonObj = new GameObject(butonAdi + "Butonu", typeof(RectTransform));
        butonObj.transform.SetParent(parent, false);

        RectTransform rt = butonObj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pozisyon;

        Image img = butonObj.AddComponent<Image>();
        Sprite kullanilacakSprite = sprite != null ? sprite : MenuButonSpriteBul();
        if (kullanilacakSprite != null)
        {
            Vector2 spriteBoyut = SpriteButonBoyutu(kullanilacakSprite);
            rt.sizeDelta = tamBoyutKullan
                ? SpriteKutuyaSigdir(boyut, kullanilacakSprite)
                : new Vector2(Mathf.Max(boyut.x, spriteBoyut.x), Mathf.Max(boyut.y, spriteBoyut.y));

            img.sprite = kullanilacakSprite;
            img.type = Image.Type.Simple;
            img.color = Color.white;
            img.preserveAspect = true;
        }
        else
        {
            rt.sizeDelta = boyut;
            img.color = new Color(0.25f, 0.55f, 0.9f);
        }

        Button btn = butonObj.AddComponent<Button>();
        btn.targetGraphic = img;
        ColorBlock renkler = btn.colors;
        renkler.normalColor = Color.white;
        renkler.highlightedColor = new Color(0.92f, 0.92f, 0.92f);
        renkler.pressedColor = new Color(0.82f, 0.82f, 0.82f);
        renkler.selectedColor = Color.white;
        renkler.disabledColor = new Color(0.78f, 0.78f, 0.78f, 0.5f);
        btn.colors = renkler;
        btn.onClick.AddListener(() => SesliTikla(tiklama));
        ButonHoverEfekti.Ekle(butonObj);

        if (metinGoster && !string.IsNullOrEmpty(metin))
        {
            GameObject yaziObj = new GameObject("Yazi", typeof(RectTransform));
            yaziObj.transform.SetParent(butonObj.transform, false);

            RectTransform yaziRt = yaziObj.GetComponent<RectTransform>();
            yaziRt.anchorMin = Vector2.zero;
            yaziRt.anchorMax = Vector2.one;
            yaziRt.offsetMin = Vector2.zero;
            yaziRt.offsetMax = Vector2.zero;

            TextMeshProUGUI yazi = yaziObj.AddComponent<TextMeshProUGUI>();
            yazi.text = metin;
            yazi.fontSize = 32f;
            yazi.alignment = TextAlignmentOptions.Center;
            yazi.color = Color.white;

            TMP_FontAsset font = FontBul();
            if (font != null) yazi.font = font;
        }

        return btn;
    }

    private static void SesliTikla(UnityAction tiklama)
    {
        SesYoneticisi.TiklamaCal();
        tiklama?.Invoke();
    }

    public static TextMeshProUGUI EtiketOlustur(Transform parent, string metin, Vector2 pozisyon, float fontBoyutu = 28f)
    {
        GameObject obj = new GameObject("Etiket", typeof(RectTransform));
        obj.transform.SetParent(parent, false);

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pozisyon;
        rt.sizeDelta = new Vector2(400f, 40f);

        TextMeshProUGUI yazi = obj.AddComponent<TextMeshProUGUI>();
        yazi.text = metin;
        yazi.fontSize = fontBoyutu;
        yazi.alignment = TextAlignmentOptions.Left;
        yazi.color = Color.white;

        TMP_FontAsset font = FontBul();
        if (font != null) yazi.font = font;

        return yazi;
    }
}
