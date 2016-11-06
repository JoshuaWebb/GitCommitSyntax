#!/usr/bin/env bash

# grab the tests from master
for path in $(git ls-tree -r --name-only master -- tests); do
	# make sure the directory exsists
	mkdir -p "${path%/*}"
	# replace the .sublime-syntax line with .tmLanguage
	git show master:$path | sed -e 's_\(SYNTAX TEST "Packages/GitCommitSyntax/GitCommitMessage\)\.sublime-syntax_\1.tmLanguage_' > "$path"
done