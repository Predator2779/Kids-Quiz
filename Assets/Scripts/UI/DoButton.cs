using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class DoButton : Button
    {
        [SerializeField] private Vector3 _targetScale = Vector3.one * 1.5f; //
        [SerializeField] [Range(0, 1)] private float _duration = 0.1f; //
        
        private Vector3 _startScale;

        protected override void Awake()
        {
            base.Awake();
            _startScale = transform.localScale;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            transform.DOScale(_targetScale, _duration);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            transform.DOScale(_targetScale, _duration);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            transform.DOScale(_startScale, _duration);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            DOTween.KillAll();
            // через CancellationToken
        }
    }
}