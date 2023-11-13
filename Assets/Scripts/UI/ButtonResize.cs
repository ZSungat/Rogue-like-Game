using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonResize : MonoBehaviour
{
    public Button button;
    private Animator animator;
    void Start()
    {
        // animator = GetComponent<Animator>();
        // animator.ResetTrigger("Selected");
        // animator.ResetTrigger("Normal");
        // animator.SetTrigger("Selected");
    }
    // public void OnSelect(BaseEventData eventData)
    // {
    //     Debug.Log(this.gameObject.name + " was selected");
    //     animator.SetTrigger("Selected");
    // }

}
