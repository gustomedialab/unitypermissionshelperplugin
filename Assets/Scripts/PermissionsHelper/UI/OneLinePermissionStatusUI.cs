using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PatchedReality.Permissions.UI
{
    using PermissionType = PermissionsHelperPlugin.PermissionType;
    using PermissionStatus = PermissionsHelperPlugin.PermissionStatus;
    public class OneLinePermissionStatusUI : MonoBehaviour
    {
        [System.Serializable]
        public class IconChoice
        {
            public List<PermissionStatus> ForStatuses;
            public Sprite Visual;

            public Color Tint;
        }

        [SerializeField]protected List<IconChoice> IconsForStatus;

        [SerializeField]protected PermissionType MyPermission;
        [SerializeField] protected UnityEngine.UI.Image Icon;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            //update icon appropriately.
            PermissionsHelperPlugin.OnPermissionStatusUpdated+=HandlePermissionChanged;
            UpdateIcon();
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {
            PermissionsHelperPlugin.OnPermissionStatusUpdated-=HandlePermissionChanged;
        }


        void HandlePermissionChanged(PermissionType permission, bool success)
        {
            if(MyPermission.Equals(permission))
            {
                UpdateIcon();
            }
        }

        void UpdateIcon()
        {
            //get status for our perm, and update icon accordingly.
            PermissionStatus status = PermissionsHelperPlugin.Instance.GetPermissionStatus(MyPermission);

            Debug.Log(
                string.Format("Got status {0} for permission {1}", 
                status.ToString(), MyPermission.ToString()));
            var choice = ChoiceForStatus(status);
            if(choice == null)
            {
                //just hide the icon.
                Icon.gameObject.SetActive(false);
            }
            else
            {
                Icon.gameObject.SetActive(true);
                Icon.sprite = choice.Visual;
                Icon.color = choice.Tint;
            }

        }

        IconChoice ChoiceForStatus(PermissionStatus status)
        {
            foreach(IconChoice choice in IconsForStatus)
            {
                if(choice.ForStatuses.Contains(status))
                {
                    return choice;
                }
            }

            return null;
        }

    }
}
