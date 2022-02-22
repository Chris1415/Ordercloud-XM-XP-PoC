# Ordercloud <-> XM/XP PoC
## About the PoC
The idea of this PoC was, to showcase the general process, of how Sitecore XM/XP could be integrated with Ordercloud via Ordercloud C# SDK. The underlying idea is, that Sitecore and Ordercloud sync data between each others via manual processes or event triggered. In Sitecore corresponding Items are created and updated during that process to reflect Ordercloud data.
Once Sitecore Items are present, all kind of OOTB Sitecore functionalities and benefits are applicable to those Ordercloud related data as well, like full Content & Experience Editor Support. 

## Presentations
- https://webinars.sitecore.com/sitecore-ordercloud-connector (German)

## Feature List
**Sync of Commerce Data**
 - Have scheduled (nightly) syncs from Ordercloud to Sitecore

![2022-02-19 14_36_38-Desktop](https://user-images.githubusercontent.com/17895694/154803115-dd130cc6-b44d-4b62-867f-d9c7a1aea63b.png)

 - Trigger sync manually on demand

https://user-images.githubusercontent.com/17895694/154803007-2b5159d9-7d4f-4b3b-951f-abfc1c0b4f23.mp4

![2022-02-19 14_40_12-JSS18Demo - Microsoft Visual Studio (Administrator)](https://user-images.githubusercontent.com/17895694/154803233-9d7e721f-7a12-4b5c-8628-9b50a25a8763.png)

 - Use Webhooks to inform Sitecore about changes in real time 

![2022-02-19 14_38_59-Sitecore OrderCloud _ API Console](https://user-images.githubusercontent.com/17895694/154803181-703d0954-d6a4-46d2-ab46-757f9f304d8c.png)

https://user-images.githubusercontent.com/17895694/154802672-fb52031b-e4ca-415e-8e71-ea43fa63e1f0.mp4


 - Have all relevant Commerce data available and editable in Sitecore XM
 
![2022-02-19 14_43_09-JSS18Demo - Microsoft Visual Studio (Administrator)](https://user-images.githubusercontent.com/17895694/154803317-3f3bd585-6037-4eb3-a284-bcdad66a91fd.png)

 -  More will follow

**Sitecore Editorial Capabilities**
 - Edit Products and Variants together with Specifications,

https://user-images.githubusercontent.com/17895694/154805466-73c6e90a-4753-4a6b-89fb-3584202f42ea.mp4

https://user-images.githubusercontent.com/17895694/154805679-caab98b3-d18d-454f-91cf-296a77f9ef29.mp4

 - Full native support of Ordercloud “XP” (Extended Properties)

https://user-images.githubusercontent.com/17895694/154804416-1dd1dfbe-d852-47e4-bf44-3959d00c6dc9.mp4

 - Create and maintain whole Catalogs with Categories Products and Product references,

https://user-images.githubusercontent.com/17895694/154804837-6def21d8-4827-4237-92e4-323068f3a96e.mp4

 - Full **WYSIWYG** Editor Support

https://user-images.githubusercontent.com/17895694/154804734-52455d55-1525-4295-9481-de5cb65cd3ac.mp4

 - Real Time sync back to Ordercloud via Events
 - More will follow

**Sitecore Headless Delivery**

- Deliver Product Information together with Variant specific information headlessly and statically into any Frontend

https://user-images.githubusercontent.com/17895694/154805834-661a2259-092b-4ef7-9ac3-f0059ddbb762.mp4

## Known Issues
Currently there are a few known issues, for which workarounds exist. In case you have a solution, don't hesitate to let me know :-D. Cause of some of theses issues, I really got strong headache

 1. Visual Studio Solution Publish does not work properly
 2. Ordercloud SDK needs a higher version of Newtonsoft, than Sitecore supports

**For 1.)**  If you use Visual Studio OOTB Publish in this solution you will end up in a situation, where deploy folder for container is full of new dlls, not only the visual studio project related AND that the config files of all the visual studio projects are not published properly to /include/***. The current workaround, and sorry for that, is to manually, after building the solution, copy over all the needed / changed dlls and config files. This is done for the provided initial code base, which you will see in /docker/deploy/platform. In case you make any changes to code or config, you will currently have to copy them over manually or use the DeployToDockerDeployFolder.ps1 script which is also included in up.ps1 script. If using this script, you first have to open the solution and build it and then uncomment the place in the up.ps1 script

**For 2.)** Because Ordercloud needs a higher version than Sitecore (In this PoC 10.2) provides, we need to patch NewtonsoftJson.dll and web.config. This is done in this example the "hard" way. In container deploy folder you will find in /bin the new version of newtonsoft.dll and in root folder some adapted web.config which points to the new DLL. **NOTICE: This web.config file, might need to be changed in your environment (TBH I am not sure about that)** 

## Limitations
This PoC is not fully implemented and is not meant to be used in production scenarios. Some of the business logic related parts are not fully implemented or need to be improved massivly. Examples
- When changing Spec Options like adding new option -> Products do not update autonatically
- On any Spec change Variant Items are removed and if necessary recreated -> Every CMS related information is lost
- No all possible events are implemented on all Entity / Item types
- Imports do not delete Items, if they are deleted in Ordercloud meanwhile and not transmitted via Webhooks
- On Item field change clear all caches is executed to let the Content Editor directly reflect these changes properly -> Massiv performance impact on first loads after Clear cache
