using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitUI : AbstractUnitUI {

    public static PlayerUnitUI INSTANCE {get; private set;}

    private void  Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("PlayerUnitUI instance created.");
        } else {
            Debug.Log("More than one PlayerUnitUI instance created.");
            Destroy(gameObject);
            return;
        }
    }

}
