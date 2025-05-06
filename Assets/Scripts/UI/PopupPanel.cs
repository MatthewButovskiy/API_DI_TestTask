using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class PopupPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text    _title;
    [SerializeField] private TMP_Text    _body;
    [SerializeField] private CanvasGroup _cg;
    [SerializeField] private Button _closeButton;
    
    void Awake()
    {
        gameObject.SetActive(false);
        _closeButton.onClick.AddListener(Hide);
    }

    public void Show(string title, string body)
    {
        _title.text = title;
        _body .text = body;
        gameObject.SetActive(true);
        _cg.alpha   = 0f;
        _cg.DOFade(1f, 0.15f);
    }

    public void Hide()
    {
        _cg.DOFade(0f, 0.15f)
            .OnComplete(() => gameObject.SetActive(false));
    }
}