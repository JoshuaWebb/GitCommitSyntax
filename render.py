import os
import re

fragments_directory = 'fragments'
template_file = 'GitCommitMessage.sublime-syntax.tpl'
rendered_path = os.path.splitext(template_file)[0]

# load all fragments
fragments = {}
for f in [i for i in os.listdir(fragments_directory)]:
    with open(os.path.join(fragments_directory, f)) as file_object:
        fragments[f] = [line for line in file_object]

rendered = open(rendered_path, 'w')
rendered.truncate()

# Template lines are regular yaml, or follow the pattern
# - __fragment-name.yaml__: new_context
template_pattern = r'''(\s+)- __([^_]+)__: (.*)'''
template_regex = re.compile(template_pattern)
with open(template_file) as f:
	for line in f:
		m = template_regex.match(line)
		if m:
			indentation  = m.group(1)
			fragment     = m.group(2)
			next_context = m.group(3)

			lines = [indentation + l.replace('$set', next_context) for l in fragments[fragment]]
			text = ''.join(lines)
		else:
			text = line

		rendered.write(text)
