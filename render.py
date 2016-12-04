rendered = open('GitCommitMessage.sublime-syntax', 'w')
rendered.truncate()

import os

# load all fragments
file_dict = {}
for f in [i for i in os.listdir('fragments')]:
    # Open them and assign them to file_dict
    with open(os.path.join('fragments', f)) as file_object:
        file_dict[f] = [line for line in file_object]

import re

with open('GitCommitMessage.sublime-syntax.tpl') as f:
	for line in f:
		m = re.match('''(\s+)- __([^_]+)__: (.*)''', line)
		if m:
			lines = [m.group(1) + l.replace('$set', m.group(3)) for l in file_dict[m.group(2)]]
			text = ''.join(lines)
		else:
			text = line

		rendered.write(text)
