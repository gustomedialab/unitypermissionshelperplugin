using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace PatchedReality.Permissions
{
    //todo: should be made into a singleton...
    public class PermissionsHelperPlugin : Singleton<PermissionsHelperPlugin>
    {
        public delegate void PermissionStatusUpdatedDelegate(PermissionType permission, bool success);
        public static PermissionStatusUpdatedDelegate OnPermissionStatusUpdated;

        public enum PermissionType
        {
            PRCameraPermissions = 0,
            PRMicrophonePermissions = 1,
            PRLocationWhileUsingPermissions = 2,
            PRSpeechRecognitionPermissions = 3,
            PRPermissionTypeUnknown = 255
        }
        public enum PermissionStatus
        {
            PRPermissionStatusUnknown = 0,
            PRPermissionStatusAuthorized = 1,
            PRPermissionStatusDenied = 2,
            PRPermissionStatusRestricted = 3,
            PRPermissionStatusUnknownPermission = 255
        }

        protected PermissionsHelperPlugin(){}


        public void RequestPermission(PermissionType permission)
        {
            _requestPermission((int)permission, this.gameObject.name,"PermissionRequestSuccess","PermissionRequestFailure");
        }

        public void OpenSettings()
        {
            _openSettings();
        }

        public PermissionStatus GetPermissionStatus(PermissionType permission)
        {
            return (PermissionStatus)_getPermissionStatus((int)permission);
        }

        void PermissionRequestSuccess(string permissionType) 
        {
            int permAsInt;
            PermissionType type = PermissionType.PRPermissionTypeUnknown;
            if(int.TryParse(permissionType,out permAsInt))
            {
                type = (PermissionType)permAsInt;
            }
            
            Debug.Log("Got success callback from native with: " + type.ToString());
            OnPermissionStatusUpdated?.Invoke(type,true);
        }

        void PermissionRequestFailure(string permissionType)
        {
            int permAsInt;
            PermissionType type = PermissionType.PRPermissionTypeUnknown;
            if(int.TryParse(permissionType,out permAsInt))
            {
                type = (PermissionType)permAsInt;
            }
            Debug.Log("Got failure callback from native with: " + type.ToString());
            OnPermissionStatusUpdated?.Invoke(type,false);
        }

#if UNITY_IOS && !UNITY_EDITOR
        [DllImport ("__Internal")]
        private static extern void _requestPermission (int permissionType, string gameObject, string successCallback, string failureCallback);

        [DllImport ("__Internal")]
        private static extern int _getPermissionStatus(int permissionType);
        
        [DllImport ("__Internal")]
        private static extern void _openSettings();
#else
        private static void _requestPermission(int permissionType, string gameObject, string successCallback, string failureCallback)
        {
            GameObject go = GameObject.Find(gameObject);
            if (go != null)
            {
                go.SendMessage(failureCallback, permissionType.ToString());
            }
        }

        private static int _getPermissionStatus(int permissionType)
        {
            return (int)PermissionStatus.PRPermissionStatusUnknownPermission;
        }


        private static void _openSettings()
        {
            UnityEngine.Debug.LogWarning("Open Settings  - platform unsupported");
        }
#endif

    }
}
