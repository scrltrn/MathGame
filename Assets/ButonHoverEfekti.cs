using UnityEngine;
using UnityEngine.EventSystems;

public class ButonHoverEfekti : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverOlcek = 1.1f;
    [SerializeField] private float animHiz = 14f;

    private Vector3 normalOlcek;
    private Vector3 hedefOlcek;

    private void Awake()
    {
        normalOlcek = transform.localScale;
        hedefOlcek = normalOlcek;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            hedefOlcek,
            Time.unscaledDeltaTime * animHiz);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hedefOlcek = normalOlcek * hoverOlcek;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hedefOlcek = normalOlcek;
    }

    public static void Ekle(GameObject obj, float olcek = 1.1f)
    {
        if (obj == null) return;

        ButonHoverEfekti efekt = obj.GetComponent<ButonHoverEfekti>();
        if (efekt == null) efekt = obj.AddComponent<ButonHoverEfekti>();

        efekt.hoverOlcek = olcek;
        efekt.normalOlcek = obj.transform.localScale;
        efekt.hedefOlcek = efekt.normalOlcek;
    }
}
