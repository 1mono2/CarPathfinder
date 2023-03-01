using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugTextOnOff : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _textOn;
   [SerializeField] private TextMeshProUGUI _textOff;
   
   public void EnableText(bool isEnable)
   {
      _textOn.gameObject.SetActive(isEnable);
      _textOff.gameObject.SetActive(!isEnable);
   }
}
