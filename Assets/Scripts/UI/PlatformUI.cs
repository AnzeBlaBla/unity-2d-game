using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlatformUI : Singleton<PlatformUI>
{
    public GameObject UIContainer;

    [Header("Platform UIs")]
    public GameObject mobileUIPrefab;
    public GameObject standaloneUIPrefab;

    void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        GameObject useUI = Instantiate(mobileUIPrefab);
#endif
#if UNITY_STANDALONE
        GameObject useUI = Instantiate(standaloneUIPrefab);
#endif
        useUI.transform.SetParent(UIContainer.transform);
        useUI.transform.localScale = Vector3.one;
        useUI.transform.localPosition = Vector3.zero;

        HideUI();
    }

    public void ShowUI()
    {
        UIContainer.SetActive(true);
    }

    public void HideUI()
    {
        UIContainer.SetActive(false);
    }

}
