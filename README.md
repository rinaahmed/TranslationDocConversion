# Document Conversion of Translation Documents to Text Files

This project converts specific translation documents with extensions, such as .DEU .ENU and converts them to [_de | _en].ALIGN documents. 

The program takes two arguments:
- _SourceFolder_: a folder containing .DEU and .ENU files
- _DestinationFolder_: the folder containing the resulting text documents.

The folder structure within the _SourceFolder_ will be recreated in the _DestinationFolder_.

## Usage:
    Compile the program in Visual Studio and run the Program.exe file from the bin directory.

    From command line, run 
    `Program <SourceFolder> <DestinationFolder>'

    If you run directly from Visual Studio, please make sure you update the two argument parameters for the folder in the Project Settings.


The programm has been written by David Zeller. 

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
