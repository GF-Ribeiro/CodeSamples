using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EletricityObjectType { Battery, Resistor, Switch, Lamp, Rotator};

public class EletricityObject : MonoBehaviour
{
    //The type of the eletricity object
    public EletricityObjectType eletricityObjectType;

    //List of the objects conections
    public List<EletricityObject> connections;

    //The wires attached to the eletricity object
    public List<GameObject> attachedWires;

    public bool circuitClosed;

    

    //Recursive funcion that checks if the original object can reach the destinyObject
    public bool CanReach(EletricityObject destinyObject, EletricityObject originalObject)
    {
        //For each connection of the current Eletricity Object
        for (int i = 0; i < connections.Count; i++)
        { 
            //Returns true if the connected object is the object we are seeking
            if (connections[i].Equals(destinyObject))
            {
                return true;
            }
            //Returns false if the connected object is the original object
            else if (connections[i].Equals(originalObject))
            {
                return false;
            }
            // If its a Switch, checks if it is closed or open
            else if (connections[i].eletricityObjectType == EletricityObjectType.Switch)
            {
                bool isClosed = connections[i].GetComponent<ObjectProperties>().switchClosed;
                //if closed, calls the function in the connected object
                if (isClosed)
                {
                    //Calls the function in the connected object, returns true if it reached the destination object
                    if( connections[i].CanReach(destinyObject, originalObject))
                    {
                        return true;
                    }
                }
            }
            //If its a Resistor or a lamp, calls the function in the connected object, returns true if it reached the destination object
            else if (connections[i].eletricityObjectType == EletricityObjectType.Resistor || connections[i].eletricityObjectType == EletricityObjectType.Lamp)
            {
                if( connections[i].CanReach(destinyObject, originalObject))
                {
                    return true;
                }
            }
            //In case it is a rotator
            else if (connections[i].eletricityObjectType == EletricityObjectType.Rotator)
            {
                List<EletricityObject> rotatorConnections = connections[i].GetComponent<Rotator>().GetConections();

                //Iterates through all of the rotators connections
                for (int k = 0; k < rotatorConnections.Count; k++)
                {
                    //Returns true if the connected object is the object we are seeking
                    if (rotatorConnections[k].Equals(destinyObject))
                    {
                        return true;
                    }
                    //Calls the function in the connected object, returns true if it reached the destination object
                    if (rotatorConnections[k].CanReach(destinyObject, originalObject))
                    {
                        return true; 
                    }
                }

            }
        }

        //if the code gets here, it means all possible paths have been explored and none reached the battery
        return false;
    }

    public bool IsOnACompleteCircuit(EletricityObject battery)
    {
        //The system is on a complete circuit if the battery can reach the current Eletricity Object and the current Eletricity Object can reach the battery
        if (this.CanReach(battery, this) && battery.CanReach(this, battery))
        {   
            return true;
        }

        return false;
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < connections.Count; i++)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, connections[i].transform.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < connections.Count; i++)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, connections[i].transform.position);
        }
    }

    //Triggers the Shock animation for each wire
    public void ChargeWires()
    {
        if(circuitClosed)
        {
            for (int i = 0; i < attachedWires.Count; i++)
            {
                attachedWires[i].GetComponent<Animator>().SetTrigger("Shock");
            }
        }
    }
}
