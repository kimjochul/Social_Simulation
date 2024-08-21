Introduction:

  XR Animator supports VMC protocol, including camera support which allows sending avatar transform as well as camera data to external app such as Warudo. However, Warduo doesn't support VMC camera by default, so you will need to install a OSC plugin and a custom blueprint to use VMC camera on Warudo.

--------------

Steps:

  On Warudo side:

    1. Install "OSC Input Node" from Steam workshop.
       https://steamcommunity.com/sharedfiles/filedetails/?id=3006445377

    2. Import the file "VMC Camera.json" as blueprint.

    3. Choose an avatar (preferably the same VRM model as XR Animator).

    4. Use VMC for both face and pose tracking, and enter 39539 as port number (or any port number used by XR Animator) for "VMC Receiver" asset.

  On XR Animator side:

    1. Double-click "VMC-protocol", and change "App mode" to "Warudo".

    2. Change "VMC-protocol" to ON.

    2. Change "Send camera data" to ON if you want VMC camera.

    4. If everything works, you should see the avatar on Warudo side mirroring the motion from XR Animator, along with the 3D camera.

--------------

Tips:

  By default, Warudo smoothens mocap data received from VMC. However, this will decrease the response time of the avatar, especially for fast motion. If your PC is fast enough to run XR Animator at a high enough frame rate, you may want to edit the following blueprints

    * Face Tracking - VMC
    * Pose Tracking - VMC
    * VMC Camera

  and reduce "Smooth Time" of the following nodes to around 0.2

    * Smooth Blendshape List
    * Smooth Position List
    * Smooth Rotation List
    * Smooth Transform

  If you use VMC camera as well, you are recommended to use the same "Smooth Time" value across the above nodes, especially position/rotation/transform if you want a better synchronized camera.

--------------

Issues:

  - If you are prompt for firewall settings, make sure you allow both XR Animator and Warudo to communicate on the network.

--------------

Warudo:

  https://warudo.app/


