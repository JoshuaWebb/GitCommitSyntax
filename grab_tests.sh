#!/usr/bin/env bash

# grab the tests from master
for path in $(git ls-tree -r --name-only master -- tests); do
	# make sure the directory exsists
	mkdir -p "${path%/*}"
	# replace the .sublime-syntax line with .tmLanguage
	git show master:$path | sed -e 's/\.sublime-syntax/.tmLanguage/' > "$path"
done