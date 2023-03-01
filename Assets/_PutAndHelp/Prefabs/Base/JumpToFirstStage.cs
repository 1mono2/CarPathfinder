using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpToFirstStage : MonoBehaviour
{
   public void JumpTo1stStage()
        {
            SceneManager.LoadScene(0);
        }
}
