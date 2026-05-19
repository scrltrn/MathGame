using UnityEngine;

public class ZorlukSecimYoneticisi : MonoBehaviour
{
    public static ZorlukSecimYoneticisi ornek;

    [SerializeField] private AnaMenuYoneticisi anaMenu;

    private GameObject zorlukPaneli;

    private const float ZorlukButonOlcegi = 0.78f;
    private const float ButonAraligi = 28f;

    private void Awake()
    {
        ornek = this;
        if (anaMenu == null)
            anaMenu = FindFirstObjectByType<AnaMenuYoneticisi>();
    }

    private void Start()
    {
        PanelOlustur();
        Kapat();
    }

    private void PanelOlustur()
    {
        GameObject canvasObj = GameObject.Find("BackGround");
        if (canvasObj == null) return;

        zorlukPaneli = OyunUIYardimci.TamEkranPanel(canvasObj.transform, "ZorlukSecimPaneli", 0.55f);

        OyunUIYardimci.BaslikOlustur(
            zorlukPaneli.transform,
            "CUSTOM — DIFFICULTY",
            new Vector2(0f, 268f),
            42f,
            new Color(1f, 0.95f, 0.4f));

        Sprite easySprite = OyunUIYardimci.EasySpriteBul();
        Sprite mediumSprite = OyunUIYardimci.MediumSpriteBul();
        Sprite hardSprite = OyunUIYardimci.HardSpriteBul();
        Sprite backSprite = OyunUIYardimci.BackButonSpriteBul();

        Vector2 zorlukBoyut = OyunUIYardimci.SpriteButonBoyutu(easySprite, ZorlukButonOlcegi);
        float butonAdimi = zorlukBoyut.y + ButonAraligi;
        float ilkButonY = 118f;

        ZorlukButonuEkle(new Vector2(0f, ilkButonY), () => ZorlukSec(ZorlukSeviyesi.Kolay), easySprite, zorlukBoyut);
        ZorlukButonuEkle(new Vector2(0f, ilkButonY - butonAdimi), () => ZorlukSec(ZorlukSeviyesi.Orta), mediumSprite, zorlukBoyut);
        ZorlukButonuEkle(new Vector2(0f, ilkButonY - butonAdimi * 2f), () => ZorlukSec(ZorlukSeviyesi.Zor), hardSprite, zorlukBoyut);
        ZorlukButonuEkle(new Vector2(0f, ilkButonY - butonAdimi * 3f), ModMenusuDon, backSprite, zorlukBoyut);
    }

    private void ZorlukButonuEkle(Vector2 pozisyon, UnityEngine.Events.UnityAction tiklama, Sprite sprite, Vector2 boyut)
    {
        OyunUIYardimci.ButonOlustur(
            zorlukPaneli.transform,
            "",
            pozisyon,
            boyut,
            tiklama,
            sprite,
            metinGoster: false,
            tamBoyutKullan: true);
    }

    public void Ac()
    {
        if (zorlukPaneli == null) return;
        zorlukPaneli.SetActive(true);
        zorlukPaneli.transform.SetAsLastSibling();
    }

    public void Kapat()
    {
        if (zorlukPaneli != null)
            zorlukPaneli.SetActive(false);
    }

    private void ZorlukSec(ZorlukSeviyesi zorluk)
    {
        Kapat();
        if (ModSecimYoneticisi.ornek != null)
            ModSecimYoneticisi.ornek.Kapat();

        if (anaMenu != null)
            anaMenu.OyunuBaslatOzel(zorluk);
    }

    private void ModMenusuDon()
    {
        Kapat();
        if (ModSecimYoneticisi.ornek != null)
            ModSecimYoneticisi.ornek.Ac();
    }
}
