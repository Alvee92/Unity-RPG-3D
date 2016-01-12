var target : Transform;

var rotSpeed : float;

 

function Update(){

    var targetPos = target.position;

    targetPos.y = transform.position.y; //set targetPos y equal to mine, so I only look at my own plane

    var targetDir = Quaternion.LookRotation(-(targetPos - transform.position));

    transform.rotation = Quaternion.Slerp(transform.rotation, targetDir, rotSpeed*Time.deltaTime);

}