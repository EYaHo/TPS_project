using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CounterOnUI : MonoBehaviour
{
    public TextMeshProUGUI interactText;

    // Update is called once per frame
    void Update()
    {
        interactText.text = "Num Inactive Bullets: " + BulletPool.Instance.GetPoolSize().ToString();
    }
}
