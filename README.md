# Ordercloud <-> XM/XP PoC
## About the PoC
The idea of this PoC was, to showcase the general process, of how Sitecore XM/XP could be integrated with Ordercloud via Ordercloud C# SDK. The underlying idea is, that Sitecore and Ordercloud sync data between each others via manual processes or event triggered. In Sitecore corresponding Items are created and updated during that process to reflect Ordercloud data.
Once Sitecore Items are present, all kind of OOTB Sitecore functionalities and benefits are applicable to those Ordercloud related data as well, like full Content & Experience Editor Support. 

## Feature List
**Sync of Commerce Data**
 - Have scheduled (nightly) syncs from Ordercloud to Sitecore
 - Trigger sync manually on demand
 - Use Webhooks to inform Sitecore about changes in real time
 - Have all relevant Commerce data available and editable in Sitecore XM
 -  More will follow

**Sitecore Editorial Capabilities**
 - Edit Products and Variants together with Specifications,
 - Full native support of Ordercloud “XP” (Extended Properties)
 - Create and maintain whole Catalogs with Categories Products and Product references,
 - Real Time sync back to Ordercloud via Events
 - Full **WYSIWYG** Editor Support
 - More will follow

## Known Issues
Currently there are a few known issues, for which workarounds exist. In case you have a solution, don't hesitate to let me know :-D. Cause of some of theses issues, I really got strong headache

 1. Visual Studio Solution Publish does not work properly
 2. Ordercloud SDK needs a higher version of Newtonsoft, than Sitecore supports

**For 1.)**  If you use Visual Studio OOTB Publish in this solution you will end up in a situation, where deploy folder for container is full of new dlls, not only the visual studio project related AND that the config files of all the visual studio projects are not published properly to /include/***. The current workaround, and sorry for that, is to manually, after building the solution, copy over all the needed / changed dlls and config files. This is done for the provided initial code base, which you will see in /docker/deploy/platform. In case you make any changes to code or config, you will currently have to copy them over manually
**For 2.)** Because Ordercloud needs a higher version than Sitecore (In this PoC 10.2) provides, we need to patch NewtonsoftJson.dll and web.config. This is done in this example the "hard" way. In container deploy folder you will find in /bin the new version of newtonsoft.dll and in root folder some adapted web.config which points to the new DLL. **NOTICE: This web.config file, might need to be changed in your environment (TBH I am not sure about that)** 
