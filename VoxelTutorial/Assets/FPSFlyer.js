



	var speed = 6.0;
	private var moveDirection = Vector3.zero;
	
	
	function FixedUpdate() {
		
		moveDirection = Input.GetAxis("Vertical") * new Vector3(transform.forward.x, Camera.main.transform.forward.y, transform.forward.z) * 10;
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= speed;
		
		var controller : CharacterController = GetComponent(CharacterController);
		var flags = controller.Move(moveDirection * Time.deltaTime);
		
	}
	
	@script RequireComponent(CharacterController)

