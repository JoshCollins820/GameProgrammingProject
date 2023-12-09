using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPuzzle : MonoBehaviour
{

    public Camera Cam;
    public LayerMask MusicPuzzleMask;
    public LayerMask FeronsRoomPuzzleMask;
    public GameObject FRPuzzle;


    // Start is called before the first frame update
    void Start()
    {
        Cam = Camera.main;
        FRPuzzle = GameObject.Find("FeronsRoomPuzzle");

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        mousePos = Cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, Color.blue);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10, MusicPuzzleMask))
            {
                Debug.Log("Raycast hit " + hit.transform.name);
                hit.transform.GetComponent<RuneOnClick>().ActivateRune();
            }
            else if (Physics.Raycast(ray, out hit, 10, FeronsRoomPuzzleMask))
            {
                Debug.Log("Raycast hit " + hit.transform.name);
                FRPuzzle.GetComponent<FeronsRoomPuzzle>().PickObject(hit.transform.name);
            }
        }
    }
}
