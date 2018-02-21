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
