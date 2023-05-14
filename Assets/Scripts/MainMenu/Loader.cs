using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene,
    }

    private static Scene TargetScene;

    public static void Load(Scene targetScene)
    {
        Loader.TargetScene = targetScene;
        SceneManager.LoadSceneAsync(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {

        SceneManager.LoadSceneAsync(Loader.TargetScene.ToString());
    }
}
