using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PatchedReality.Permissions.UI
{
    using PermissionType = PermissionsHelperPlugin.PermissionType;
    using PermissionStatus = PermissionsHelperPlugin.PermissionStatus;
    using CollectiveState = CollectivePermissionsStatus.CollectiveState;
    //manage overall screen state for the permissions UI - mainly messaging the player appropriately.
    public class AllAtOncePermissionsUI : MonoBehaviour, IOrderedPermissionsProvider
    {
        //TODO: this list appears on a couple components. think of a way 
        //to just set it in one place..
        [Tooltip("In order list of permissions we will ask for in sequence.")]
        [SerializeField] protected List<PermissionType> PermissionsInOrder;

        [SerializeField] protected TMP_Text HeaderText;
        [SerializeField] protected TMP_Text MessageText;

        [SerializeField] protected AllAtOncePermissionsButtonHandler PermissionsHandler;

        [SerializeField] protected string NeedAuthHeader = "Welcome Stranger";
        [SerializeField] protected string NeedAuthMessage = "We need some permissions from you before you can use the app.";

        [SerializeField] protected string NeedSettingsHeader = "One More Step";
        [SerializeField] protected string NeedSettingsMessage = "In order to complete giving permissions you need to go to settings and allow: {0}";


        [SerializeField] protected string AllDoneHeader = "Thanks!";
        [SerializeField] protected string AllDoneMessage = "Great! You're ready to go. Enjoy!";

        public List<PermissionType> GetOrderedPermissions()
        {
            return PermissionsInOrder;
        }
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            PermissionsHelperPlugin.OnPermissionStatusUpdated+=HandlePermissionChanged;
            PermissionsHandler.Initialize(this);
            UpdateMessages();
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {
            PermissionsHelperPlugin.OnPermissionStatusUpdated-=HandlePermissionChanged;
        }

        void HandlePermissionChanged(PermissionType type, bool result)
        {
            UpdateMessages();
        }

        void UpdateMessages()
        {
            var permsCollection = new CollectivePermissionsStatus(PermissionsInOrder);
            CollectiveState state = permsCollection.GetCurrentState();
            switch(state)
            {
                case CollectiveState.AllAuthorized:
                {
                    HeaderText.text = AllDoneHeader;
                    MessageText.text = AllDoneMessage;
                    break;
                }
                case CollectiveState.AllUnknown:
                case CollectiveState.SomeUnknown:
                {
                    HeaderText.text = NeedAuthHeader;
                    MessageText.text = NeedAuthMessage;
                    break;
                }
                case CollectiveState.AllAsked:
                {
                    HeaderText.text = NeedSettingsHeader;
                    MessageText.text = NeedSettingsMessage;
                    break;
                }
            }
        }


    }
}
