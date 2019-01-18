using UnityEngine;
using UnityEngine.Events;

namespace Gamekit2D
{
    public class InteractOnButton2D : InteractOnTrigger2D
    {
        RoleContr roleController;

        public UnityEvent OnButtonPress;

        bool m_CanExecuteButtons;

        protected override void ExecuteOnEnter(Collider2D other)
        {
            m_CanExecuteButtons = true;
            OnEnter.Invoke ();
        }

        protected override void ExecuteOnExit(Collider2D other)
        {
            m_CanExecuteButtons = false;
            OnExit.Invoke ();
        }

        //使用PlayerInput的时候记得用回这个
        void Update_Origin()
        {
            if (m_CanExecuteButtons)
            {
                if (OnButtonPress.GetPersistentEventCount() > 0 && PlayerInput.Instance.Interact.Down)
                    OnButtonPress.Invoke();
            }
        }

        void Update()
        {
            //OnButtonPress.GetPersistentEventCount()  --->  获取该事件监听者个数
            if (m_CanExecuteButtons)
            {
                if (OnButtonPress.GetPersistentEventCount() > 0 && roleController.Interact())
                {
                    OnButtonPress.Invoke();
                }
            }
        }


        // -------------------------------------------------------------------------------------------------------------------
        private void Start()
        {
            roleController = FindObjectOfType<RoleContr>();
        }
    }
}