# File emitter mod

# Video example
https://www.youtube.com/watch?v=m93YOQSrMnk
(note: the vid was before I added in endianness so it was spitting out most significant bit first, default is now least significant bit first)

## summary 
This Logic World mod adds a file emitter component which reads the bytes of a file on your system and spits out one bit at a time as it receives clock signals (via the top input).

The main output is the current bit (stays active until next clock signal).

The side output activates when EOF is reached.

## Configuration
After placing a file emitter, you will see it's ID on it's side. You will use this ID in commands.

The server commands (you have to type `server "{{command and arguments go here}}"`):
- `fem-setpath {{emitterId}} {{/path/to/your/file}}`

  This links up the file emitter to a file on your system. This is only temporary / the state is not saved with your world.

  E.g. `server "fem-setpath FE1 /home/bob/myfile.whatever"`

- `fem-setflags {{emitterId}} {{flags}}`
  Flags should be a number, which represents 0 or more flags. The individual flags are...
  - 1: the reset flag. When this flag is set, the next clock signal will reset the read offset to 0, and the clock signal after that one will read the first bit.
  - 2: big endian flag. When this flag is set, the most significant bit will be emitted first for each byte.

  Flags are powers of two so you can combine them, for example using "3" sets both of the above flags.

- emitterId is case insensitive in both of the above commands

# issues
- currently you have to actually remove and replace file emitter to change the file after it has been reading from another file, or it will keep reading the old file

TODO:
- Lots
- Persisting state was giving me trouble, so I decided to avoid it altogether. Persisting file paths and offsets would make QOL better.
- X panel input for filename
- flags setting via command is clunky, this is just a temporary solution for resetting / changing endianness
