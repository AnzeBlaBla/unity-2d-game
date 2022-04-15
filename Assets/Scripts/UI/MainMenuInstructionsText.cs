using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuInstructionsText : MonoBehaviour
{
    public TMP_Text instructionsText;
    public string standaloneText = "Left click to shoot, right click to move. You'll figure the rest out.";
    public string mobileText = "Tap to move, use the buttons to shoot. You'll figure the rest out.";
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_STANDALONE
        instructionsText.text = standaloneText;
#endif
#if UNITY_ANDROID || UNITY_IOS
        instructionsText.text = mobileText;
#endif
    }

}
