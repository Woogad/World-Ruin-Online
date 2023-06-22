using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OptionsUI : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI _musicVolumeText;
    [SerializeField] private TextMeshProUGUI _soundVolumeText;
    [SerializeField] private Button _musicUpperBn;
    [SerializeField] private Button _musicLowerBn;
    [SerializeField] private Button _soundUpperBn;
    [SerializeField] private Button _soundLowerBn;
    [SerializeField] private TMP_Dropdown _resolotionDropdown;
    [SerializeField] private Toggle _isFullScreenToggle;
    [SerializeField] private Button _closeBn;
    [SerializeField] private Button _controllerBn;


    private void Awake()
    {
        _closeBn.onClick.AddListener(() =>
        {
            Hide();
        });
        _musicUpperBn.onClick.AddListener(() =>
        {
            if (MusicManager.Instance == null)
            {
                MusicManager musicManager = GameObject.FindAnyObjectByType<MusicManager>();
                musicManager.UpVolume();
                UpdateVisual();
            }
            else
            {
                MusicManager.Instance.UpVolume();
                UpdateVisual();
            }
        });
        _musicLowerBn.onClick.AddListener(() =>
        {
            if (MusicManager.Instance == null)
            {
                MusicManager musicManager = GameObject.FindAnyObjectByType<MusicManager>();
                musicManager.LowVolume();
                UpdateVisual();
            }
            else
            {
                MusicManager.Instance.LowVolume();
                UpdateVisual();
            }
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
        _controllerBn.onClick.AddListener(() =>
        {
            ShowControllerUI.Instance.Show();
        });

        _resolotionDropdown.onValueChanged.AddListener(DropdownOnValueChanged);
        _isFullScreenToggle.onValueChanged.AddListener((isFullScreen) =>
        {
            ScreenResolutionManager.Instance.SetIsFullScreenValue(isFullScreen);
            ScreenResolutionManager.Instance.SetScreenResolutionByPlayerPrefs();
        });

    }
    private void Start()
    {
        AddDropdownValue(GetDropdownValueList());
        UpdateVisual();
        Hide();
    }

    private void DropdownOnValueChanged(int tMP_DropdownValue)
    {
        if (tMP_DropdownValue == 0) return;
        string resolution = GetDropdownValueList()[tMP_DropdownValue].text;
        string[] words = resolution.Split("x");
        int i = 0;
        foreach (string chareacter in words)
        {
            if (i == 0)
            {
                int value = int.Parse(chareacter);
                ScreenResolutionManager.Instance.SetScreenWidthValue(value);
                i++;
            }
            else
            {
                int value = int.Parse(chareacter);
                ScreenResolutionManager.Instance.SetScreenHeightValue(value);
            }
        }
        ScreenResolutionManager.Instance.SetScreenResolutionByPlayerPrefs();
        AddDropdownValue(GetDropdownValueList());
    }

    private void UpdateVisual()
    {
        if (MusicManager.Instance == null)
        {
            MusicManager musicManager = GameObject.FindAnyObjectByType<MusicManager>();
            _musicVolumeText.text = Mathf.Round(musicManager.GetVolume() * 10f).ToString();
        }
        else
        {
            _musicVolumeText.text = Mathf.Round(MusicManager.Instance.GetVolume() * 10f).ToString();
        }
        _soundVolumeText.text = Mathf.Round(SoundManager.Instance.GetVolume() * 10f).ToString();
        _isFullScreenToggle.isOn = ScreenResolutionManager.Instance.IsFullScreen(ScreenResolutionManager.Instance.GetScreenIsFullScreenNum());
    }

    private void AddDropdownValue(List<TMP_Dropdown.OptionData> tmp_DropdownList)
    {
        _resolotionDropdown.ClearOptions();
        _resolotionDropdown.AddOptions(tmp_DropdownList);
    }

    private List<TMP_Dropdown.OptionData> GetDropdownValueList()
    {
        int playerPrefsWidth = ScreenResolutionManager.Instance.GetScreenWidth();
        int playerPrefsHeight = ScreenResolutionManager.Instance.GetScreenHeight();

        string selectedResolution = $"<<{playerPrefsWidth}x{playerPrefsHeight}>>";

        List<TMP_Dropdown.OptionData> resolutionValueList = new List<TMP_Dropdown.OptionData>
        {
            new TMP_Dropdown.OptionData(selectedResolution)
        };

        resolutionValueList.AddRange(ScreenResolutionManager.Instance.GetResolutionOptionDatas());

        return resolutionValueList;
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
