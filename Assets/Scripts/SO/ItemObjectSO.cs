using UnityEngine;

[CreateAssetMenu()]
public class ItemObjectSO : ScriptableObject
{
    public Transform Prefab;
    public ItemType.ItemTypeList Type;
    public float ItemValue;
    public int Price;
    public string ItemName;
}
