# sst
Secure Serial Tunnel

## About

sst is a program that lets a user access an SSH session from a serial text terminal.
The initial use case was to tunnel to an SSH server running GNU/Linux from a VT420 serial terminal.
When the program is started, the user enters an SSH host name and password, selects a serial port to use, and hits the "Connect" button.
A shell session then appears on the text terminal, and the user can interact with the shell from there.
When finished, the user hits the "Disconnect" button to end the SSH session.

## Requirements

To build the app:
- A Windows workstation
- Visual Studio Community 2019
- .NET Framework 4.7.2

To run the app:
- A Windows workstation
- .NET Framework 4.7.2
- A serial port (A USB to RS-232 adapter works well.)
- A serial terminal and cable (A VT420 was used during development.)
- An SSH host to connect to

## Work Needed

The following work needs to be done to improve functionality.
1. Better error handling.
2. Save/load support.  Secure storage of SSH host information and serial terminal configuration would make this easier to use.
3. SSH public key certificate support.  This would allow for use of SSH in more situations.
