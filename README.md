pinger
======
A Better Ping...IMHO

This is a simple command-line tool written in C# with the help of 
Chris Sells' excellent command line parser class to ping a range of 
IP Addresses.

Usage
-----
The command line parameters of the tool are as follows:

```
Usage: Pinger.exe [@argfile] <startingAddress> [endingAddress] [/r:<value>]
       [/help|?|h] [/version|v]


@argfile         Read arguments from a file.

                 - Addresses -
startingAddress  starting address
endingAddress    ending address (Default is "")

                 - Behavior -
/r:<value>       repeat (Default is "1")

                 - HELP -
/help            Show usage.
/version         Show version.

```
