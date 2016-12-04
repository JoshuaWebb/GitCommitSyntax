%YAML 1.2
---
name: Git Commit Message
file_extensions: [COMMIT_EDITMSG, MERGE_MSG, TAG_EDITMSG]
scope: text.git-commit-msg
uuid: BD99186C-6F05-4A89-9BBC-B6AFB205B7BC

variables:
    blank_line: ^\s*$
    empty_comment: ^#\s*$

contexts:
  prototype:
    - include: comments

  main:
    # blank lines before the first line are ignored.
    - match: '{{blank_line}}'
    - __committed-changes.yaml__: committed-changes-main
    - __unstaged-changes.yaml__: unstaged-changes-main
    - __first-line.yaml__: second-line
    - include: plain-comment

  committed-changes-main:
    - meta_content_scope: text.git-commit-msg.committed
    - include: changes-main
    # if we get the first line in the middle of the changes
    - __first-line.yaml__: committed-changes-after-first

  unstaged-changes-main:
    - meta_content_scope: text.git-commit-msg.unstaged
    - include: changes-main
    # if we get the first line in the middle of the changes
    - __first-line.yaml__: unstaged-changes-after-first

  changes-main:
    # blank lines before the first line are ignored.
    - match: '{{blank_line}}'
    - include: changes
    # end of changes (we haven't had first line yet)
    - __end-changes.yaml__: main
    - include: plain-comment

  committed-changes-after-first:
    - meta_content_scope: text.git-commit-msg.committed
    - __second-line.yaml__: committed-changes-body
    - include: changes-after-first

  unstaged-changes-after-first:
    - meta_content_scope: text.git-commit-msg.unstaged
    - __second-line.yaml__: unstaged-changes-body
    - include: changes-after-first

  changes-after-first:
    - include: changes
    # changes ended, we've had the first line, but we're waiting on the second line
    - __end-changes.yaml__: second-line
    - include: plain-comment

  committed-changes-body:
    - meta_content_scope: text.git-commit-msg.committed
    - include: changes-body

  unstaged-changes-body:
    - meta_content_scope: text.git-commit-msg.unstaged
    - include: changes-body

  changes-body:
    - include: changes
    # changes ended, and we've had the first and second line
    - __end-changes.yaml__: body
    - include: body

  second-line:
    - __committed-changes.yaml__: committed-changes-after-first
    - __unstaged-changes.yaml__: unstaged-changes-after-first
    - __second-line.yaml__: body
    - include: plain-comment

  body:
    - __committed-changes.yaml__: committed-changes-body
    - __unstaged-changes.yaml__: unstaged-changes-body
    - match: ^(?!#)(?:.{1,72})(.*)
      scope: text.git-commit-msg.body
      captures:
        "1": invalid.illegal.too-long
    - include: plain-comment

  plain-comment:
    # Plain ol' comment
    - match: (^#.*$)
      scope: comment.line.number-sign

  comments:
    # TODO: see if we can use meta_scope: comment.line.number-sign
    - match: ^#\sOn branch (.*)
      scope: comment.line.number-sign
      captures:
        "1": string.unquoted.branch-name
    - match: ^#\sYour branch is (?:(ahead) of|(behind)) '(.*)' by (\d+) commit(?:s?(?:\.|, and can be fast-forwarded\.))
      scope: comment.line.number-sign
      captures:
        "1": keyword.other.status.ahead
        "2": keyword.other.status.behind
        "3": string.unquoted.branch-name.remote
        "4": constant.numeric
    - match: ^#\sYour branch is (up-to-date) with '(.*)'\.
      scope: comment.line.number-sign
      captures:
        "1": keyword.other.status.up-to-date
        "2": string.unquoted.branch-name.remote
    - match: ^#\sYour branch and '(.*)' have (diverged),
      scope: comment.line.number-sign
      captures:
        "1": string.unquoted.branch-name.remote
        "2": keyword.other.status.diverged
    - match: ^# and have (\d+) and (\d+) different commits each, respectively.
      scope: comment.line.number-sign
      captures:
        "1": constant.numeric
        "2": constant.numeric

    - match: ^#\s(Author:)\s+(.*)\s<([^>]+)>
      scope: comment.line.number-sign
      captures:
        "1": keyword.other.author
        "2": string.unquoted.name
        "3": string.unquoted.email

    - match: ^#\s(Date:)\s+(.*)
      scope: comment.line.number-sign
      captures:
        "1": keyword.other.date
        "2": string.unquoted.date

    - match: ^#\s(Changes not staged for commit:)
      scope: comment.line.number-sign
      captures:
        "1": markup.heading
      push:
        - meta_content_scope: text.git-commit-msg.unstaged
        - include: changes
        - match: ^#\s*$
          scope: comment.line.number-sign
          pop: true

    - match: "^# (Untracked files:)"
      scope: comment.line.number-sign
      captures:
        "1": markup.heading
      push:
        - meta_include_prototype: false
        - match: ^#\s+(\S.*)$
          scope: comment.line.number-sign
          captures:
            "1": markup.list.unnumbered.untracked
        - match: ^#\s*$
          scope: comment.line.number-sign
          pop: true

    - match: "^# (Conflicts:)"
      scope: comment.line.number-sign
      captures:
        "1": markup.heading
      push:
        - meta_include_prototype: false
        - match: ^#\s+(\S.*)$
          scope: comment.line.number-sign
          captures:
            "1": markup.list.unnumbered.resolved-conflict
        - match: ^#\s*$
          scope: comment.line.number-sign
          pop: true
    - match: ^# If this is not correct, please remove the file
      scope: comment.line.number-sign
      push:
        - meta_include_prototype: false
        - match: ^#\s+and try again\.$
          scope: comment.line.number-sign
          pop: true
        - match: ^#\s+(\S.*)$
          scope: comment.line.number-sign
          captures:
            "1": string.unquoted.merge-head

    # diff cut line
    - match: ^#\s+------------------------ >8 ------------------------
      scope: comment.line.number-sign meta.diff.marker
      push:
        # explanation comment lines
        - match: (^#.*$)
          scope: comment.line.number-sign
        - match: ^(?!#)
          # doesn't use comment scope
          set: "Packages/Diff/Diff.sublime-syntax"

  new-file:
    - meta_include_prototype: false
    - match: ^#\s+(new file:)\s+(.*)
      scope: comment.line.number-sign
      captures:
        "1": keyword.other.new-file
        "2": markup.inserted

  modified:
    - meta_include_prototype: false
    - match: ^#\s+(modified:)\s+(.*)
      scope: comment.line.number-sign
      captures:
        "1": keyword.other.modified
        "2": markup.changed

  renamed:
    - meta_include_prototype: false
    # split on the the first ' -> ', if the filename has
    # ' -> ' in it, may god have mercy on your soul.
    - match: ^#\s+(renamed:)\s+(.*?)(?:\s(->)\s)(.*)
      scope: comment.line.number-sign
      captures:
        "1": keyword.other.renamed
        "2": markup.changed
        "3": punctuation.operator
        "4": markup.inserted

  deleted:
    - meta_include_prototype: false
    - match: ^#\s+(deleted:)\s+(.*)
      scope: comment.line.number-sign
      captures:
        "1": keyword.other.deleted
        "2": markup.deleted

  changes:
    - include: new-file
    - include: modified
    - include: renamed
    - include: deleted
