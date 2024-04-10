import os
import re

for d in os.listdir():
    if not d.endswith('.cs'):
        continue
    with open(d, 'r') as f:
        print('\n'+d)
        method_names = []
        p = re.compile('')
        for l in f:
            if l.startswith('    '):
                l = l.strip().split()
                l = l[1][:l[1].find('(')]
                method_names.append(f'+ {l}()')
        print(*method_names, sep='\n')