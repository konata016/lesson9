using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    public float speed = 3;
    GameObject sword;
    Vector3 move;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        sword = transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float X_Rotation = Input.GetAxis("Mouse X");
        transform.Rotate(0, X_Rotation, 0);

        float angleDir = transform.eulerAngles.y * (Mathf.PI / 180.0f);
        Vector3 dirVertical = new Vector3(Mathf.Sin(angleDir), 0, Mathf.Cos(angleDir));
        Vector3 dirHorizontal = new Vector3(-Mathf.Cos(angleDir), 0, Mathf.Sin(angleDir));

        move = dirHorizontal * Input.GetAxis("Horizontal") * speed;
        move = dirVertical * Input.GetAxis("Vertical") * speed;
        rb.AddForce(speed * (move - rb.velocity));

        Swipe();
    }

    void Swipe()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 pos = ray.GetPoint(5);
            sword.transform.LookAt(pos);
        }
    }
}
