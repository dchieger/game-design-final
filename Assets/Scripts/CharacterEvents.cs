using System;
using UnityEngine.Events;

public class CharacterEvents
{
    public static UnityAction<Damageable, int> characterHit = delegate (Damageable characterHit, int damageReceived) { };
    public static UnityAction<Damageable, int> characterHealed = delegate (Damageable characterHealed, int healthReceived) { };
}
