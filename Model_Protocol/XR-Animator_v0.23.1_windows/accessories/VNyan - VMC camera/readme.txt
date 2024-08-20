Introduction:

    XR Animator supports VMC protocol, including camera support which allows sending avatar transform as well as camera data to external app such as VNyan. "VMC_camera_for_VNyan.vnprop" is the prop file necessary for XR Animator to connect to VNyan via VMC-protocol with camera support.

--------------

Steps:

  On VNyan side:

    1. Choose an avatar (preferably the same VRM model as XR Animator).

    2. Click "Props", "Add Prop" and then select "VMC_camera_for_VNyan.vnprop".

    3. Choose "Tracker 1" (or basically any tracker slot) as "Linked Bone".

    4. Click "Settings", "General settings", pick a VMC Receiver and enter 39539 as port number (or any port number used by XR Animator).

    5. Click "VMC Tracker Mapping", choose a tracker slot to edit ("Tracker 1" by default), and enter "Camera" as name.

  On XR Animator side:

    1. Double-click "VMC-protocol", and change "App mode" to "VNyan".

    2. Change "VMC-protocol" to ON.

    2. Change "Send camera data" to ON if you want VMC camera.

    4. If everything works, you should see the avatar on VNyan side mirroring the motion from XR Animator, along with the 3D camera.

--------------

Issues:

  - If you are prompt for firewall settings, make sure you allow both XR Animator and VNyan to communicate on the network.

  - VNyan does not yet support the fov parameter for 3D camera.

--------------

VNyan download page:

  https://suvidriel.itch.io/vnyan
