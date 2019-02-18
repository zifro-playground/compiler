# Script to generate Unity meta files for the current folder structure
# Created by Kalle Jillheden (github.com/jilleJr)

import os

topfolder = os.path.dirname(__file__)

for subdir,dirs,files in os.walk(topfolder, topdown=True):
    for dir in dirs:
        print("dir:", dir)