using UnityEngine;

public class AnaMenuYoneticisi : MonoBehaviour
{
    [SerializeField] private GameObject oyunPaneli;
    [SerializeField] private OyunYoneticisi oyunYoneticisi;

    private GameObject menuPaneli;
    private const float MenuButonOlcegi = 0.78f;
    private const float MenuButonAraligi = 30f;

    private void Awake()
    {
        if (oyunPaneli == null)
        {
            GameObject cember = GameObject.Find("Cember");
            if (cember != null) oyunPaneli = cember;
        }

        if (oyunYoneticisi == null)
            oyunYoneticisi = OyunYoneticisi.ornek ?? FindFirstObjectByType<OyunYoneticisi>();

        if (oyunPaneli != null)
            oyunPaneli.SetActive(false);
    }

    private void Start()
    {
        MenuOlustur();
        AnaMenuyuGoster();
    }

    private void MenuOlustur()
    {
        GameObject canvasObj = GameObject.Find("BackGround");
        if (canvasObj == null) return;

        menuPaneli = OyunUIYardimci.TamEkranPanel(canvasObj.transform, "AnaMenu", 0.55f);

        OyunUIYardimci.LogoOrtaUstOlustur(menuPaneli.transform, 92f, new Vector2(200f, 200f));

        Sprite playSprite = OyunUIYardimci.PlayButonSpriteBul();
        Vector2 menuBoyut = OyunUIYardimci.SpriteButonBoyutu(playSprite, MenuButonOlcegi);
        float butonAdimi = menuBoyut.y + MenuButonAraligi;
        float ilkButonY = 35f;

        MenuButonuEkle(new Vector2(0f, ilkButonY), ModSeciminiAc, playSprite, menuBoyut);
        MenuButonuEkle(new Vector2(0f, ilkButonY - butonAdimi), AyarlariAc, OyunUIYardimci.MenuButonSpriteBul(), menuBoyut);
        MenuButonuEkle(new Vector2(0f, ilkButonY - butonAdimi * 2f), OyundanCik, OyunUIYardimci.QuitButonSpriteBul(), menuBoyut);
    }

    private void MenuButonuEkle(Vector2 pozisyon, UnityEngine.Events.UnityAction tiklama, Sprite sprite, Vector2 boyut)
    {
        OyunUIYardimci.ButonOlustur(
            menuPaneli.transform,
            "",
            pozisyon,
            boyut,
            tiklama,
            sprite,
            metinGoster: false,
            tamBoyutKullan: true);
    }

    public void AnaMenuyuGoster()
    {
        if (ModSecimYoneticisi.ornek != null)
            ModSecimYoneticisi.ornek.Kapat();

        if (ZorlukSecimYoneticisi.ornek != null)
            ZorlukSecimYoneticisi.ornek.Kapat();

        if (AyarlarYoneticisi.ornek != null)
            AyarlarYoneticisi.ornek.Kapat();

        if (menuPaneli != null) menuPaneli.SetActive(true);
        if (oyunPaneli != null) oyunPaneli.SetActive(false);
        if (oyunYoneticisi != null) oyunYoneticisi.OyunuDurdur();
        if (PiModYoneticisi.ornek != null)
            PiModYoneticisi.ornek.Kapat();
        OyunIciGeriButonunuGizle();
    }

    private void ModSeciminiAc()
    {
        if (menuPaneli != null) menuPaneli.SetActive(false);

        if (ModSecimYoneticisi.ornek != null)
            ModSecimYoneticisi.ornek.Ac();
    }

    public void OyunuBaslatKlasik()
    {
        MenuleriKapat();
        if (oyunPaneli != null) oyunPaneli.SetActive(true);
        if (oyunYoneticisi != null) oyunYoneticisi.OyunuBaslatKlasik();
        OyunIciGeriButonunuGoster();
    }

