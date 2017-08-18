import glob

fileList = glob.glob('./**/*.cs', recursive = True)
print(len(fileList), " files.")

rows = 0
for file in fileList:
    with open(file, 'r') as f:
        rows += sum(1 for line in f)
        
print(rows)
