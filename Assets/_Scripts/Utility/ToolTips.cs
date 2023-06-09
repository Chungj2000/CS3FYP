using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script used for storing tooltip strings which can be attached to visual components.
 * ToolTipUI uses this to update tooltip strings on text fields.
 */
public class ToolTips : MonoBehaviour {

    [SerializeField] UnitUI_Tips tip;

    //Tooltip text for UnitUI hover over values in the ToolTipUI text field.
    private const string nameTip =        "Name of the Unit.";
    private const string healthTip =      "The amount of HP a Unit has. Having 0 HP results in the Unit dying.";
    private const string attackTip =      "The Attack value of a Unit. Damage dealt towards a Unit is calculated from this.";
    private const string defenceTip =     "The Defence value of a Unit. Damage received from a Unit is calculated from this.";
    private const string movementTip =    "The Movement value of a Unit. This determines how many tiles a Unit can traverse.";
    private const string attackRangeTip = "The Attack Range value of a Unit. This determines how far a Unit can attack from.";
    private const string costTip =        "The Cost of a Unit. Summoning a Unit will deduce your total gold by this amount.";
    private const string weaponTypeTip =  "Determines the effectiveness of an attack by comparing with Armour Type.";
    private const string armourTypeTip =  "Determines the effectiveness of an attack by comparing with Weapon Type.";
    private const string goldTip =        "The amount of Gold a Player possesses. This is needed to summon more Units.";
    
    public enum UnitUI_Tips {
        Name,
        Health,
        Attack,
        Defence,
        Movement,
        AttackRange,
        Cost,
        WeaponType,
        ArmourType,
        Gold
    }

    //Determine which tip to display based on the enum case.
    public string IdentifyUnitUI_Tip(UnitUI_Tips tip) {
        switch(tip) {

            case UnitUI_Tips.Name:
                //Debug.Log("Name");
                return nameTip;

            case UnitUI_Tips.Health:
                //Debug.Log("Health");
                return healthTip;

            case UnitUI_Tips.Attack:
                //Debug.Log("Attack");
                return attackTip;

            case UnitUI_Tips.Defence:
                //Debug.Log("Defence");
                return defenceTip;

            case UnitUI_Tips.Movement:
                //Debug.Log("Movement");
                return movementTip;

            case UnitUI_Tips.AttackRange:
                //Debug.Log("Attack Range");
                return attackRangeTip;

            case UnitUI_Tips.Cost:
                //Debug.Log("Cost");
                return costTip;

            case UnitUI_Tips.WeaponType:
                //Debug.Log("Cost");
                return weaponTypeTip;

            case UnitUI_Tips.ArmourType:
                //Debug.Log("Cost");
                return armourTypeTip;

            case UnitUI_Tips.Gold:
                //Debug.Log("Gold");
                return goldTip;
            
            default:
                Debug.LogError("No tip found.");
                return "Error";
        }
    }

    //Getters.
    public UnitUI_Tips GetTip() {
        return tip;
    }
    
}
