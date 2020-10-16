using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdds : MonoBehaviour
{

    string GooglePlay_ID = "3572023";
    public bool testMode = true;

    private void Start()
    {
        Advertisement.Initialize(GooglePlay_ID, testMode);
    }

    public void DisplayInterstitialAD()
    {
        Advertisement.Show();
    }

}
