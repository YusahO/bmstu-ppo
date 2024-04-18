import sys

filename = sys.argv[1]

with open(filename, 'r') as f:
    contents = f.readlines()

usings = []
last_using = 0
for l in contents:
    if l.startswith('using'):
        usings.append(l)
        last_using += 1

contents = contents[last_using:]
usings.sort()

print(*usings, sep='')