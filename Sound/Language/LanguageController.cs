using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    public static LanguageController instance;

    public event EventHandler LanguageChanged;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeLanguageToEnglish()
    {
        ChangeLanguage(0);
    }
    public void ChangeLanguageToDutch()
    {
        ChangeLanguage(1);
    }
    public void ChangeLanguageToSpanish()
    {
        ChangeLanguage(2);
    }

    void ChangeLanguage(int language)
    {
        //Changes the language index
        PlayerPrefs.SetInt("LanguageIndex", language);
        //Tells all subscribed functions that the language was changed
        LanguageChanged?.Invoke(this, new EventArgs());
    }
}
