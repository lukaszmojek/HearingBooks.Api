#textExtraction
# Clean PDF text extraction
This is more complex version of the [[PhotoTextExtraction]].
Firstly the PDF is going to be separated into pages, so it will be broken down to image files. Once that is achieved, then [[PhotoTextExtraction]] will be used to extract the text from all of the image files, and put them into the **.txt** files containing text for individual images.
As a last step, all of the text files will be conmbined into one, keeping in mind the actual order of the pages and voila, text has been extracted successfully.

---

For now it was made as a proof of concept, that it actually can be done using those steps

To split the PDF into the images, running
`pdftoppm -png {name}.pdf {name_of_output_files}`

Then executing this script
```bash
#!/bin/bash

for file in $(ls -a | grep ".png")
do
	echo $file
	tesseract $file $file -l pol
done
```

Finally, running this command
`cat {name}.txt > complete.txt`

---

The process overall seems simple, but containting that functionality inside a C# application placed in docker conatiner might not be as easy as it sounds.