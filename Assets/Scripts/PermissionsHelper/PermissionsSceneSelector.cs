﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PatchedReality.Permissions
{
    using CollectiveState = CollectivePermissionsStatus.CollectiveState;
    using PermissionType = PermissionsHelperPlugin.PermissionType;
    
    /*
        Put one of these in your "loading" scene. It will decide if it needs to go to the
        permission granting scene or your real app scene. Right now, it uses LoadScene
        with no loading between - on the assumption that you arrange the layout of your loading
        scene, your permissions scene, and your app start / title scene to match...
     */
    public class PermissionsSceneSelector : MonoBehaviour
    {
        [SerializeField] protected string PermissionsScene;
        [SerializeField] protected string MainScene;
        
        [SerializeField] protected List< PermissionType> RequiredPermissions;

        // Start is called before the first frame update
        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            PermissionsHelperPlugin.Instance.SetRequiredPermissions(RequiredPermissions);
            var status = PermissionsHelperPlugin.Instance.GetCollectiveState();
            PatchedReality.Permissions.UI.AllAtOncePermissionsButtonHandler.OnUserReadyToContinue += ToMainScene;
            
            if(status.Equals(CollectiveState.AllAuthorized))
            {
                ToMainScene();
            }
            else 
            {
                ToPermissionsScene();
            }
        }

        void ToPermissionsScene()
        {
            //listen for our ok from the single button class.
            //TODO: remove this assumption - this scene selector would be useful
            //even in either permission scene styles.
            SceneManager.LoadScene(PermissionsScene,LoadSceneMode.Single);

        }
        void ToMainScene()
        {
            PatchedReality.Permissions.UI.AllAtOncePermissionsButtonHandler.OnUserReadyToContinue -= ToMainScene;
            SceneManager.LoadScene(MainScene,LoadSceneMode.Single);
        }

    }

}

