# File emitter mod

## summary 
This Logic World mod adds a file emitter component which reads the bytes of a file on your system and spits out one bit at a time as it receives clock signals (via the top input).

The main output is the current bit (stays active until next clock signal).

The side output activates when EOF is reached.

## Configuration
- Use the component edit menu ('X') to update the path name. This path should contain no special characters of course.

## Data Sheet
- The inputs:
  - CLOCK: The tall pin on top is the clock pin. Send a signal to this pin to make the file emitter emit the next bit (or reset if the RESET pin is also active.)
  - RESET: The medium pin by CLOCK and the side with the EOF output is the reset flag. If it is active when CLOCK pin is signaled, the internal byte buffer gets wiped away and the file will be reloaded on the following CLOCK signal. You need to do this after you change the file path or to read the file again after you have reached EOF.
  - BIG ENDIAN: The medium pin on the other side of CLOCK is a flag to control whether least significant bit is read first (default) or most significant bit first (keep this pin active for most significant first).
- Outputs:
  - The centered output on wide side is the current bit (active = 1, inactive = 0)

## Issues
- Make sure to remove any old version of the file-emitter block from your world before you update the mod! This update has breaking changes.

## TODO:
- 8 bit output variant

## Video example
https://www.youtube.com/watch?v=m93YOQSrMnk
(Note: The vid was before I added in endianness so it was spitting out most significant bit first, default is now least significant bit first. It's also missing the new input pins.)
