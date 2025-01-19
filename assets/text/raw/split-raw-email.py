import sys
import os

OUTFILE = "output/email-"
def main(argv: list[str]) -> None:
    filename = argv[1]

    os.system("mkdir -p output")
    try:
        with open(filename, "r") as f:
            lines = f.readlines()
            file_count = 0
            for line in lines:
                if line == "========\n":
                    file_count += 1
                    continue
                with open(f"{OUTFILE}{file_count:02}.txt", "a") as output_file:
                    output_file.write(line)
    except FileNotFoundError:
        print(f"File {filename} not found.", file=sys.stderr)

if __name__ == "__main__":
    main(sys.argv)