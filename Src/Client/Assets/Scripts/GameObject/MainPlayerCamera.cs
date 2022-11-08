using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerCamera : MonoSingleton<MainPlayerCamera>
{
    public new Camera camera;
    public Transform viewPoint;

    public GameObject player;
    public float followSpeed = 5f;
    public float rotateSpeed = 5f;
    private Quaternion quater = Quaternion.identity;

    private void LateUpdate()
    {
        if (player == null)
        {
            if (Models.User.Instance.CurrentCharacterObject != null)
                player = Models.User.Instance.CurrentCharacterObject.gameObject;
            else
                return;
        }
        this.transform.position = Vector3.Lerp(transform.position, player.transform.position, followSpeed * Time.deltaTime);
        if (Input.GetMouseButton(1))
        {
            Vector3 angleBase = this.transform.localRotation.eulerAngles;
            this.transform.rotation = Quaternion.Euler
                (angleBase.x - Input.GetAxis("Mouse Y") * rotateSpeed, angleBase.y + Input.GetAxis("Mouse X") * rotateSpeed, 0);
            Vector3 angle = this.transform.rotation.eulerAngles - player.transform.rotation.eulerAngles;
            angle.z = 0;
            quater = Quaternion.Euler(angle);
        }
        else
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, player.transform.rotation * quater, followSpeed * Time.deltaTime);
        }
        if (Input.GetAxis("Vertical") > 0.01)
        {
            quater = Quaternion.Lerp(quater, Quaternion.identity, Time.deltaTime * followSpeed);
        }
    }
}