    public void OyunuBaslatOzel(ZorlukSeviyesi zorluk)
    {
        MenuleriKapat();
        if (oyunPaneli != null) oyunPaneli.SetActive(true);
        if (oyunYoneticisi != null) oyunYoneticisi.OyunuBaslatOzel(zorluk);
        OyunIciGeriButonunuGoster();
    }

    public void OyunuBaslatZamanaKarsi()
    {
        MenuleriKapat();
        if (PiModYoneticisi.ornek != null)
            PiModYoneticisi.ornek.Kapat();
        if (oyunPaneli != null) oyunPaneli.SetActive(true);
        if (oyunYoneticisi != null) oyunYoneticisi.OyunuBaslatZamanaKarsi();
        OyunIciGeriButonunuGoster();
    }

    public void OyunuBaslatPiModu()
    {
        MenuleriKapat();
        if (oyunYoneticisi != null)
            oyunYoneticisi.OyunuDurdur();
        if (ZamanaKarsiYoneticisi.ornek != null)
            ZamanaKarsiYoneticisi.ornek.Durdur();
        if (oyunPaneli != null)
            oyunPaneli.SetActive(false);
        OyunIciGeriButonunuGizle();
        if (PiModYoneticisi.ornek != null)
            PiModYoneticisi.ornek.Baslat();
    }

    public void ModSecimMenuyeDon()
    {
        ModSecimMenuyeDon(true);
    }

    public void ModSecimMenuyeDon(bool zamanaKarsiKaydet)
    {
        if (zamanaKarsiKaydet &&
            oyunYoneticisi != null &&
            oyunYoneticisi.aktifMod == OyunModu.ZamanaKarsi &&
            ZamanaKarsiYoneticisi.ornek != null)
        {
            ZamanaKarsiYoneticisi.ornek.OturumdanCik(false);
        }
        else if (ZamanaKarsiYoneticisi.ornek != null)
        {
            ZamanaKarsiYoneticisi.ornek.Durdur();
        }

        if (oyunYoneticisi != null)
            oyunYoneticisi.OyunuDurdur();

        if (oyunPaneli != null)
            oyunPaneli.SetActive(false);

        if (PiModYoneticisi.ornek != null)
            PiModYoneticisi.ornek.Kapat();

        OyunIciGeriButonunuGizle();

        if (ZorlukSecimYoneticisi.ornek != null)
            ZorlukSecimYoneticisi.ornek.Kapat();

        if (menuPaneli != null)
            menuPaneli.SetActive(false);

        if (ModSecimYoneticisi.ornek != null)
            ModSecimYoneticisi.ornek.Ac();
    }

    private void OyunIciGeriButonunuGoster()
    {
        if (OyunIciMenuYoneticisi.ornek != null)
            OyunIciMenuYoneticisi.ornek.Goster();
    }

    private void OyunIciGeriButonunuGizle()
    {
        if (OyunIciMenuYoneticisi.ornek != null)
            OyunIciMenuYoneticisi.ornek.Gizle();
    }

    private void MenuleriKapat()
    {
        if (AyarlarYoneticisi.ornek != null)
            AyarlarYoneticisi.ornek.Kapat();

        if (menuPaneli != null) menuPaneli.SetActive(false);
        if (ModSecimYoneticisi.ornek != null)
            ModSecimYoneticisi.ornek.Kapat();
        if (ZorlukSecimYoneticisi.ornek != null)
            ZorlukSecimYoneticisi.ornek.Kapat();
    }

    private void AyarlariAc()
    {
        if (AyarlarYoneticisi.ornek != null)
            AyarlarYoneticisi.ornek.Ac();
    }

    public void OyundanCik()
    {
        if (oyunYoneticisi != null &&
            oyunYoneticisi.aktifMod == OyunModu.ZamanaKarsi &&
            ZamanaKarsiYoneticisi.ornek != null)
        {
            ZamanaKarsiYoneticisi.ornek.OturumdanCik(false);
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
