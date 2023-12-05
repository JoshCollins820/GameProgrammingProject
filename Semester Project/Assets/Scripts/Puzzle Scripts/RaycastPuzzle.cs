using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPuzzle : MonoBehaviour
{

    public Camera Cam;
    public LayerMask mask;


    // Start is called before the first frame update
    void Start()
    {
        Cam = Camera.main;

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

            if (Physics.Raycast(ray, out hit, 10, mask))
            {
                Debug.Log(hit.transform.name);
                hit.transform.GetComponent<RuneOnClick>().ActivateRune();
            }
        }
    }
}
