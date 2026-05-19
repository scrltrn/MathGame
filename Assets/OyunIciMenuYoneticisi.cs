using UnityEngine;

public class OyunIciMenuYoneticisi : MonoBehaviour
{
    public static OyunIciMenuYoneticisi ornek;

    [SerializeField] private AnaMenuYoneticisi anaMenu;

    private GameObject geriButonObjesi;

    private void Awake()
    {
        ornek = this;
        if (anaMenu == null)
            anaMenu = FindFirstObjectByType<AnaMenuYoneticisi>();
    }

    private void Start()
    {
        GeriButonuOlustur();
        Gizle();
    }

    private void GeriButonuOlustur()
    {
        GameObject canvasObj = GameObject.Find("BackGround");
        if (canvasObj == null) return;

        Sprite backSprite = OyunUIYardimci.BackButonSpriteBul();
        Vector2 backBoyut = OyunUIYardimci.SpriteButonBoyutu(backSprite, 0.62f);

        geriButonObjesi = OyunUIYardimci.ButonOlustur(
            canvasObj.transform,
            "",
            new Vector2(-430f, 220f),
            backBoyut,
            ModSecimMenuyeDon,
            backSprite,
            metinGoster: false,
            tamBoyutKullan: true).gameObject;

        geriButonObjesi.name = "OyunGeriButonu";
    }

    public void Goster()
    {
        if (geriButonObjesi != null)
        {
            geriButonObjesi.SetActive(true);
            geriButonObjesi.transform.SetAsLastSibling();
        }
    }

    public void Gizle()
    {
        if (geriButonObjesi != null)
            geriButonObjesi.SetActive(false);
    }

    private void ModSecimMenuyeDon()
    {
        if (anaMenu != null)
            anaMenu.ModSecimMenuyeDon();
    }
}
