using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiaizeVolumeSlider : MonoBehaviour
{
    public Slider slider;
    public SavedFloat vSaver;

    public void InitializeSlider()
    {
        //pr//int("slidervalue=" + slider.value + " savedV="+vSaver.GetValue());
        slider.value = vSaver.GetValue();
    }
}
