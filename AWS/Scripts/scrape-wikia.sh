#!/bin/bash

# Read the text file line by line
while IFS= read -r url
do
    # Use wget to download the file
    wget "$url"
done < urls.txt

