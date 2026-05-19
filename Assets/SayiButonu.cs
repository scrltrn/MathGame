using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SayiButonu : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public string karakter;
    public int sutunNo;
    public bool secildiMi = false;

    public Image sayiGorseli;

    private void Awake()
    {
        ButonHoverEfekti.Ekle(gameObject);
    }

    public void DegeriAyarla(string yeniDeger)
    {
        karakter = yeniDeger;

        Sprite sprite = OyunYoneticisi.ornek.SpriteGetir(yeniDeger);
        sayiGorseli.sprite = sprite;
        sayiGorseli.preserveAspect = true;

        if (sprite == null) return;

        Vector2 boyut = OyunUIYardimci.SpriteButonBoyutu(sprite, OyunUIYardimci.OyunGridButonOlcegi);
        RectTransform rt = (RectTransform)transform;
        rt.sizeDelta = boyut;

        RectTransform imgRt = sayiGorseli.rectTransform;
        imgRt.anchorMin = Vector2.zero;
        imgRt.anchorMax = Vector2.one;
        imgRt.offsetMin = Vector2.zero;
        imgRt.offsetMax = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SesYoneticisi.TiklamaCal();
        OyunYoneticisi.ornek.ButonEtkilesimi(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            OyunYoneticisi.ornek.ButonEtkilesimi(this);
        }
    }
}
