using System.Collections;
using UnityEngine;

public class DisplayMessage : MonoBehaviour
{
    public WarningPopup warningPopupPf; //warning popup for showing warning messages Prefab
    public GameObject warningPopupFather;
    public float Delay;
    public void Display(string Message)
    {
        WarningPopup warningPopup = Instantiate(warningPopupPf, warningPopupFather.transform);
        warningPopup.Show(Message);
    }

    public void DisplayAfterDelay(string Message)
    {
       StartCoroutine(displayAfterDelay(Message));
    }

    private IEnumerator displayAfterDelay(string Message)
    {
        yield return new WaitForSeconds(Delay);

        WarningPopup warningPopup = Instantiate(warningPopupPf, warningPopupFather.transform);
        warningPopup.Show(Message);
    }

}
