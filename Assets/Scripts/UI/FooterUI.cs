using System;
using TMPro;
using UnityEngine;

public class FooterUI : MonoBehaviour
{
    [SerializeField] TMP_Text tmpText;

    void Start()
    {
        SetFooter();
    }

    private void OnValidate()
    {
        SetFooter();
    }

    void SetFooter()
    {
        string version = Application.version;
        string companyName = Application.companyName;
        int currentYear = DateTime.Now.Year;
        tmpText.text = $"{version} | {companyName} ©{currentYear}";
    }
}
