# JHU/APL pubgeo
JHU/APL is working to help advance the state of the art in geospatial computer vision by developing public benchmark data sets and open source software. 
For more information on this and other efforts, please visit [JHU/APL](http://www.jhuapl.edu/pubgeo.html).

## WVCal
WorldView Radiometric Calibration Tool

Performs radiometric calibration to top of atmosphere radiance for WorldView NITF images and saves resulting images in uncompressed ENVI format. Intended to perform the same function as ENVI's built-in calibration, but at far faster speeds.
	
Calibration info is extracted from the NITF file and an accompanying IMD file. The IMD file must have the same base file name as the NITF. If a tar file, rather than IMD, with the NITF base name is present, it will be searched automatically for the first IMD file within it.

WVCal can be run as either a console command or with a GUI. Running with no parameters starts the GUI. Specifying -help displays usage info.
	
Supported formats include:
* WorldView 2 Pan
* WorldView 2 MS 4-band or 8-band
* WorldView 3 Pan
* WorldView 3 MS 4-band or 8-band
* WorldView 3 SWIR 8-band
* (other formats may be supported but are untested)

## WVCal Dependencies
WVCal is provided as a visual studio 2010 project, compiling requires at least .NET Framework 4.0.

WVCal depends on GDAL for its operation. The Dependencies folder has a quick guide for downloading and 'installing' a compatible version of GDAL for compiling this project.

## WVCal usage

### Command-line arguments
    WVCal -input file -output file [-imd file] [-outputdatatype type] [-scalefactor factor] [-applyfinetuningcalibration true/false]

#### Options
* **-input file** NITF file to be calibrated.
* **-output file** Name of ENVI file to be saved. A header file with file + ".hdr" will also be created.
* **-imd file** IMD file to be used for image calibration. Default is the input NITF file name, but with a .IMD extension. If not present, a tar file with the same name will be searched for the first IMD file within.
* **-outputdatatype type** Supported types are 4 (float32) or 12 (uint16). Default is 12.
* **-scalefactor factor** All output values will be multiplied by factor before saving. This is recorded in the ENVI header as radiance_scale_factor. Default is 1.
* **-applyfinetuningcalibration** Applies additional fine-tuning parameters for better calibration to WV2 and WV3 images. Values are from DigitalGlobe technical document ABSRADCAL_FLEET_2016v0_Rel20170403.pdf. Default is true.

#### Examples
    WVCal.exe -input WV03Image.NTF -output WV03Image_cal
    WVCal.exe -input WV03Image.NTF -output WV03Image_cal -outputdatatype 12 -scalefactor 100
