using UnityEngine;

public class ActiveInactiveGO : MonoBehaviour
{
    public void SwitchStateGameObject(GameObject _gameObject)
    {
        _gameObject.SetActive(!_gameObject.activeSelf);
    }
}
