# alternate-platform

This is the repository for an easy-to-use Unity package for networking HTC Vives.

### 02/19
- in order to have two computers on the same network, you need to build it from the same project.
- you need to instantiate the camera rig at runtime in your local side
- you need to photon instantiate the display object as a child of Camera (eye)
- photonview.cs needs to be on the display object and needs to watch the script that has OnPhotonSerialize() (no need to watch for transform)


### 02/21
- Make sure to check "Is Kinematic" on your display object to keep it locked to it's parent
- Display objects for hands created
- You need to have empty game objects for different spawn points
- spawn points are added to the array and based on Photon Network countOfPlayers, a user will be added to the room at a unique spawn point

### 02/28 
- You need to request ownership in order to call functions on an object that was there at the beginning.
- You don't need to approve ownership, it is approved by default.
- To sync objects across players, you need to use Pun RPCs.

### 03/04
- Pick up an object with controllers and sync across players.
- Photonview should view the script and not the transform. (This fixes flickering problem)
- Have all the PunPRC scripts in the same script ideally. Have a network manager that manages the PunRPC scripts.
- Always have the third argument for the PhotonView.RPC function

### 03/21
- Clean up scripts, documentation
- Make sure Owner on Photon View Script is set to "Takeover" NOT fixed when working with an object you want all players to interact with


