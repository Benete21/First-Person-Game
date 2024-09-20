using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Windows;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.UIElements.Experimental;
using System.Drawing;
public class FirstPersonControls : MonoBehaviour
{
    [Header("MOVEMENT SETTINGS")]
    [Space(5)]
    // Public variables to set movement and look speed, and the playercamera
    public float moveSpeed; // Speed at which the player moves
    public float lookSpeed; // Sensitivity of the camera movement
    public float gravity = -9.81f; // Gravity value
    public float jumpHeight = 1.0f; // Height of the jump
    public Transform playerCamera; // Reference to the player's camera
                                   // Private variables to store inputvalues and the character controller
    private Vector2 moveInput; // Stores the movement input from the player
    private Vector2 lookInput; // Stores the look input from the player
    private float verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 velocity; // Velocity of the player
    private CharacterController characterController; // Reference to the CharacterController component

    //does not need shooting mechanic
    /*[Header("SHOOTING SETTINGS")]
    [Space(5)]
    public GameObject projectilePrefab; // Projectile prefab for shooting
    public Transform firePoint; // Point from which the projectile is fired
    public float projectileSpeed = 20f; // Speed at which the projectile is fired*/


    [Header("PICKING UP SETTINGS")]
    [Space(5)]
    public Transform holdPosition; // Position where the picked-up object will be held
    private GameObject heldObject; // Reference to the currently held object
    public float pickUpRange = 3f; // Range within which objects can be picked up
    //private bool holdingGun = false;

    [Header("CROUCH SETTINGS")]
    [Space(5)]
    public float crouchHeight = 1;      //make short
    public float standingHeight = 2;    //make normal
    public float crouchSpeed = 1.5f;    //make slow
    private bool isCrouching = false;   //whether the player is crouching or not

    [Header("SEARCH SETTINGS")]
    [Space(5)]
    public Transform hiddenPosition; // Position where hiddenitem is
    public Transform clue1Position;
    public Transform clue2Position;
    public Transform clue3Position;
    public GameObject hiddenObject; // Referese to hidden item prefab
    public GameObject clue1Object;
    public GameObject clue2Object;
    public GameObject clue3Object;
    public float hiddenItemRange = 6f; 
    private bool isHoldingHidden = false; // asks if the hidden item is held
    private bool hiddenActive = false;  //asks if thr hidden item is in the scene

    [Header("INVENTORY SETTINGS")]
    [Space(5)]
    public GameObject canvaInventory;
    private bool canvaActive = false;
    public Transform ItemContent;
    public GameObject InventoryItem;

    public List<ItemInfo> items = new List<ItemInfo>(); // list to help track all the itmes information
    public ItemInfo iteminfo;

    private Text itemName;
    private Text itemDesc;
    private Sprite itemIcon;

    [Header("INTERACT SETTINGS")]
    [Space(5)]
    public bool isDoorOpen = false;
    public float DoorAngle = 90f;
    public float openSpeed = 2f;
    public Quaternion openRotation;
    public Quaternion closedRotation;
    public bool Key1 = false;
    public bool Key2 = false;
    public bool Key3 = false;
    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        // Create a new instance of the input actions
        var playerInput = new Controls();

        // Enable the input actions
        playerInput.Player.Enable();

        // Subscribe to the movement input events
        playerInput.Player.Movment.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Update moveInput when movement input is performed
        playerInput.Player.Movment.canceled += ctx => moveInput = Vector2.zero; // Reset moveInput when movement input is canceled

        // Subscribe to the look input events
        playerInput.Player.LookAround.performed += ctx => lookInput = ctx.ReadValue<Vector2>(); // Update lookInput when look input is performed
        playerInput.Player.LookAround.canceled += ctx => lookInput = Vector2.zero; // Reset lookInput when look input is canceled

        // Subscribe to the jump input event
        playerInput.Player.Jump.performed += ctx => Jump(); // Call theJump method when jump input is performed

        // Subscribe to the shoot input event
        //playerInput.Player.Shoot.performed += ctx => Shoot(); // Call theShoot method when shoot input is performed

