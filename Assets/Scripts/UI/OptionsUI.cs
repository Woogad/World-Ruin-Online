using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsUI : MonoBehaviour
{

    public static OptionsUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _musicVolumeText;
    [SerializeField] private TextMeshProUGUI _soundVolumeText;
    [SerializeField] private Button _musicUpperBn;
    [SerializeField] private Button _musicLowerBn;
    [SerializeField] private Button _soundUpperBn;
    [SerializeField] private Button _soundLowerBn;
    [SerializeField] private Button _closeBn;

    private void Awake()
    {
        _closeBn.onClick.AddListener(() =>
        {
            Hide();
        });
        _musicUpperBn.onClick.AddListener(() =>
        {
            MusicManager.Instance.UpVolume();
            UpdateVisual();
        });
        _musicLowerBn.onClick.AddListener(() =>
        {
            MusicManager.Instance.LowVolume();
            UpdateVisual();
        });
        _soundUpperBn.onClick.AddListener(() =>
        {
            SoundManager.Instance.UpVolume();
            UpdateVisual();
        });
        _soundLowerBn.onClick.AddListener(() =>
        {
            SoundManager.Instance.LowVolume();
            UpdateVisual();
        });
    }
    private void Start()
    {
        Instance = this;
        UpdateVisual();
        Hide();
    }

    private void UpdateVisual()
    {
        _musicVolumeText.text = Mathf.Round(MusicManager.Instance.GetVolume() * 10f).ToString();
        _soundVolumeText.text = Mathf.Round(SoundManager.Instance.GetVolume() * 10f).ToString();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
