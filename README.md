# PolarisGround - UGCS3
Custom ground control station implemented in c#

Copyright (C) 2017 Hery A Mwenegoha.

Works with PolarisAir.

## Setup and Functions
Tested on VS2017 and VS2015.

Radio (RFD900 tested) interface at user's preffered baudrate (recommended 57600) to the FCS, live telemetry on display including UAV location.

Pre-define waypoints and send to UAV easily, adjust waypoints on the fly based on need. Use guided mode to implement point-to-fly. Log data locally for debugging etc. 

Hardware-in-Loop interface with Xplane10 and the FCS. 

## Work
1. Some bug fixes 
  1.1. splash screen load up issues
  1.2. packet loss over small distances (rfd900 radios)
  1.3. Optimise logger
2. Optimisation...

Still a long way from having a fully functioning ground control station and flight control sytsem, let me know if you would like to help.
