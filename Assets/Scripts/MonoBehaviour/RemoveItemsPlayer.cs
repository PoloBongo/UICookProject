using System;
using UnityEngine;

public class RemoveItemsPlayer : MonoBehaviour
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (leftHand.transform.childCount > 0) Destroy(leftHand.transform.GetChild(0).gameObject);
            if (rightHand.transform.childCount > 0) Destroy(rightHand.transform.GetChild(0).gameObject);
        }
    }
}
