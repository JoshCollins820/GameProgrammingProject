using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWardrobe : MonoBehaviour
{
    // Wardrobe positions
    [SerializeField] Transform door;
    [SerializeField] Transform closedPos;
    [SerializeField] Transform peekPos;

    public float doorSpeed = 8f;
    public bool open = false;


    // Start is called before the first frame update
    void Start()
    {
        // Grab the door and transforms of the positions
        door = this.transform.parent.gameObject.transform.Find("Door").gameObject.transform;
        closedPos = this.transform.parent.gameObject.transform.Find("ClosedPosition").gameObject.transform;
        peekPos = this.transform.parent.gameObject.transform.Find("PeekPosition").gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is hiding in the wardrobe, "PeekPosition"
        if(open)
        {
            door.SetPositionAndRotation(Vector3.Lerp(door.position, peekPos.position, Time.deltaTime * doorSpeed),
                Quaternion.Lerp(door.rotation, peekPos.rotation, Time.deltaTime * doorSpeed));
        }
        // If the player is not in the wardrobe, "ClosedPosition"
        else if(!open)
        {
            door.SetPositionAndRotation(Vector3.Lerp(door.position, closedPos.position, Time.deltaTime * doorSpeed),
                Quaternion.Lerp(door.rotation, closedPos.rotation, Time.deltaTime * doorSpeed));
        }
    }
}
