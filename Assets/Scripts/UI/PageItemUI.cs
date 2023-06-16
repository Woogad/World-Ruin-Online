using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageItemUI : MonoBehaviour
{
    public int PageIndex;

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
