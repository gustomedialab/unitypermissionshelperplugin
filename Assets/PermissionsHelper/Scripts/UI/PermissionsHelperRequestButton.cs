using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PatchedReality.Permissions.UI
{
    using PermissionType = PermissionsHelperPlugin.PermissionType;
    using PermissionStatus = PermissionsHelperPlugin.PermissionStatus;

    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class PermissionsHelperRequestButton : MonoBehaviour
    {

        [SerializeField] protected PermissionType Permission;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            Button.onClick.AddListener(HandleButtonClick);
        }

        void OnEnable()
        {
            PermissionsHelperPlugin.OnPermissionStatusUpdated += HandlePermissionRequestStatusChange;
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {
            PermissionsHelperPlugin.OnPermissionStatusUpdated -= HandlePermissionRequestStatusChange;
        }

        void HandlePermissionRequestStatusChange(PermissionType permission, bool result)
        {
            if (Permission.Equals(permission))
            {
                UpdateButtonState(result);
            }
        }

        void UpdateButtonState(bool enabled)
        {
            this.Button.interactable = !enabled;
        }

        void HandleButtonClick()
        {
            PermissionsHelperPlugin.Instance.RequestPermission(this.Permission);
        }

        UnityEngine.UI.Button Button
        {
            get
            {
                if (button == null)
                {
                    button = GetComponent<UnityEngine.UI.Button>();
                }
                return button;
            }
        }
        UnityEngine.UI.Button button;
    }

}
