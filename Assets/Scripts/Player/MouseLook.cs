using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MouseLook : MonoBehaviour
{
    public TextMeshProUGUI mainDialog;
    public Texture2D cursorTexture;
    public MobilePhone mobilePhone;

    private const bool X_INVERSE = false;
    private const bool Y_INVERSE = false;
    private const float SENSITIVITY = 20;

    private const float speed = 0.5f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);

        //mainDialog.gameObject.SetActive(false);
    }

    public void ToggleCursor()
    {
        Cursor.visible = !Cursor.visible;
    }

    void Update()
    {
        PollForAxisAdjustments();

         if (Input.GetKeyDown(KeyCode.Escape)) {
             if (Cursor.lockState == CursorLockMode.Locked) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
             }else{
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
             }
         }

        if (mobilePhone.Active)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layerMask = 1 << 8;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                //Debug.DrawLine(ray.origin, hit.point);
                if (hit.collider != null)
                    ManageCollisions(hit.collider.gameObject);
            }
        }

    }

    private void ManageCollisions( GameObject go )
    {
        MonsterBehaviour monster = go.GetComponent<MonsterBehaviour>();

        if (monster && monster.IsActive)
        {
            monster.Suffer();
        }
    }

    private void PollForAxisAdjustments()
    {
        float xAxisMovement = Input.GetAxis("Mouse X") * SENSITIVITY * Time.deltaTime;
        float yAxisMovement = Input.GetAxis("Mouse Y") * SENSITIVITY * Time.deltaTime;

        if (X_INVERSE) {
            xAxisMovement *= -1;
        }

        if (Y_INVERSE) {
            yAxisMovement *= -1;
        }

        makeXAxisAdjustments(xAxisMovement);
        makeYAxisAdjustments(yAxisMovement);
    }

     private void makeXAxisAdjustments(float adjustment) {
        if (adjustment == 0) return;

        Vector3 currentRotation = this.gameObject.transform.eulerAngles;
        Quaternion newRotation = Quaternion.Euler(currentRotation.x, (currentRotation.y + adjustment), currentRotation.z);
        //this.gameObject.transform.rotation = newRotation;
        this.gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, newRotation, Time.time * speed);
    }

    private void makeYAxisAdjustments(float adjustment) {
        if (adjustment == 0) return;

        Vector3 currentRotation = this.gameObject.transform.eulerAngles;
        Quaternion newRotation = Quaternion.Euler((currentRotation.x - adjustment), currentRotation.y, currentRotation.z);
        //this.gameObject.transform.rotation = newRotation;
        this.gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, newRotation, Time.time * speed);
    }
}
