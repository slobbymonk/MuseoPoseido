using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RefreshTextHover : MonoBehaviour
{
    public TMP_Text _text;
    private string _lines;
    public float _textSpeed;

    public void LoadText()
    {
        _lines = _text.text;
        _text.text = "";

        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        foreach (var c in _lines.ToCharArray())
        {
            _text.text += c;
            yield return new WaitForSeconds(_textSpeed);
        }
    }
}
