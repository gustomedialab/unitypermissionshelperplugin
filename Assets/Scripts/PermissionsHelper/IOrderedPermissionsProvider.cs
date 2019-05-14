using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PatchedReality.Permissions
{
    public interface IOrderedPermissionsProvider
    {
        List<PermissionsHelperPlugin.PermissionType> GetOrderedPermissions();
    }

}
