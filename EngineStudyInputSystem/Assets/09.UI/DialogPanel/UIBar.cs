using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIBar : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement _panelElement;
    private DialogPanel _dialogPanel;
    private BulletUI _bulletUI;

    public string DialogText
    {
        get => _dialogPanel.Text;
        set => _dialogPanel.Text = value;
    }

    private bool _isOn = false;
    public bool IsOn
    {
        get => _isOn;
        set
        {
            _dialogPanel.SetOn(value);
            _isOn = value;
        }
    }

    private Camera _mainCam;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    private void Start()
    {
        _mainCam = Camera.main;
    }

    private void OnEnable()
    {
        _panelElement = _document.rootVisualElement.Q("panel");
        _dialogPanel = new DialogPanel(_panelElement, "", transform);

        VisualElement bulletContainer = _document.rootVisualElement.Q("bullet-counter");
        _bulletUI = new BulletUI(bulletContainer, 7);


    }

    public void LookToCamera()
    {
        _dialogPanel.LookRotation(_mainCam.transform.rotation);
    }

    public void SetBulletUI(int count)
    {
        print(count);
        _bulletUI.BulletCount = count;
    }

}
