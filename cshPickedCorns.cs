using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshPickedCorns : MonoBehaviour
{
    int i = 0;
    public GameObject[] corns;
    // Start is called before the first frame update

    public void CornSetactive(int corn)
    {
        if (corn % 3 == 0)
        {
            corns[i].SetActive(true);
            i++;
        }
    }
}
