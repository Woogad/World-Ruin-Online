using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemObjectSO : ScriptableObject
{
    public Transform Prefab;
    public float ItemValue;
    public string ItemName;
}
