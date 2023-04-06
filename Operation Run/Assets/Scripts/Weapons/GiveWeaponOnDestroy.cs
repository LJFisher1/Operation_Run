using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveWeaponOnDestroy : MonoBehaviour
{
    [SerializeField] Weapon weapon;

    private void OnDestroy()
    {
        //have to check if player exists or bizzarly, you get a bunch of errors when you stop the project. I assume this has something to do with the destory order.
        //In general doing this on destroy probably isnt the best but it works well for now & is super easy to put on stuff.
        if (GameManager.instance.player != null) GameManager.instance.playerController.ChangeWeapon(weapon);
        else return;
    }
}
