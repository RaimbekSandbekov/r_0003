using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
	float panSpeed = 0.1f;
	float zoomSpeed = 0.1f;
	float rotSpeed = 2;
	float angleSpeed = 1;

	float panXSlimit = -150f;
	float panXMlimit = 150f;
	float panZSlimit = -150f;
	float panZMlimit = 150f;

	float zoomUpLimit = 50f;
	float zoomDownLimit = 4f;

	float angleUpLimit = 25f;
	float angleDownLimit = 80f;

	public GameObject Controller;

	bool MOVECAMERA = true;

	public GameObject turret;
	public Camera Cam;
	bool buildMode = false;

	bool UnitSelected = false;
	public GameObject plShip;
	
	void Start()
    {
		plShip = null;
    }

	void Update()
	{
		Function_Click_Raycast();   // FUNCTION WITH CLICK-RAYCAST on Unit and MOVE ship on click position
		Function_SmartPhone_Controls();
		//off Function_Computer_Controls();
		//off Function_Camera_Movement_Limits();  // Sets Limits on Movement and Rotation of CAMERA
	}

	void Function_Click_Raycast()	// FUNCTION WITH CLICK-RAYCAST on Unit and MOVE ship on click position
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 300.0f))
			{
				if (hit.collider.tag == "Water_Plane")
				{
					if (UnitSelected)
					{
						//plShip.transform.position = hit.point;
						//plShip.destination = hit.point;
						//plShip.GetComponent<Ship>().destination = hit.point;
						if (hit.collider.name == "Pl_Hitbox_fletcher")
						{
							print("dd");
							
						}
						if (hit.collider.name == "Pl_Lexington_hitbox")
						{
							print("lx");
							
						}

						if(plShip.GetComponent<Ship>())
						{
							Ship shipscript = plShip.GetComponent<Ship>();
							shipscript.isMoving = true;
							shipscript.destination = hit.point;
							UnitSelected = false;
						}
						if (plShip.GetComponent<Carrier>())
						{
							Carrier shipscript = plShip.GetComponent<Carrier>();
							shipscript.isMoving = true;
							shipscript.destination = hit.point;
							UnitSelected = false;
						}
						if (plShip.GetComponent<Cruiser>())
						{
							Cruiser shipscript = plShip.GetComponent<Cruiser>();
							shipscript.isMoving = true;
							shipscript.destination = hit.point;
							UnitSelected = false;
						}
					}
				}
				if (hit.collider.tag == "Player_Ship")
				{
					UnitSelected = true;
					plShip = hit.collider.gameObject;
				}
			}
		}
	}

	void Function_SmartPhone_Controls()
	{
		if (MOVECAMERA == true)
		{
			if (Input.touchCount == 4)							  // X-Rotation
			{
				Touch touchZero = Input.GetTouch(0);
				if (touchZero.phase == TouchPhase.Moved)
				{
					Cam.transform.Rotate(-Vector3.right * Time.deltaTime * angleSpeed * touchZero.deltaPosition.y, Space.Self);
				}
			}
			else if (Input.touchCount == 3)                       // Y-Rotation
			{
				Touch touchZero = Input.GetTouch(0);
				if (touchZero.phase == TouchPhase.Moved)
				{
					Cam.transform.Rotate(-Vector3.up * Time.deltaTime * rotSpeed * touchZero.deltaPosition.x, Space.World); // rotate
				}
			}
			else if (Input.touchCount == 2)
			{
				Touch touchZoomOne = Input.GetTouch(0);
				Touch touchZoomTwo = Input.GetTouch(1);
				if (touchZoomTwo.phase == TouchPhase.Moved && touchZoomOne.phase == TouchPhase.Moved)
				{
					// Find the position in the previous frame of each touch.
					Vector2 touchZeroPrevPos = touchZoomOne.position - touchZoomOne.deltaPosition;
					Vector2 touchOnePrevPos = touchZoomTwo.position - touchZoomTwo.deltaPosition;
					// Find the magnitude of the vector (the distance) between the touches in each frame.
					float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
					float touchDeltaMag = (touchZoomOne.position - touchZoomTwo.position).magnitude;
					// Find the difference in the distances between each frame.
					float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
					Controller.transform.Translate(0, (zoomSpeed * deltaMagnitudeDiff), 0, Space.World);
				}
			}
			else if (Input.touchCount == 1)
			{
				Touch touchPan = Input.GetTouch(0);
				if (touchPan.fingerId == 0 && touchPan.fingerId != 1 && touchPan.fingerId != 2 && touchPan.fingerId != 3)
				{
					if (touchPan.phase == TouchPhase.Moved)
					{
						Controller.transform.Translate(-touchPan.deltaPosition.x * panSpeed, 0, -touchPan.deltaPosition.y * panSpeed, Space.Self);
					}
				}
			}
		}
	}

	void Function_Computer_Controls()
	{
		if (Input.GetMouseButton(0))
		{
			transform.Translate(Input.GetAxis("Mouse X") * Time.deltaTime * panSpeed * -450,0, Input.GetAxis("Mouse Y") * Time.deltaTime * panSpeed * -450, Space.World);
		}
		if(Input.GetAxis("Mouse ScrollWheel") > 0f)                                                 // Zoom-Up Mouse Scroll
		{
			Controller.transform.Translate(0, (zoomSpeed * 9f), 0, Space.World);
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0f)                                                 // Zoom-Down Mouse Scroll
		{
			Controller.transform.Translate(0, (-zoomSpeed * 9), 0, Space.World);
		}
		if (Input.GetKey(KeyCode.W))																 // Zoom-Up KEYBOARD
		{
			Controller.transform.Translate(0, (zoomSpeed * 1.4f), 0, Space.World);		// NOTE: multiply base valuable into numbers, because it may
		}																			// affect sensor controls, THIS is computer controls
		else if (Input.GetKey(KeyCode.S))                                                            // Zoom-Down KEYBOARD
		{
			Controller.transform.Translate(0, (-zoomSpeed * 1.4f), 0, Space.World);
		}
		if (Input.GetKey(KeyCode.Q))																 // Y-Rotation Left
		{
			Cam.transform.Rotate(-Vector3.up * Time.deltaTime * rotSpeed * 30, Space.World);
		}
		else if (Input.GetKey(KeyCode.E))															 // Y-Rotation Right
		{
			Cam.transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed * 30, Space.World);
		}
		if (Input.GetKey(KeyCode.A))																 // X-Rotation Up
		{
			Cam.transform.Rotate(-Vector3.right * Time.deltaTime * angleSpeed * 25, Space.Self);
		}
		else if (Input.GetKey(KeyCode.D))															 // X-Rotation Down
		{
			Cam.transform.Rotate(Vector3.right * Time.deltaTime * angleSpeed * 25, Space.Self);
		}
	}

	void Function_Camera_Movement_Limits()						// !!! Sets Limits on Movement and Rotation of CAMERA !!!
	{
		if (transform.position.z >= panZMlimit)																		// Pan Z-Position Max Limit
		{
			Controller.transform.position = new Vector3(transform.position.x, transform.position.y, panZMlimit);
		}
		if (transform.position.z <= panZSlimit)                                                                     // Pan Z-Position Min Limit
		{
			Controller.transform.position = new Vector3(transform.position.x, transform.position.y, panZSlimit);
		}
		if (transform.position.x >= panXMlimit)                                                                     // Pan X-Position Max Limit
		{
			Controller.transform.position = new Vector3(panXMlimit, transform.position.y, transform.position.z);	
		}
		if (transform.position.x <= panXSlimit)                                                                     // Pan X-Position Min Limit
		{
			Controller.transform.position = new Vector3(panXSlimit, transform.position.y, transform.position.z);
		}
		if (Cam.transform.eulerAngles.x <= angleUpLimit)																// Y-Rotation Up Limit
		{
			Cam.transform.eulerAngles = new Vector3(angleUpLimit, transform.eulerAngles.y, transform.eulerAngles.z);
		}
		if (Cam.transform.eulerAngles.x >= angleDownLimit)                                                              // Y-Rotation Down Limit
		{
			Cam.transform.eulerAngles = new Vector3(angleDownLimit, transform.eulerAngles.y, transform.eulerAngles.z);
		}
		if (transform.position.y >= zoomUpLimit)																			// Zoom-Up Limit
		{
			transform.position = new Vector3(transform.position.x, zoomUpLimit, transform.position.z);
		}
		if (transform.position.y <= zoomDownLimit)																			// Zoom-Down Limit
		{
			transform.position = new Vector3(transform.position.x, zoomDownLimit, transform.position.z);
		}
	}
	
}
