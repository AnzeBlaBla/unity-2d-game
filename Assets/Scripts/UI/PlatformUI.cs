using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlatformUI : MonoBehaviour
{
    public GameObject UIContainer;

    [Header("Platform UIs")]
    public GameObject mobileUIPrefab;
    public GameObject standaloneUIPrefab;

    // Start is called before the first frame update
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
    }
}
