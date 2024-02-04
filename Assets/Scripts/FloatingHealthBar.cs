using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateHealth(float health, float maxHealth)
    {
        slider.value = health / maxHealth;
    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
