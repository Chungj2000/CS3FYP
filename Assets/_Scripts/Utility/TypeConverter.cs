using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Utility Script for dealing with type conversion.
 */
public class TypeConverter : MonoBehaviour {

    public static TypeConverter INSTANCE {get; private set;}

    private void Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("TypeConverter instance created.");
        } else {
            Debug.LogError("More than one TypeConverter instance created.");
            Destroy(this);
            return;
        }
    }
    
    //Converters for CombatType into a string values.
    public string WeaponTypeToString(CombatDataType.WeaponType weaponType) {
        switch(weaponType) {

            case CombatDataType.WeaponType.Pierce:
                //Debug.Log("Pierce");
                return "PIERCE";

            case CombatDataType.WeaponType.Slash:
                //Debug.Log("Slash");
                return "SLASH";

            case CombatDataType.WeaponType.Blunt:
                //Debug.Log("Blunt");
                return "BLUNT";

            default:
                Debug.LogError("No weapon type found.");
                return "";
        }
    }

    public string ArmourTypeToString(CombatDataType.ArmourType armourType) {
        switch(armourType) {

            case CombatDataType.ArmourType.Unarmoured:
                //Debug.Log("Unarmoured");
                return "UNARMOURED";

            case CombatDataType.ArmourType.Light:
                //Debug.Log("Light");
                return "LIGHT";

            case CombatDataType.ArmourType.Heavy:
                //Debug.Log("Heavy");
                return "HEAVY";

            default:
                Debug.LogError("No armour type found.");
                return "";
        }
    }

}
