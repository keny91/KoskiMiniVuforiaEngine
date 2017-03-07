using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingElementTooglable : GrowingElement
{

	
	// Update is called once per frame
	new void Update () {

        checkDistance();
        animationElements.SetBool("DoGrowth", growthTriggered);
    }


}