        // Subscribe to the pick-up input event
        playerInput.Player.PickUp.performed += ctx => PickUpObject(); //Call the PickUpObject method when pick-up input is performed

        //Subscribe to the crouch input event
        playerInput.Player.Crouch.performed += ctx => ToggleCrouch(); //Call the toggleCrouch method when crouch input is perfromed

        // Subscribe to the hiddenItemsearch event
        playerInput.Player.SearchHiddenItems.performed += ctx => SearchHidden(); //Call the SearchHidden method when hiddenItemSearch input is performed

        // Subscribe to the inventory event
        playerInput.Player.ToggleInventory.performed += ctx => ToggleInventory(); //Call the ToggleInventory method when inventory input is performed

        // Subscribe to the inventory adder event
        playerInput.Player.PutInInventory.performed += ctx => PutInInventory(); //Call the PutInInventory method when inventory adderinput is performed

        // Subscribe to the interact input event
        playerInput.Player.Interact.performed += ctx => Interact(); // Interact with switch
    }
    private void Update()
    {
        // Call Move and LookAround methods every frame to handle playermovement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
    }
    public void Move()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Transform direction from local to world space
        move = transform.TransformDirection(move);

        float currentSpeed;
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        // Move the character controller based on the movement vector and speed
    characterController.Move(move * moveSpeed * Time.deltaTime);
    }
    public void LookAround()
    {
        // Get horizontal and vertical look inputs and adjust based on sensitivity
    float LookX = lookInput.x * lookSpeed;
        float LookY = lookInput.y * lookSpeed;
        // Horizontal rotation: Rotate the player object around the y-axis
        transform.Rotate(0, LookX, 0);
        // Vertical rotation: Adjust the vertical look rotation and clamp it to prevent flipping
    verticalLookRotation -= LookY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f,
        90f);
        // Apply the clamped vertical rotation to the player camera
        playerCamera.localEulerAngles = new Vector3(verticalLookRotation,
        0, 0);
    }
    public void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f; // Small value to keep the player grounded
        }
        velocity.y += gravity * Time.deltaTime; // Apply gravity to the velocity
    characterController.Move(velocity * Time.deltaTime); // Apply the velocity to the character
    }
    public void Jump()
    {
        if (characterController.isGrounded)
        {
            // Calculate the jump velocity
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    /*public void Shoot()
    {
        if (holdingGun == true)
        {
            // Instantiate the projectile at the fire point
            GameObject projectile = Instantiate(projectilePrefab,firePoint.position, firePoint.rotation);
            // Get the Rigidbody component of the projectile and set its velocity
            Rigidbody rb = projectile.GetComponent<Rigidbody>();rb.velocity = firePoint.forward * projectileSpeed;
            // Destroy the projectile after 3 seconds
            Destroy(projectile, 3f);
        }
    }*/
    public void PickUpObject()
    {
        // Check if we are already holding an object
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false; //Enable physics
            heldObject.transform.parent = null;
            //holdingGun = false;
        }

        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view not necessary
        //Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // Check if the hit object has the tag "PickUp"
            if (hit.collider.CompareTag("PickUp"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;

                isHoldingHidden = true;
            }
           /* else if (hit.collider.CompareTag("Gun"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
               // holdingGun = true;
            }*/
            else if (hit.collider.CompareTag("Key1"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;

                isHoldingHidden = true;
                Key1 = true;
            }
            else if (hit.collider.CompareTag("Key2"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;

                isHoldingHidden = true;
                Key2 = true;
            }
            else if (hit.collider.CompareTag("Key3"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;

                isHoldingHidden = true;
                Key3 = true;
            }
        }
    }
    public void ToggleCrouch()
    {
        if (isCrouching)
        {
            characterController.height = standingHeight;
            isCrouching = false;
        }
        else 
        {
            characterController.height = crouchHeight;  
            isCrouching = true;
        }
    }

    public void SearchHidden()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view not necessary
        //Debug.DrawRay(playerCamera.position, playerCamera.forward * hiddenItemRange, Color.red, 2f);

        if (Physics.Raycast(ray, out hit, hiddenItemRange))
        {
            //if (hit.collider.CompareTag("Hidden"))
            //{
                if (hiddenActive == false)
                {
                    // Instantiate the hiddenitem in the hidden item posiyion
                    GameObject hiddenItem = Instantiate(hiddenObject, hiddenPosition.position, hiddenPosition.rotation);
                    GameObject clue1 = Instantiate(clue1Object, clue1Position.position, clue1Position.rotation);
                    GameObject clue2 = Instantiate(clue2Object, clue2Position.position, clue2Position.rotation);
                    GameObject clue3 = Instantiate(clue3Object, clue3Position.position, clue3Position.rotation);
                    // Get the Rigidbody component of the hiddenItem
                    Rigidbody rb = hiddenItem.GetComponent<Rigidbody>();
                    Rigidbody rbclue1 = clue1.GetComponent<Rigidbody>();
                    Rigidbody rbclue2 = clue2.GetComponent<Rigidbody>();
                    Rigidbody rbclue3 = clue3.GetComponent<Rigidbody>();
                // Destroy the hiddenitem if the item is not held
                    hiddenActive = true;
                    Debug.Log("found item");
                    //used to make the item disapear after some time
                   /* if (isHoldingHidden == false)
                    {
                        // Destroy the hiddenitem if the item is not held
                        Destroy(hiddenItem, 7f);
                        Destroy(clue1, 7f);
                        Destroy(clue2, 7f);
                        Destroy(clue3, 7f);
                    }*/
                }
            // }
        }
        else
        {
            Debug.Log("not close to item");
        }
    }
    public void ToggleInventory() // toggles the inventory 
    {
        if (canvaActive)
        {
            canvaInventory.SetActive(false);
            canvaActive = false;
        }
        else
        {
            canvaInventory.SetActive(true);
            canvaActive = true;
        }
    }
    public void PutInInventory()// code to put items in the hand into invetory
    {
            if (heldObject != null)
            {
                Destroy(heldObject); // destroys the held item on the scene
                GameObject inventItem = Instantiate(InventoryItem, ItemContent); // puts the held item that was in the hand into the inventory
                itemName = inventItem.transform.Find("ItemName").GetComponent<Text>(); // this is so the data in the iteminfo can be assigned to the same in the inventory UI
                itemDesc = inventItem.transform.Find("ItemDesc").GetComponent<Text>();

                itemName.text = iteminfo.getName();
                itemDesc.text = iteminfo.getDescription();
            }
    }
    public void Interact()
    {
        // Perform a raycast to detect the lightswitch
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (Key1 == true)
            {
                if (hit.collider.CompareTag("Door1")) // Check if the object is a door
                {              
                    // Start moving the door upwards
                    StartCoroutine(RaiseDoor(hit.collider.gameObject));
                }
            }
            else if (Key2 == true)
            {
                if (hit.collider.CompareTag("Door2")) // Check if the object is a door
                {
                    // Start moving the door upwards
                    StartCoroutine(RaiseDoor(hit.collider.gameObject));
                }
            }
            else if (Key3 == true)
            {
                if (hit.collider.CompareTag("Door3")) // Check if the object is a door
                {
                    // Start moving the door upwards
                    StartCoroutine(RaiseDoor(hit.collider.gameObject));
                }
            }

        }
    }
    private IEnumerator RaiseDoor(GameObject door)
        {
            float raiseAmount = 5f; // The total distance the door will be raised
            float raiseSpeed = 2f; // The speed at which the door will be raised
            Vector3 startPosition = door.transform.position; // Store the initial position of the door
            Vector3 endPosition = startPosition + Vector3.up * raiseAmount; // Calculate the final position of the door after raising
                                                                            // Continue raising the door until it reaches the target height
            while (door.transform.position.y < endPosition.y)
            {
                // Move the door towards the target position at the specifiedspeed
                door.transform.position = Vector3.MoveTowards(door.transform.position, endPosition, raiseSpeed * Time.deltaTime);
                yield return null; // Wait until the next frame before continuing the loop
            }
        }
    }


