using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace YF
{
    public class BasePanel : MonoBehaviour
    {
        protected bool isRemove = false;
        protected new string name;
        public virtual void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public virtual void OpenPanel(string name)
        {
            this.name = name;
            SetActive(true);
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0.0f;
            DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1, 2);
        }

        public virtual void ClosePanel()
        {
            isRemove = true;
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0, 1).OnComplete(() =>
            {
                SetActive(false);
                Destroy(gameObject);
                if (UIManager.Instance.panelDict.ContainsKey(name))
                {
                    UIManager.Instance.panelDict.Remove(name);
                }
            });
        }
    }
}


