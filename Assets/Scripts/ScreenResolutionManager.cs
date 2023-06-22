using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenResolutionManager : MonoBehaviour
{
    public static ScreenResolutionManager Instance { get; private set; }


    private const string PLAYER_PREFS_SCREEN_RESOLUTION_WIDTH = "ScreenResolutionWidth";
    private const string PLAYER_PREFS_SCREEN_RESOLUTION_HEIGHT = "ScreenResolutionHeight";
    private const string PLAYER_PREFS_SCREEN_RESOLUTION_IS_FULL_SCREEN = "IsFullScreen";
    private const int DEFAULT_SCREEN_WIDTH = 1920;
    private const int DEFAULT_SCREEN_HEIGHT = 1080;
    private const int DEFAULT_FULL_SCREEN = 0;

    private List<TMP_Dropdown.OptionData> _defultResolution = new List<TMP_Dropdown.OptionData>
    {
        new TMP_Dropdown.OptionData("1280x720"),
        new TMP_Dropdown.OptionData("1366x768"),
        new TMP_Dropdown.OptionData("1600x900"),
        new TMP_Dropdown.OptionData("1920x1080"),
        new TMP_Dropdown.OptionData("2560x1440"),
    };

    private void Awake()
    {
        Instance = this;
    }

    public bool IsFullScreen(int IsFullScreenNum)
    {
        if (IsFullScreenNum == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetScreenResolutionByPlayerPrefs()
    {
        Screen.SetResolution(GetScreenWidth(), GetScreenHeight(), IsFullScreen(GetScreenIsFullScreenNum()));
    }

    public List<TMP_Dropdown.OptionData> GetResolutionOptionDatas()
    {
        return this._defultResolution;
    }

    public int GetScreenWidth()
    {
        return PlayerPrefs.GetInt(PLAYER_PREFS_SCREEN_RESOLUTION_WIDTH, DEFAULT_SCREEN_WIDTH);
    }

    public int GetScreenHeight()
    {
        return PlayerPrefs.GetInt(PLAYER_PREFS_SCREEN_RESOLUTION_HEIGHT, DEFAULT_SCREEN_HEIGHT);
    }

    public int GetScreenIsFullScreenNum()
    {
        return PlayerPrefs.GetInt(PLAYER_PREFS_SCREEN_RESOLUTION_IS_FULL_SCREEN, DEFAULT_FULL_SCREEN);
    }

    public void SetScreenWidthValue(int width)
    {
        PlayerPrefs.SetInt(PLAYER_PREFS_SCREEN_RESOLUTION_WIDTH, width);
        PlayerPrefs.Save();
    }
    public void SetScreenHeightValue(int height)
    {
        PlayerPrefs.SetInt(PLAYER_PREFS_SCREEN_RESOLUTION_HEIGHT, height);
        PlayerPrefs.Save();
    }

    public void SetIsFullScreenValue(bool IsFullScreen)
    {
        if (IsFullScreen)
        {
            PlayerPrefs.SetInt(PLAYER_PREFS_SCREEN_RESOLUTION_IS_FULL_SCREEN, 1);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt(PLAYER_PREFS_SCREEN_RESOLUTION_IS_FULL_SCREEN, 0);
            PlayerPrefs.Save();
        }
    }
}
