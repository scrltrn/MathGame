using UnityEngine;
using TMPro;

public class ModSecimYoneticisi : MonoBehaviour
{
    public static ModSecimYoneticisi ornek;

    [SerializeField] private AnaMenuYoneticisi anaMenu;

    private GameObject modPaneli;
    private GameObject modLogosu;
    private TextMeshProUGUI uyariYazi;

    private const float ModButonOlcegi = 0.62f;
    private const float ModButonAraligi = 22f;

    private void Awake()
    {
        ornek = this;
        if (anaMenu == null)
            anaMenu = FindFirstObjectByType<AnaMenuYoneticisi>();
    }

    private void Start()
    {
        ModPanelOlustur();
        Kapat();
    }

    private void ModPanelOlustur()
    {
        GameObject canvasObj = GameObject.Find("BackGround");
        if (canvasObj == null) return;

        modPaneli = OyunUIYardimci.TamEkranPanel(canvasObj.transform, "ModSecimPaneli", 0.55f);

        modLogosu = OyunUIYardimci.LogoSolUstKoseOlustur(canvasObj.transform);
        if (modLogosu != null)
        {
            modLogosu.transform.SetSiblingIndex(1);
            modLogosu.SetActive(false);
        }

        OyunUIYardimci.BaslikOlustur(
            modPaneli.transform,
            "SELECT MODE",
            new Vector2(0f, 268f),
            48f,
            new Color(1f, 0.95f, 0.4f));

        Sprite classicSprite = OyunUIYardimci.ClassicModeSpriteBul();
        Sprite customSprite = OyunUIYardimci.CustomModeSpriteBul();
        Sprite againstSprite = OyunUIYardimci.AgainstTimeSpriteBul();
        Sprite backSprite = OyunUIYardimci.BackButonSpriteBul();

        Vector2 modBoyut = OyunUIYardimci.SpriteButonBoyutu(classicSprite, ModButonOlcegi);
        float butonAdimi = modBoyut.y + ModButonAraligi;
        float ilkButonY = 155f;

        ModButonuEkle(new Vector2(0f, ilkButonY), KlasikSec, classicSprite, modBoyut);
        ModButonuEkle(new Vector2(0f, ilkButonY - butonAdimi), OzelModAc, customSprite, modBoyut);
        ModButonuEkle(
            new Vector2(0f, ilkButonY - butonAdimi * 2f),
            ZamanaKarsiSec,
            againstSprite,
            modBoyut);
        ModButonuEkle(
            new Vector2(0f, ilkButonY - butonAdimi * 3f),
            PiModSec,
            OyunUIYardimci.MenuButonSpriteBul(),
            modBoyut,
            "PI MODE",
            metinSpriteUstunde: true);
        ModButonuEkle(new Vector2(0f, ilkButonY - butonAdimi * 4f), AnaMenuyeDon, backSprite, modBoyut);

        float uyariY = ilkButonY - butonAdimi * 4f - modBoyut.y * 0.5f - 28f;
        uyariYazi = OyunUIYardimci.EtiketOlustur(modPaneli.transform, "", new Vector2(0f, uyariY), 22f);
        uyariYazi.alignment = TextAlignmentOptions.Center;
        uyariYazi.color = new Color(1f, 0.85f, 0.5f);
    }

    private void ModButonuEkle(
        Vector2 pozisyon,
        UnityEngine.Events.UnityAction tiklama,
        Sprite sprite,
        Vector2 boyut,
        string metin = "",
        bool metinSpriteUstunde = false)
    {
        bool spriteVar = sprite != null;
        bool metinGoster = metinSpriteUstunde
            ? !string.IsNullOrEmpty(metin)
            : !spriteVar && !string.IsNullOrEmpty(metin);

        OyunUIYardimci.ButonOlustur(
            modPaneli.transform,
            metinSpriteUstunde ? metin : (spriteVar ? "" : metin),
            pozisyon,
            boyut,
            tiklama,
            sprite,
            metinGoster: metinGoster,
            tamBoyutKullan: true);
    }

    public void Ac()
    {
        if (modPaneli == null) return;

        if (uyariYazi != null)
        {
            int atSkor = ZamanaKarsiYoneticisi.EnYuksekSkorOku();
            int piSkor = PiModYoneticisi.EnIyiSkorOku();
            string satir = "";
            if (atSkor > 0) satir += "Against Time best: " + atSkor;
            if (piSkor > 0)
                satir += (satir.Length > 0 ? "  |  " : "") + "Pi best: " + piSkor + " digits";
            uyariYazi.text = satir;
        }

        modPaneli.SetActive(true);
        modPaneli.transform.SetAsLastSibling();

        if (modLogosu != null)
        {
            modLogosu.SetActive(true);
            modLogosu.transform.SetAsLastSibling();
        }
    }

    public void Kapat()
    {
        if (modPaneli != null)
            modPaneli.SetActive(false);

        if (modLogosu != null)
            modLogosu.SetActive(false);
    }

    private void KlasikSec()
    {
        Kapat();
        if (anaMenu != null)
            anaMenu.OyunuBaslatKlasik();
    }

    private void OzelModAc()
    {
        Kapat();
        if (ZorlukSecimYoneticisi.ornek != null)
            ZorlukSecimYoneticisi.ornek.Ac();
    }

    private void ZamanaKarsiSec()
    {
        Kapat();
        if (anaMenu != null)
            anaMenu.OyunuBaslatZamanaKarsi();
    }

    private void PiModSec()
    {
        Kapat();
        if (anaMenu != null)
            anaMenu.OyunuBaslatPiModu();
    }

    private void AnaMenuyeDon()
    {
        Kapat();
        if (anaMenu != null)
            anaMenu.AnaMenuyuGoster();
    }
}
