using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.UI;

namespace PutAndHelp.DebugUI
{
    public class FingerBehavior : MonoBehaviour
    {
        [SerializeField] Image _fingerImage;

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                _fingerImage.gameObject.SetActive(true);
                _fingerImage.transform.position = Input.GetTouch(0).position;
                
            }else
                _fingerImage.gameObject.SetActive(false);
            
        }

        // public void MoveFinger(LeanFinger finger)
        // {
        //     _fingerImage.transform.position = finger.ScreenPosition;
        // }

    }
}