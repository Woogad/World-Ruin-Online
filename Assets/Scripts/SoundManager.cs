using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private const string PLAYER_PREFS_SOUND_EFFECT_VOLUME = "SoundEffectVolume";

    [SerializeField] private AudioClipRefsSO _audioClipRefsSO;

    private float _volume = 0.3f;

    private void Awake()
    {
        Instance = this;
        _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, 0.8f);
    }
    private void Start()
    {
        GunShopCounter.OnAnyBuyGun += GunShopCounterOnAnyBuyGun;
        GunShopCounter.OnAnyFailBuyGun += GunShopCounterOnAnyFailBuyGun;
        ItemShopCounter.OnAnyBuyItem += ItemShopCounterOnAnyBuyItem;
        ItemShopCounter.OnAnyFailBuyItem += ItemShopCounterOnAnyFailBuyItem;
        CoinTrader.OnAnyTradeCoin += CoinTraderOnAnyTradeCoin;
        CoinTrader.OnAnyFailTradeCoin += CoinTraderOnAnyFailTradeCoin;
        ClearCounter.OnAnyClearCounterPickObject += ClearCounterOnAnyPickObject;
        Player.OnAnyPlayerPickGun += PlayerOnPickGun;
    }

    private void ItemShopCounterOnAnyFailBuyItem(object sender, System.EventArgs e)
    {
        ItemShopCounter itemShopCounter = sender as ItemShopCounter;
        PlaySound(_audioClipRefsSO.FailInteract, itemShopCounter.transform.position);
    }

    private void CoinTraderOnAnyFailTradeCoin(object sender, System.EventArgs e)
    {
        CoinTrader coinTrader = sender as CoinTrader;
        PlaySound(_audioClipRefsSO.FailInteract, coinTrader.transform.position);
    }

    private void GunShopCounterOnAnyFailBuyGun(object sender, System.EventArgs e)
    {
        GunShopCounter gunShopCounter = sender as GunShopCounter;
        PlaySound(_audioClipRefsSO.FailInteract, gunShopCounter.transform.position);
    }

    private void CoinTraderOnAnyTradeCoin(object sender, System.EventArgs e)
    {
        CoinTrader coinTrader = sender as CoinTrader;
        PlaySound(_audioClipRefsSO.TradeCoin, coinTrader.transform.position);
    }

    private void ClearCounterOnAnyPickObject(object sender, System.EventArgs e)
    {
        Player player = sender as Player;
        PlaySound(_audioClipRefsSO.PickGun, player.transform.position);
    }

    private void PlayerOnPickGun(object sender, System.EventArgs e)
    {
        Player player = sender as Player;
        PlaySound(_audioClipRefsSO.PickGun, player.transform.position);
    }

    private void ItemShopCounterOnAnyBuyItem(object sender, System.EventArgs e)
    {
        ItemShopCounter itemShopCounter = sender as ItemShopCounter;
        PlaySound(_audioClipRefsSO.BuyObject, itemShopCounter.transform.position);
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
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * _volume);
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
    public void PlayPlayerDeadSound(Vector3 position, float volume)
    {
        PlaySound(_audioClipRefsSO.PlayerDead, position, volume);
    }

    public void PlayKillScoreSound(Vector3 position, float volume)
    {
        PlaySound(_audioClipRefsSO.KillScore, position, volume);
    }

    public void PlayPickGoldCoinSound(Vector3 position, float volume)
    {
        PlaySound(_audioClipRefsSO.PickGoldCoin, position, volume);
    }

    public void UpVolume()
    {
        _volume += 0.1f;
        _volume = Mathf.Round(_volume * 100f) / 100f; //? For fix float error added

        if (_volume > 1f)
        {
            _volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, _volume);
        PlayerPrefs.Save();
    }
    public void LowVolume()
    {
        _volume -= 0.1f;
        _volume = Mathf.Round(_volume * 100f) / 100f; //? For fix float error added

        if (_volume < 0f)
        {
            _volume = 1f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, _volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return this._volume;
    }
}
