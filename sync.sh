#!/usr/bin/env bash

# you can pass in the branch as the first argument if you want...
branch=${1:-master}

# Update the theme (only grab the compiled theme)
git checkout $branch -- theme/Git-Material-Theme.tmTheme

# grab the tests
for path in $(git ls-tree -r --name-only $branch -- tests); do
	# make sure the directory exsists
	mkdir -p "${path%/*}"
	# replace the .sublime-syntax line with .tmLanguage
	git show $branch:$path | sed -e 's/\.sublime-syntax/.tmLanguage/' > "$path"
done
