using UnityEngine;
using UnityEngine.UI;

public class ButtonClickPickable : MonoBehaviour
{
    [SerializeField] private GameObject canvasUI;
    public delegate void OnClick(string buttonName);
    public static event OnClick OnButtonClick;
    void Start () {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick(){
        OnButtonClick?.Invoke(gameObject.name);
        canvasUI.SetActive(false);
    }
}
