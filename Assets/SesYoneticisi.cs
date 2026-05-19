using UnityEngine;

public class SesYoneticisi : MonoBehaviour
{
    public static SesYoneticisi ornek;

    [SerializeField] private AudioClip tiklamaSesi;
    [SerializeField] private AudioClip arkaplanMuzigi;
    [SerializeField] private AudioClip saatTikSesi;
    [SerializeField] [Range(0f, 1f)] private float muzikSesSeviyesi = 0.45f;
    [SerializeField] [Range(0f, 1f)] private float saatTikSesSeviyesi = 0.55f;

    private AudioSource muzikKaynagi;
    private AudioSource efektKaynagi;
    private AudioSource saatTikKaynagi;

    private bool saatTikIsteniyor;
    private bool saatTikDuraklatildi;

    private void Awake()
    {
        if (ornek != null && ornek != this)
        {
            Destroy(this);
            return;
        }

        ornek = this;

        muzikKaynagi = OlusturKaynak("Muzik", arkaplanMuzigi, true);
        efektKaynagi = OlusturKaynak("Efekt", null, false);
        saatTikKaynagi = OlusturKaynak("SaatTik", saatTikSesi, true);

        if (saatTikSesi == null)
            saatTikSesi = SaatTikClipBul();
        if (saatTikKaynagi != null && saatTikKaynagi.clip == null)
            saatTikKaynagi.clip = saatTikSesi;
    }

    private void Start()
    {
        MuzigiBaslat();
        SesSeviyesiniUygula(PlayerPrefs.GetFloat("Ayarlar_Ses", 1f));
    }

    private void Update()
    {
        if (saatTikIsteniyor)
            SaatTikUygula();
    }

    private AudioSource OlusturKaynak(string isim, AudioClip clip, bool dongu)
    {
        GameObject obj = new GameObject(isim);
        obj.transform.SetParent(transform);

        AudioSource kaynak = obj.AddComponent<AudioSource>();
        kaynak.clip = clip;
        kaynak.loop = dongu;
        kaynak.playOnAwake = false;
        kaynak.spatialBlend = 0f;
        return kaynak;
    }

    public void MuzigiBaslat()
    {
        if (muzikKaynagi == null || arkaplanMuzigi == null) return;
        if (!muzikKaynagi.isPlaying)
            muzikKaynagi.Play();
    }

    public static void TiklamaCal()
    {
        if (ornek == null) return;
        ornek.TiklamaOynat();
    }

    public static void SaatTikAc(bool acik)
    {
        if (ornek == null) return;
        ornek.saatTikIsteniyor = acik;
        ornek.SaatTikUygula();
    }

    public static void SaatTikYenile()
    {
        if (ornek != null && ornek.saatTikIsteniyor)
            ornek.SaatTikUygula();
    }

    private void TiklamaOynat()
    {
        if (tiklamaSesi == null || efektKaynagi == null) return;
        efektKaynagi.PlayOneShot(tiklamaSesi);
    }

    private void SaatTikUygula()
    {
        if (saatTikKaynagi == null || saatTikSesi == null) return;

        if (!saatTikIsteniyor)
        {
            saatTikKaynagi.Stop();
            saatTikDuraklatildi = false;
            return;
        }

        if (AyarlarYoneticisi.AyarlarAcikMi)
        {
            if (saatTikKaynagi.isPlaying)
            {
                saatTikKaynagi.Pause();
                saatTikDuraklatildi = true;
            }
            return;
        }

        if (saatTikDuraklatildi)
        {
            saatTikKaynagi.UnPause();
            saatTikDuraklatildi = false;
            return;
        }

        if (!saatTikKaynagi.isPlaying)
            saatTikKaynagi.Play();
    }

    public static void SesSeviyesiniUygula(float master)
    {
        AudioListener.volume = master;
        if (ornek == null) return;

        if (ornek.muzikKaynagi != null)
            ornek.muzikKaynagi.volume = ornek.muzikSesSeviyesi * master;

        if (ornek.saatTikKaynagi != null)
            ornek.saatTikKaynagi.volume = ornek.saatTikSesSeviyesi * master;
    }

    private static AudioClip SaatTikClipBul()
    {
        AudioClip[] tumKlipler = Resources.FindObjectsOfTypeAll<AudioClip>();
        foreach (AudioClip clip in tumKlipler)
        {
            if (clip.name.Contains("spinopel") || clip.name.Contains("spiopel"))
                return clip;
        }
        return null;
    }
}
