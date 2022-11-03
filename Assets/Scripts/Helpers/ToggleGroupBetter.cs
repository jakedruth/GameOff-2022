using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;


namespace JDR.Utils
{
    public class ToggleGroupBetter : ToggleGroup
    {
        public int defaultToggleIndex;

        public delegate void ChangedEventHandler(Toggle newActive);

        public event ChangedEventHandler OnChange;
        [SerializeField]
        public ToggleIndexEvent getIndexOnChange;

        protected override void Start()
        {
            int childCount = gameObject.transform.childCount;
            defaultToggleIndex = Mathf.Clamp(defaultToggleIndex, 0, childCount - 1);

            for (int i = 0; i < childCount; i++)
            {
                Toggle toggle = gameObject.transform.GetChild(i).GetComponent<Toggle>();
                toggle.group = this;
                toggle.SetIsOnWithoutNotify(i == defaultToggleIndex);

                toggle.onValueChanged.AddListener(isSelected =>
                {
                    if (!isSelected)
                    {
                        return;
                    }

                    Toggle activeToggle = Active();
                    DoOnChange(activeToggle);
                    getIndexOnChange.Invoke(activeToggle.transform.GetSiblingIndex());
                });
            }
        }

        public Toggle Active()
        {
            return ActiveToggles().FirstOrDefault();
        }

        protected virtual void DoOnChange(Toggle newactive)
        {
            OnChange?.Invoke(newactive);
        }

        public void SetInteractableAll(bool isInteractable)
        {
            foreach (Transform transformToggle in gameObject.transform)
            {
                Toggle toggle = transformToggle.gameObject.GetComponent<Toggle>();
                toggle.interactable = isInteractable;
            }
        }

        [System.Serializable]
        public class ToggleIndexEvent : UnityEvent<int>
        {
        }
    }
}
