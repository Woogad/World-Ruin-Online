using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefabMainMenu : MonoBehaviour
{
    [SerializeField] private PlayerPrefabsVisualListSO _playerPrefabsVisualListSO;

    private const string PLAYER_VISUAL = "PlayerVisual";

    private enum Pose
    {
        IsCrouch,
        IsGuard,
        IsGuard2,
    }
    private Pose _pose;

    private void Start()
    {
        _pose = (Pose)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Pose)).Length);
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        int playerPrefabIndex = PlayerPrefs.GetInt(PlayerPrefabManager.PLAYER_PREFS_PLAYER_PREFAB_INDEX, 0);
        Transform prefabTransform = Instantiate(_playerPrefabsVisualListSO.PlayerPrefabVisaulList[playerPrefabIndex], gameObject.transform);
        Transform playerVisual = prefabTransform.Find(PLAYER_VISUAL);
        Animator animator = playerVisual.GetComponent<Animator>();
        animator.SetBool(_pose.ToString(), true);

    }
}
