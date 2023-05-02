using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO _audioClipRefsSO;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GunShopCounter.OnAnyBuyGun += GunShopCounterOnAnyBuyGun;
        ItemShopCounter.OnAnyBuyItem += ItemShopCounterOnAnyBuyItem;
        ClearCounter.OnAnyClearCounterPickObject += ClearCounterOnAnyPickObject;
        Player.Instance.OnPickGun += PlayerOnPickGun;
    }

    private void ClearCounterOnAnyPickObject(object sender, System.EventArgs e)
    {
        PlaySound(_audioClipRefsSO.PickGun, Player.Instance.transform.position);
    }

    private void PlayerOnPickGun(object sender, System.EventArgs e)
    {
        PlaySound(_audioClipRefsSO.PickGun, Player.Instance.transform.position);
    }

    private void ItemShopCounterOnAnyBuyItem(object sender, System.EventArgs e)
    {
        ItemShopCounter gunShopCounter = sender as ItemShopCounter;
        PlaySound(_audioClipRefsSO.BuyObject, gunShopCounter.transform.position);
    }

    private void GunShopCounterOnAnyBuyGun(object sender, System.EventArgs e)
    {
        GunShopCounter gunShopCounter = sender as GunShopCounter;
        PlaySound(_audioClipRefsSO.BuyObject, gunShopCounter.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public void PlayGunShootSound(Vector3 position, float volume)
    {
        PlaySound(_audioClipRefsSO.GunShoot, position, volume);
    }

    public void PlayEmptyShoot(Vector3 position, float volume)
    {
        PlaySound(_audioClipRefsSO.EmptyShoot, position, volume);
    }

    public void PlayReloadSound(Vector3 position, float volume)
    {
        PlaySound(_audioClipRefsSO.Reload, position, volume);
    }

    public void PlayFootstepSound(Vector3 position, float volume)
    {
        PlaySound(_audioClipRefsSO.FootStep, position, volume);
    }
}
