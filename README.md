# Soulsborne-Animconverter
Parses HKX files to cut down on time spent converting animations to damnhavok.

Fixes root node in the relevant Havok XML files, converts via HKXCMD, creates .DAMNHAVOK via SSFADF. 

## Prerequisites
* HKXCMD https://github.com/figment/hkxcmd
* SSFADF (also the Noesis plugin) https://github.com/Danilodum/dark_souls_hkx/releases
* Converted anims (BBAnimConverter in case of Bloodborne or where applicable) https://github.com/Sanadsk/BBAnimConverter
* Exported anims from Havok Content Tools (DS1 may need HCT 2010 32bit -- BB doesn't care) in XML format (DS1 anims may need to be saved as Win32)

## Usage
Place hkxcmd at the root directory.

Place SSFADF into the "files" folder.

Place all animations AND skeleton.hkx into the "files" folder.

Run.

All relevant files are put into "files/Out" folder. 
