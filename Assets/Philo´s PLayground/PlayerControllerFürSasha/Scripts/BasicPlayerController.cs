using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicPlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    //private Canvas playerUi;
    private TMP_Text mouseIndicator;
    private Camera firstPersonCam;
    private string mouseIndicatorStandardText = "NIX";
    public Canvas escMenu;
    public Canvas inventory;
    public float mouseSpeed = 2.0f;
    public float mouseYRestriction = 90;
    private float verticalAxis;
    private float horizontalAxis;
    [SerializeField]
    private float forwardSpeed = 15;
    [SerializeField]
    private float strafeSpeed = 18;
    public float airSpeed = 5.0f;
    [SerializeField]
    private float jumpForce = 6.66f;

    public float rightClickHoldTime = 3.5f;
    public float mouseResetTime = 0.5f;
    public float playerAccelarationForce = 20.5f;
   
    public bool isInMenu = false;
    public bool allowMovementWhileInMenu = false;
    public bool isOnGround;
    //need to look in to player phhysics, standard 
    public bool allowMidAirMovement = true;
    //private bool usePhhysicsMOvement = false;
    private float rightMouseButtonTimer;
    private float resetCrosshairTimer;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        //playerUi = GetComponentInChildren<Canvas>();
        mouseIndicator = GetComponentInChildren<TMP_Text>();
        firstPersonCam = GetComponentInChildren<Camera>();
        
        //mouseIndicator.gameObject.SetActive(false);
        mouseIndicator.text = mouseIndicatorStandardText;
        escMenu.gameObject.SetActive(false);
        inventory.gameObject.SetActive(false);

        rightMouseButtonTimer = Time.time;
        resetCrosshairTimer = Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        resetCrosshairTimer += Time.deltaTime;
        
        HandleMouse();
        HandleMouseLook();
        HandleMovement();
        MiscellaneousInput(/*Input.inputString*/);

        Debug.Log(rightMouseButtonTimer);
        Debug.Log(Input.inputString);
    }
    private void HandleMouseLook()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSpeed;
        float verticalRotation = Input.GetAxis("Mouse Y") * mouseSpeed;
        float _camYRotation = firstPersonCam.gameObject.transform.rotation.x;

        gameObject.transform.Rotate(0, horizontalRotation, 0);

        if (_camYRotation >= 0.6) 
        {
            firstPersonCam.gameObject.transform.Rotate(-1, 0, 0);
            return; }
        else if (_camYRotation <= -0.6) 
        {
            firstPersonCam.gameObject.transform.Rotate(1, 0, 0);
            return; }
        else {firstPersonCam.gameObject.transform.Rotate(-verticalRotation, 0, 0); }
        
        
        Debug.Log(firstPersonCam.gameObject.transform.rotation.x);
        Debug.Log(_camYRotation);
        
    }
    private void HandleMovement()
    {
        verticalAxis = Input.GetAxis("Vertical");
        horizontalAxis = Input.GetAxis("Horizontal");

        //restrict or allow W,A,S,D while player is Airborne or in Menu
        if (isOnGround || allowMidAirMovement) 
        {
            if (allowMovementWhileInMenu || !isInMenu)
            {
                if (!isOnGround) { TransformPlayer(airSpeed, airSpeed); }
                else { TransformPlayer(forwardSpeed, strafeSpeed); }
              }
            
        }
        //Space
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround == true) 
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    private void TransformPlayer(float _forwardSpeed, float _strafeSpeed)
    {
        //W,A
        //playerRb.AddRelativeForce(Vector3.forward * playerAccelarationForce * verticalAxis, ForceMode.Impulse);
        transform.Translate(Vector3.forward * Time.deltaTime * _forwardSpeed * verticalAxis);
        //S,D
        transform.Translate(Vector3.left * Time.deltaTime * _strafeSpeed * -horizontalAxis);
    }
    private void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseIndicator.text = "you pressed the left mouse button";
            Debug.Log("left mouse button pressed");
            resetCrosshairTimer = 0;
        }
        else if (Input.GetMouseButton(1))
        {
            rightMouseButtonTimer += Time.deltaTime;
            Debug.Log("right mouse button pressed"); 
        }
        else if (Input.GetMouseButtonUp(1))
        {
            rightMouseButtonTimer = 0;
        }
        else if (resetCrosshairTimer >= mouseResetTime)
        {
            mouseIndicator.text = mouseIndicatorStandardText;
        }
        Debug.Log(Input.GetMouseButton(0));


        if (rightMouseButtonTimer >= rightClickHoldTime/7)
        {
            mouseIndicator.text = "halten ...";
            if (rightMouseButtonTimer >= rightClickHoldTime / 2.5f)
            {
                mouseIndicator.text += "Standby....";

                if (rightMouseButtonTimer >= rightClickHoldTime)
                {
                    mouseIndicator.text = "hmmm strange ... eigentlich sollte jetzt was passieren";
                    resetCrosshairTimer = 0;
                    rightMouseButtonTimer = 0;
                }
            }
        }
    }
    private void MiscellaneousInput()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape) && escMenu.gameObject.activeInHierarchy == false && inventory.gameObject.activeInHierarchy == false)
        {  SetMenu(escMenu, true);}
        else if (Input.GetKeyDown(KeyCode.Escape))
        {   SetMenu(escMenu, false);}
        if (Input.GetKeyDown(KeyCode.I) && escMenu.gameObject.activeInHierarchy == false && inventory.gameObject.activeInHierarchy == false)
        { SetMenu(inventory, true); }
        else if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
        {   SetMenu(inventory, false);}
    }
    //private void MiscellaneousInput(string input)
    //{
    //  if (string.IsNullOrEmpty(input))
    //    { return; }
    //  else if (input == "Escape")
    //    {
    //        SetMenu(escMenu, false);
    //    }
    //}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        { isOnGround = false;}
    }
    
    public void CloseMenu(Canvas canvas)
    {
        SetMenu(canvas, false);
    }
    private void SetMenu(Canvas canvas, bool isActive)
    {
        canvas.gameObject.SetActive(isActive);
        isInMenu = isActive;
    }
}
