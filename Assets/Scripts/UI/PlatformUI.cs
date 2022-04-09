using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlatformUI : Singleton<PlatformUI>
{
    //public GameObject UIContainer;

    [Header("UI Elements")]
    public GameObject HUDContainer;
    public GameObject settingsContainer;

    void Awake()
    {
/* #if UNITY_ANDROID || UNITY_IOS
        GameObject useUI = Instantiate(mobileUIPrefab);
#endif
#if UNITY_STANDALONE
        GameObject useUI = Instantiate(standaloneUIPrefab);
#endif
        useUI.transform.SetParent(UIContainer.transform);
        useUI.transform.localScale = Vector3.one;
        useUI.transform.localPosition = Vector3.zero;
 */
        HideAll();
    }

    public void HideAll()
    {
        HUDContainer.SetActive(false);
        settingsContainer.SetActive(false);
    }
}
