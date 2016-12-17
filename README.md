# Commit Message Syntax (TextMate Versions)

This branch contains a port of all the languages

## Developing

Run `./sync.sh` to update the shared files (tests and theme).

Generate the tmLanguage from the YAML-tmLanguage via

	Ctrl+Shift+P -> PackageDev: Convert (YAML, JSON, PList) to...

Then run tests on the generated language with
	
	Ctrl+Shift+P -> Build With: Syntax Tests

`F7` thereafter
