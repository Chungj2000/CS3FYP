using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Custom Data Type class for storing various Enums used in Combat.
 */
public class CombatDataType : MonoBehaviour {
    
    //Variations of ranged attacks.
    public enum RangeType {
        Precise,    //Ranged attack only targets at the ATK_RANGE perimeter. Melee formula.
        Area,       //Range attack targets between any Unit between 2 and ATK_RANGE.
        Splash,     //A ranged Area attack that inflicts a cross AoE on a target, inflicting half to Area of Effect units.
        Close       //A ranged Area attack that can also attack in melee range.
    };

    // Deals 50% extra damage to [ArmourType] Units > Neutral damage > Inflicts 50% less damage.
    public enum WeaponType {
        Pierce,      // Unarmoured > [Pierce]/Light > Heavy
        Slash,       // Light > [Slash]/Heavy > Unarmoured
        Blunt        // Heavy > [Blunt]/Unarmoured > Light
    }

    // Receives 50% less damage from [WeaponType] Units > Neutral damage > Takes 50% more damage.
    public enum ArmourType {
        Unarmoured,  // Slash > [Unarmoured]/Blunt > Pierce
        Light,       // Blunt > [Light]/Pierce > Slash
        Heavy        // Pierce > [Heavy]/Slash > Blunt
    }

}
