# OCR
In order to allow converting photos of books/texts to be synthetized it is was needed to use OCR for extracting the text from **photos** or **pdfs** without the text separated out.

# Tesseract-OCR
Since it is a great and open-source OCR engine working flawlessly on Linux, it was decide to use it as an engine.
The idea here is, that there will be separate service running in docker container, where **tesseract-ocr** and its sub-packages for concrete languages are installed.

Application will have predefined languages to select from as docker image should be built with those concrete packages in mind, there is probably no way to do that dynamically (check that either way).

For now Tesseract-OCR (will) be used as a utility for
- [[PhotoTextExtraction]]
- [[PhotoSyntesis]] 
- [[CleanPdfTextExtraction]]
- [[CleanPdfSynthesis]]

