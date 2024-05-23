using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private Transform target;
    public Vector3 offset;

    public void Initialize(Transform target, Vector3 offset)
    {
        this.target = target;
        this.offset = offset;
        slider.gameObject.SetActive(true);
    }

    public void UpdateHealthBar(float currentValue)
    {
        slider.value = currentValue;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = targetPosition;
        }
    }

}
