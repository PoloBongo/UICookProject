using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField] private GameObject notification;
    [SerializeField] private TMP_Text notificationText;
    [SerializeField] private AudioSource playSound;
    private Coroutine coroutine;

    private void OnEnable()
    {
        ContentCloset.OnShowNotificationFunc += StartNotification;
        RaycastContainer.OnShowNotificationFunc += StartNotification;
        DropdownSlots.OnShowNotificationFunc += StartNotification;
        SetupFormOptions.OnShowNotificationFunc += StartNotification;
        Cook.OnShowNotificationFunc += StartNotification;
        GetModalRecipe.OnShowNotificationFunc += StartNotification;
    }

    private void OnDisable()
    {
        ContentCloset.OnShowNotificationFunc -= StartNotification;
        RaycastContainer.OnShowNotificationFunc -= StartNotification;
        DropdownSlots.OnShowNotificationFunc -= StartNotification;
        SetupFormOptions.OnShowNotificationFunc -= StartNotification;
        Cook.OnShowNotificationFunc -= StartNotification;
        GetModalRecipe.OnShowNotificationFunc -= StartNotification;

    }

    private void StartNotification(string _message)
    {
        if (coroutine == null) coroutine = StartCoroutine(NotificationFunc(_message));
    }
    
    IEnumerator NotificationFunc(string _message)
    {
        Debug.Log(_message);
        notification.SetActive(true);
        playSound.Play();
        notificationText.text = _message;
        yield return new WaitForSeconds(2f);
        playSound.Stop();
        notification.SetActive(false);
        coroutine = null;
    }
}
