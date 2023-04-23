using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Script used to handle damage calculations using CombatData.
 */
public class CombatDataHandler : MonoBehaviour {
    
    public static CombatDataHandler INSTANCE {get; private set;}
    private const float effectiveMultiplier = 1.5f;
    private const float ineffectiveMultiplier = .5f;
    private UnitHandler attackingUnit;
    private UnitHandler defendingUnit;

    private void Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("CombatDataHandler instance created.");
        } else {
            Debug.LogError("More than one CombatDataHandler instance created.");
            Destroy(this);
            return;
        }
    }

    //Calculates rounded damage using Weapon and Armour Type, as well as the parameters of the targetted and attacking units.
    //Identifies whether an attack is effective, neutral, or ineffective based on CombatData cases.
    public int CombatDataCalculateDamage() {

        float damageValue = 0;

        CombatDataType.WeaponType weaponType = attackingUnit.GetWeaponType();
        Debug.Log("Attacking Unit Weapon Type: " + weaponType);
        CombatDataType.ArmourType armourType = defendingUnit.GetArmourType();
        Debug.Log("Defending Unit Armour Type: " + armourType);

        //Pierce Weapons

        if(weaponType == CombatDataType.WeaponType.Pierce) {

            if(armourType == CombatDataType.ArmourType.Unarmoured) {

                damageValue = (attackingUnit.GetParamATK() * effectiveMultiplier) - defendingUnit.GetParamDEF();
                Debug.Log("Pierce VS Unarmoured is effective. Dealing " + damageValue + " damage.");
                return Mathf.RoundToInt(damageValue);

            } else if (armourType == CombatDataType.ArmourType.Light) {

                damageValue = attackingUnit.GetParamATK() - defendingUnit.GetParamDEF();
                Debug.Log("Pierce VS Light is neutral. Dealing " + damageValue + " damage.");
                return Mathf.RoundToInt(damageValue);

            } else if (armourType == CombatDataType.ArmourType.Heavy) {

                damageValue = (attackingUnit.GetParamATK() * ineffectiveMultiplier) - defendingUnit.GetParamDEF();
                Debug.Log("Pierce VS Heavy is ineffective. Dealing " + damageValue + " damage.");
                return Mathf.RoundToInt(damageValue);

            } else {

                Debug.LogError("Defending unit has no armour type.");
                return Mathf.RoundToInt(damageValue);

            }

        //Slash Weapons

        } else if (weaponType == CombatDataType.WeaponType.Slash) {

            if(armourType == CombatDataType.ArmourType.Unarmoured) {

                damageValue = (attackingUnit.GetParamATK() * ineffectiveMultiplier) - defendingUnit.GetParamDEF();
                Debug.Log("Slash VS Unarmoured is ineffective. Dealing " + damageValue + " damage.");
                return Mathf.RoundToInt(damageValue);

            } else if (armourType == CombatDataType.ArmourType.Light) {

                damageValue = (attackingUnit.GetParamATK() * effectiveMultiplier) - defendingUnit.GetParamDEF();
                Debug.Log("Slash VS Light is effective. Dealing " + damageValue + " damage.");
                return Mathf.RoundToInt(damageValue);

            } else if (armourType == CombatDataType.ArmourType.Heavy) {

                damageValue = attackingUnit.GetParamATK() - defendingUnit.GetParamDEF();
                Debug.Log("Slash VS Heavy is neutral. Dealing " + damageValue + " damage.");
                return Mathf.RoundToInt(damageValue);

            } else {

                Debug.LogError("Defending unit has no armour type.");
                return Mathf.RoundToInt(damageValue);

            }

        //Blunt Weapons

        } else if (weaponType == CombatDataType.WeaponType.Blunt) {

            if(armourType == CombatDataType.ArmourType.Unarmoured) {

                damageValue = attackingUnit.GetParamATK() - defendingUnit.GetParamDEF();
                Debug.Log("Blunt VS Unarmoured is neutral. Dealing " + damageValue + " damage.");
                return Mathf.RoundToInt(damageValue);

            } else if (armourType == CombatDataType.ArmourType.Light) {

                damageValue = (attackingUnit.GetParamATK() * ineffectiveMultiplier) - defendingUnit.GetParamDEF();
                Debug.Log("Blunt VS Light is ineffective. Dealing " + damageValue + " damage.");
                return Mathf.RoundToInt(damageValue);

            } else if (armourType == CombatDataType.ArmourType.Heavy) {

                damageValue = (attackingUnit.GetParamATK() * effectiveMultiplier) - defendingUnit.GetParamDEF();
                Debug.Log("Blunt VS Heavy is effective. Dealing " + damageValue + " damage.");
                return Mathf.RoundToInt(damageValue);

            } else {

                Debug.LogError("Defending unit has no armour type.");
                return Mathf.RoundToInt(damageValue);

            }

        } else {

            Debug.LogError("Attacking unit has no weapon type.");
            return Mathf.RoundToInt(damageValue);

        }

    }

    public void SetAttackingUnit(UnitHandler attackingUnit) {
        this.attackingUnit = attackingUnit;
    }

    public void SetDefendingUnit(UnitHandler defendingUnit) {
        this.defendingUnit = defendingUnit;
    }

}
