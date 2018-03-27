# alternate-platform

This is the repository for an easy-to-use Unity package for networking HTC Vives. Check out "Using alternate-platform" to get started or if you are having issues check out the "Wait, why isn't ___ working?" section below

## Using alternate-platform
1. Setting up PUN
  * Download Photon Unity Networking from the Asset Store
  * Open the PUN Wizard (Window > Photon Unity Networking)
  * Register to get your AppID
  * Copy that appID into the related field in the Inspector
  
2. Add NetworkManager.cs to your scene in a Network Manager Empty GameObject
  * See the comments in the script for descriptions of how the script works with Photon 
  * Set spawn points as empty gameobjects in your scene and add them along with the gameobjects for the headset and controllers to the script's public gameobjects
  
3. Creating a gameobject that can be moved around by all players

  * Create a gameobject and add:
    - rigidbody (that is kinematic)
    - Transform Manager script
    - Photon View script with:
      * Owner: Takeover 
      * Observe option: Reliable Delta Compressed
      * Observed Components: Transform Manager Script
      
4. A gameobject that can be instantiated by a player and then interactable for all players

  * Follow step 3 then put the gameobject in Assets->Resources so it can be instantiated in the Photon Network
  * In InputManager script you can see the following line that creates a prefab in the Photon Network:
     > PhotonNetwork.Instantiate(spherePrefab.name,new Vector3(0,3,0), Quaternion.identity, 0);

## Wait, why isn't ___ working?

#### My room isn't showing on another Unity build/game view
- If you want to join the same room, also make sure that the app id on both games are the same

#### My Object isn't showing in the room
- make sure you have attached the Photon View Script
- make sure Photon View is observing Transform Manager script (or the script you have created that has OnPhotonSerializeView())
- make sure the observe option is set to "Reliable Data Compressed"

#### My object is there but it won't move || My object only moves on one build || My object moves but then goes back to it's original position

- make sure "Is Kinematic" is checked
- make sure Owner on Photon View is set to "Takeover"
- once again, make sure you are observing the Transfrom Manager script
- make sure your script has the line:
 >          ~GameObjectYouWishToInteractWith~.GetComponent<PhotonView>().RequestOwnership();
  * this line will give you ownership and after that you can call the function that will interact with the object (Seen in Input Manager script)
  
#### The function I wrote only changes the object on one build
- make sure the functions you write to interact with objects are (like in Transform Manager) using: [PunRPC]
- make sure that inside your [PunRPC] function you are also checking to run the function again if photonView.isMine (seen in Transform Manager)

#### There is an error with my PunRPC function
- make sure when you call photonView.RPC(); you always feed it three arguments.
    * First argument: the name of the RPC function you wish to call i.e. "SetNewParent" (using quotation marks)
    * Second argument: PhotonTargets.OthersBuffered
    * Third Argument: any data you need to input to the function i.e. a transform, a color, etc. OR if you dont have any data to feed the function simply put photonView.viewID
    * (Seen in Transform Manager - detachParent() uses photonView.viewID while the others input data to the functions)

#### My object is jumping around like crazy when I try to interact with it
- make sure that you are _only_ observing the Transform Manager script



## Our Workflow Timeline

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



