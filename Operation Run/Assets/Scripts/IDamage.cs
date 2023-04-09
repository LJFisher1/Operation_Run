using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void TakeDamage(int dmg);

    void TakeDamage(int dmg, GameObject attacker) { return; }
    bool IsAlive { get => true; }
}