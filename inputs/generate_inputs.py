import os

files_to_create = [
    "input.txt",
    "part1.testinput.txt",
    "part2.testinput.txt",
]

for day in range(1, 26):
    dir_name = f"day{day}"
    os.makedirs(dir_name, exist_ok=True)
    for f in files_to_create:
        file_name = f"{dir_name}/{f}"
        if not os.path.exists(file_name):
            with open(file_name, 'w'): pass