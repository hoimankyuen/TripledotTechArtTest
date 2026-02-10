using System.Collections.Generic;
using UnityEngine;

public class UIPopup : UIAnimatedTogglable
{
    [Header("Components")]
    [SerializeField] private UIPopupPage mainPage;
    [SerializeField] private List<UIPopupPage> subPages;

    private void Awake()
    {
        ShowMainPageImmediately();
    }

    public void ShowMainPageImmediately()
    {
        mainPage.Show(true, false, true);
        foreach (UIPopupPage subPage in subPages)
        {
            subPage.Show(false, true, true);
        }
    }
    
    public void ShowMainPage()
    {
        mainPage.Show(true, false);
        foreach (UIPopupPage subPage in subPages)
        {
            subPage.Show(false, true);
        }
    }

    public void ShowSubPage(int subPageIndex)
    {
        mainPage.Show(false, false);
        for (int i = 0; i < subPages.Count; i++)
        {
            subPages[i].Show(i == subPageIndex, true);
        }
    }
}
