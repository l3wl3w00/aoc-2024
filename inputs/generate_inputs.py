import os

files_to_create = [
    "part1.input.txt",
    "part1.testinput.txt",
    "part2.input.txt",
    "part2.testinput.txt",
]

for day in range(1, 26):
    dir_name = f"day{day}"
    os.makedirs(dir_name, exist_ok=True)
    for f in files_to_create:
        if os.path.exists(f):
            with open(f, 'w'): pass