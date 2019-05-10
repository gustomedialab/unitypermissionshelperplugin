using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace PatchedReality.Permissions
{
    public class PermissionsHelper
    {
        public enum PermissionType
        {
            PRCameraPermissions = 0,
            PRMicrophonePermissions = 1,
            PRLocationWhileUsingPermissions = 2,
            PRSpeechRecognitionPermissions = 3
        }
        public enum PermissionStatus
        {
            PRPermissionStatusUnknown = 0,
            PRPermissionStatusAuthorized = 1,
            PRPermissionStatusDenied = 2,
            PRPermissionStatusRestricted = 3,
            PRPermissionStatusUnknownPermission = 255
        }


        public static void RequestPermission(PermissionType permission, GameObject go, string successMethod, string failureMethod)
        {
            if (go == null)
            {
                Debug.LogError("Cannot request permissions without a callback object!");
                return;
            }

            _requestPermission((int)permission, go.name, successMethod, failureMethod);
        }

        public static void OpenSettings()
        {
            _openSettings();
        }

        public static PermissionStatus GetPermissionStatus(PermissionType permission)
        {
            return (PermissionStatus)_getPermissionStatus((int)permission);
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
