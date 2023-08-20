using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayUI : MonoBehaviour
{
    [SerializeField] private List<PageItemUI> _pageList = new List<PageItemUI>();
    [SerializeField] private Button _nextPage;
    [SerializeField] private Button _previousPage;
    [SerializeField] private Button _closeBn;

    private int _currentPage;

    private void Awake()
    {
        _closeBn.onClick.AddListener(() =>
        {
            Hide();
        });
        _nextPage.onClick.AddListener(() =>
        {
            _currentPage += 1;
            if (_currentPage >= _pageList.Count)
            {
                _currentPage = 0;
            }
            IsOnPageIndex(_currentPage);
        });
        _previousPage.onClick.AddListener(() =>
        {
            _currentPage -= 1;
            if (_currentPage < 0)
            {
                _currentPage = _pageList.Count - 1;
            }
            IsOnPageIndex(_currentPage);
        });
    }

    private void Start()
    {
        _currentPage = 0;
        IsOnPageIndex(_currentPage);
        Hide();
    }

    private void IsOnPageIndex(int index)
    {
        foreach (var page in _pageList)
        {
            if (page.PageIndex == index)
            {
                page.Show();
            }
            else
            {
                page.Hide();
            }
        }
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
