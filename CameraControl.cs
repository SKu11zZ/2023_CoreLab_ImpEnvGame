using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    private float cameraMinDistance;
    private bool isFree;
    // Start is called before the first frame update
    void Start()
    {
        cameraMinDistance = Vector3.Distance(Camera.main.transform.position, this.transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        if (!UIControl.Instance.IsShowUnlock && Input.GetKeyDown(KeyCode.Space))
        {
            isFree = !isFree;
            SceneControl.Instance.enabled = !isFree;
        }
        if (isFree)
        {
            if (Input.GetMouseButton(0))
            {
                this.transform.position += new Vector3(-Input.GetAxis("Mouse X"), 0f, -Input.GetAxis("Mouse Y")) * Time.deltaTime * 5f;
            }
            Camera.main.transform.localPosition += Camera.main.transform.forward * Input.mouseScrollDelta.y;
            if (Vector3.Distance(Camera.main.transform.position, this.transform.position) < cameraMinDistance)
            {
                Camera.main.transform.localPosition = -Camera.main.transform.forward * cameraMinDistance;
            }
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, target.position, 0.1f);
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, -Camera.main.transform.forward * cameraMinDistance, 0.1f);
        }
    }
}
