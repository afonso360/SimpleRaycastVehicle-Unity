using UnityEditor;
using UnityEngine;

public class CarSetup : ScriptableWizard
{
	//public float range = 500;
	//public Color color = Color.red;
	public string CarName = "New car";
	public bool ReplaceColider = true;

	[Header("Car Parts")]
	public GameObject CarBody;
	public GameObject WheelFR;
	public GameObject WheelFL;
	public GameObject WheelBR;
	public GameObject WheelBL;

	[Header("Wheel Defualt Parameters")]
	public float wheelMass = 1f;
	public float WheelMaxSuspension = 0.04f;
	public float WheelSpring = 8000f;
	public float WheelDamper = 500f;

	[Header("Rigidbody Parameters")]
	public float RigidbodyMass = 100f;
	public float RigidbodyDrag = 0.2f;
	public float RigidbodyAngularDrag = 10f;




	[MenuItem("GameObject/Setup Car")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard<CarSetup>("Create Car", "Create", "Apply");
		//If you don't want to use the secondary button simply leave it out:
		//ScriptableWizard.DisplayWizard<WizardCreateLight>("Create Light", "Create");
	}

	void OnWizardCreate()
	{
		if(CarBody==null ||WheelFR==null||WheelFL==null||WheelBR==null||WheelBL==null  ){
			Debug.LogError ("Drag car parts to place");
			return;
		}

		RaycastWheelSimple RCWheelScript;
		Mesh WheelMesh;
		Bounds WheelBounds;

		GameObject CarRoot = new GameObject(CarName);
		CarBody.transform.SetParent (CarRoot.transform);
		PlayerCar PlayerCarScript = CarRoot.AddComponent<PlayerCar> ();
		PlayerCarScript.LFWheelTransform = WheelFL.transform;
		PlayerCarScript.RFWheelTransform = WheelFR.transform;
		// set up rigidbody
		Rigidbody CarRB = CarRoot.AddComponent<Rigidbody> ();
		CarRB.mass = RigidbodyMass;
		CarRB.drag = RigidbodyDrag;
		CarRB.angularDrag = RigidbodyAngularDrag;



		// replace collider
		if (ReplaceColider) {
			MeshCollider Meshcoll =  CarBody.GetComponent<MeshCollider> ();
			if (Meshcoll != null)
				DestroyImmediate (Meshcoll);
			CarBody.AddComponent<BoxCollider> ();

		}
		GameObject FR = new GameObject("WheelFR");
		FR.transform.SetParent (CarRoot.transform);
		FR.transform.position = WheelFR.transform.position;
		WheelFR.transform.SetParent (FR.transform);
		PlayerCarScript.frontRightWheel = WheelFR.transform;
		RCWheelScript = FR.AddComponent<RaycastWheelSimple> ();
		RCWheelScript.graphic =  WheelFR.transform;
		WheelMesh = WheelFR.GetComponent<MeshFilter> ().sharedMesh;
		WheelBounds = WheelMesh.bounds;

		SetParameterss (RCWheelScript,WheelMesh.bounds.extents.y);

		GameObject FL = new GameObject("WheelFL");
		FL.transform.SetParent (CarRoot.transform);
		FL.transform.position = WheelFL.transform.position;
		WheelFL.transform.SetParent (FL.transform);
		PlayerCarScript.frontLeftWheel = WheelFL.transform;
		RCWheelScript = FL.AddComponent<RaycastWheelSimple> ();
		RCWheelScript.graphic =  WheelFL.transform;
		WheelMesh = WheelFL.GetComponent<MeshFilter> ().sharedMesh;
		WheelBounds = WheelMesh.bounds;
		SetParameterss (RCWheelScript,WheelMesh.bounds.extents.y);

		GameObject BR = new GameObject("WheelBR");
		BR.transform.SetParent (CarRoot.transform);
		BR.transform.position = WheelBR.transform.position;
		WheelBR.transform.SetParent (BR.transform);
		PlayerCarScript.rearRightWheel = WheelBR.transform;
		RCWheelScript = BR.AddComponent<RaycastWheelSimple> ();
		RCWheelScript.graphic =  WheelBR.transform;
		WheelMesh = WheelBR.GetComponent<MeshFilter> ().sharedMesh;
		WheelBounds = WheelMesh.bounds;
		SetParameterss (RCWheelScript,WheelMesh.bounds.extents.y);

		GameObject BL = new GameObject("WheelBL");
		BL.transform.SetParent (CarRoot.transform);
		BL.transform.position = WheelBL.transform.position;
		WheelBL.transform.SetParent (BL.transform);
		PlayerCarScript.rearLeftWheel = WheelBL.transform;
		RCWheelScript = BL.AddComponent<RaycastWheelSimple> ();
		RCWheelScript.graphic =  WheelBL.transform;
		WheelMesh = WheelBL.GetComponent<MeshFilter> ().sharedMesh;
		WheelBounds = WheelMesh.bounds;
		SetParameterss (RCWheelScript,WheelMesh.bounds.extents.y);

	}

	void SetParameterss(RaycastWheelSimple _RCWScript,float _radius){		
		_RCWScript.mass = wheelMass;
		_RCWScript.maxSuspension = WheelMaxSuspension;
		_RCWScript.spring = WheelSpring;
		_RCWScript.damper = WheelDamper;
		if(_radius!=-1f) _RCWScript.radius = _radius;
	}

	void OnWizardUpdate()
	{
		helpString = "Drag Car parts to place and press CREATE for new car setup. \n\nSelect car and pres APPLY to for parms changes";
	}

	// When the user pressed the "Apply" button OnWizardOtherButton is called.
	void OnWizardOtherButton()
	{
		if (Selection.activeTransform != null)
		{	
			RaycastWheelSimple[] allWheels = Selection.activeTransform.GetComponentsInChildren<RaycastWheelSimple> ();
			if (allWheels == null || allWheels.Length==0) {
				Debug.LogError ("Select Car Root object!");
			} else {
				// setup wheel script params
				for (int i = 0; i < allWheels.Length; i++)
				{
					SetParameterss (allWheels[i],-1f); 
				}
				// set rigidbody
				Rigidbody TempCarsRB =  Selection.activeTransform.GetComponent<Rigidbody>();
				if (TempCarsRB != null) {
					TempCarsRB.mass = RigidbodyMass;
					TempCarsRB.drag = RigidbodyDrag;
					TempCarsRB.angularDrag = RigidbodyAngularDrag;
				} 

			}
		}
	}
}