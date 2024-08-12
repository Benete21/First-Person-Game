using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEditor.Rendering;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
<<<<<<< Updated upstream
using UnityEngine.Windows;
public class FirstPersonControl : MonoBehaviour
=======
using Unity.UI;
public class FirstPersonControls : MonoBehaviour
>>>>>>> Stashed changes
{
    // Public variables to set movement and look SPEED, and the player camera
    public float moveSpeed; // Speed at which the player moves
    public float lookSpeed; // Sensitivity of the camera movement
    public float gravity = -9.81f; // Gravity value
    public float jumpHeight = 1.0f; // Height of the jump
    public Transform playerCamera; // Reference to the player's camera

    // Private variables to STORE input values and the character controller
    private Vector2 moveInput; // Stores the movement input from the player
    private Vector2 lookInput; // Stores the look input from the player
    private float verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 velocity; // Velocity of the player

    private CharacterController characterController; // Reference to the CharacterController component
<<<<<<< Updated upstream
    private void Awake() //Use to initialsie references and components as soon as script instance is loaded (Starts Before the Start() Method)
=======

    [Header("SHOOTING SETTINGS")]
    [Space(5)]
    public GameObject projectilePrefab; // Projectile prefab for shooting
    public Transform firePoint; // Point from which the projectile is fired
    public float projectileSpeed = 20f; // Speed at which the projectile is fired


    [Header("PICKING UP SETTINGS")]
    [Space(5)]
    public Transform holdPosition; // Position where the picked-up object will be held
    private GameObject heldObject; // Reference to the currently held object
    public float pickUpRange = 3f; // Range within which objects can be picked up
    private bool holdingGun = false;

    [Header("CROUCH SETTINGS")]
    [Space(5)]
    public float crouchHeight = 1;      //make short
    public float standingHeight = 2;    //make normal
    public float crouchSpeed = 1.5f;    //make slow
    private bool isCrouching = false;   //whether the player is crouching or not

    [Header("SEARCH SETTINGS")]
    [Space(5)]
    public float searchRange = 5f;      //range in which items can be seen by the player
    public GameObject hiddenGameObject; 
    public Transform hiddenPosition;
    private bool hiddenItemActive = false;  // shows when the item has been active or not
    private bool isItemGrabbed = false;     //Checks if the hidden item has been collected

    private void Awake()
>>>>>>> Stashed changes
    {
        // Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        // Create a new instance of the INPUT ACTIONS
        var playerInput = new Controls();

        // Enable the input actions
        playerInput.Player.Enable();

        // Subscribe to the movement input events
        playerInput.Player.Movment.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Update moveInput when movement input is performed
                                                                                             // ctx is short for context(The keybindings in this instance eg the context for the left joy stick)
                                                                                             //sets the value of moveInput to the context of the readValue
        playerInput.Player.Movment.canceled += ctx => moveInput = Vector2.zero; // Reset moveInput when movement input is canceled

        // Subscribe to the look input events
        playerInput.Player.LookAround.performed += ctx => lookInput = ctx.ReadValue<Vector2>(); // Update lookInput when look input is performed
        playerInput.Player.LookAround.canceled += ctx => lookInput = Vector2.zero; // Reset lookInput when look input is canceled

        // Subscribe to the jump input event
<<<<<<< Updated upstream
        playerInput.Player.Jump.performed += ctx => Jump(); // Call the Jump method when jump input is performed
=======
        playerInput.Player.Jump.performed += ctx => Jump(); // Call theJump method when jump input is performed

        // Subscribe to the shoot input event
        playerInput.Player.Shoot.performed += ctx => Shoot(); // Call theShoot method when shoot input is performed

        // Subscribe to the pick-up input event
        playerInput.Player.PickUp.performed += ctx => PickUpObject(); //Call the PickUpObject method when pick-up input is performed

        //Subscribe to the crouch input event
        playerInput.Player.Crouch.performed += ctx => ToggleCrouch(); //Call the toggleCrouch method when crouch input is perfromed

        //Subscibe to the search item input event
        playerInput.Player.SearchItems.performed += ctx => SearchItems(); // Calls the search items method when search input is performed

        playerInput.Player.ItemsFoundTab.performed += ctx => FoundItems(); // Calls the search items method when search input is performed

>>>>>>> Stashed changes
    }
    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
    }
    public void Move()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Transform direction from local to world space - local space rotation of the game object _ world space referes to the whole worlds perspective (Want the player to move according to the world)
        move = transform.TransformDirection(move);

        // Move the character controller based on the movement vector and speed
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }
    public void LookAround()
    {
        // Get horizontal and vertical look inputs and adjust based onsensitivity
        float LookX = lookInput.x * lookSpeed;
        float LookY = lookInput.y * lookSpeed;

        // Horizontal rotation: Rotate the player object around the y-axis
        transform.Rotate(0, LookX, 0);

        // Vertical rotation: Adjust the vertical look rotation and clampit to prevent flipping
        verticalLookRotation -= LookY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f); // Restricting the look up and down to 90 degrees and 90 degrees

        // Apply the clamped vertical rotation to the player camera
        playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
    }
    public void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f; // Small value to keep the player grounded
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity to thevelocity
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
<<<<<<< Updated upstream
}

=======
    public void Shoot()
    {
        if (holdingGun == true)
        {
            // Instantiate the projectile at the fire point
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            // Get the Rigidbody component of the projectile and set its velocity
            Rigidbody rb = projectile.GetComponent<Rigidbody>(); rb.velocity = firePoint.forward * projectileSpeed;
            // Destroy the projectile after 3 seconds
            Destroy(projectile, 3f);
        }
    }
    public void PickUpObject()
    {
        // Check if we are already holding an object
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false; //Enable physics
            heldObject.transform.parent = null;
            holdingGun = false;
        }

        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view not necessary
        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // Check if the hit object has the tag "PickUp"
            if (hit.collider.CompareTag("PickUp"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                isItemGrabbed = true; //This makes the hidden item equal to true, used here so the item does not dissapear when it is in your hand


    // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
            }
            else if (hit.collider.CompareTag("Gun"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
                holdingGun = true;
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

    public void SearchItems()
    {
            // Instantiate the hiddn item  =where the object is 
            GameObject hiddenItem = Instantiate(hiddenGameObject, hiddenPosition.position, hiddenPosition.rotation);
            // Get the Rigidbody component of the hidden item
            Rigidbody rb = hiddenItem.GetComponent<Rigidbody>();
            // Destroy the projectile after 6 seconds
            if(isItemGrabbed == false) 
            {
                Destroy(hiddenItem, 6f);
            }
    }
    public void FoundItems()
    {
        
    }
}
>>>>>>> Stashed changes
